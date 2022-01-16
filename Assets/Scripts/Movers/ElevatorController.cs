using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class ElevatorController : EventListener
{
    [SerializeField] private Elevator[] elevators;
    [SerializeField] private WeightChecker[] weightCheckers;
    [SerializeField] private ElevatorSensor[] sensors;
    [SerializeField] private Rigidbody[] pulleys;
    [SerializeField] private float acceleration;
    [SerializeField] private float deacceleration;
    [SerializeField] private float maxSpeed;
    private const float maxDifference = 1f;


    private void Start()
    {
        targetEvent = OnElevatorEvent;
    }

    private void FixedUpdate()
    {
        float firstWeight = weightCheckers[0].TotalWeight();
        float secondWeight = weightCheckers[1].TotalWeight();
        float netWeight = firstWeight - secondWeight;

        if (System.Convert.ToInt32(netWeight) == 0f)
        {
            foreach (var elevator in elevators)
            {
                elevator.velocity -= elevator.velocity.normalized * deacceleration * Time.fixedDeltaTime;
            }
        }

        elevators[0].velocity += Vector3.down * netWeight * acceleration * Time.fixedDeltaTime;
        elevators[1].velocity += Vector3.up * netWeight * acceleration * Time.fixedDeltaTime;

        if (elevators[0].velocity.magnitude > maxSpeed)
        {
            elevators[0].velocity = elevators[0].velocity.normalized * maxSpeed;
        }

        if (elevators[1].velocity.magnitude > maxSpeed)
        {
            elevators[1].velocity = elevators[1].velocity.normalized * maxSpeed;
        }

        if (elevators[0].velocity.normalized == Vector3.down)
        {
            if ((sensors[0].limit - sensors[0].elevator.position).normalized == Vector3.up)
            {
                elevators[0].velocity = Vector3.zero;
                elevators[1].velocity = Vector3.zero;
            }
        }
        else if (elevators[1].velocity.normalized == Vector3.down)
        {
            if ((sensors[1].limit - sensors[1].elevator.position).normalized == Vector3.up)
            {
                elevators[0].velocity = Vector3.zero;
                elevators[1].velocity = Vector3.zero;
            }
        }

        foreach (var pulley in pulleys)
        {
            pulley.angularVelocity = Vector3.back * elevators[0].velocity.y;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            Raise_ElevatorEvent();
        }
    }

    private void OnElevatorEvent(EventData obj)
    {
        if (obj.Code == EventCode._ELEVATOR_EVENTCODE)
        {
            object[] datas = (object[]) obj.CustomData;
            Vector3 _pos0 = (Vector3) datas[0];
            Vector3 _pos1 = (Vector3) datas[1];
            Vector3 _vel0 = (Vector3) datas[2];
            Vector3 _vel1 = (Vector3) datas[3];

            if ((elevators[0].transform.position - _pos0).magnitude >= maxDifference || (elevators[1].transform.position - _pos1).magnitude >= maxDifference ||
                System.Convert.ToInt32(elevators[0].velocity.magnitude) == 0 || System.Convert.ToInt32(elevators[1].velocity.magnitude) == 0)
            {
                elevators[0].transform.position = _pos0;
                elevators[0].velocity = _vel0;
                elevators[1].transform.position = _pos1;
                elevators[1].velocity = _vel1;
            }
        }
    }

    public void Raise_ElevatorEvent()
    {
        object[] datas = new object[] {elevators[0].transform.position, elevators[1].transform.position, elevators[0].velocity, elevators[1].velocity};
        PhotonNetwork.RaiseEvent(EventCode._ELEVATOR_EVENTCODE, datas, RaiseEventOptions.Default, SendOptions.SendUnreliable);
    }
}
