using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    [SerializeField] private Elevator[] elevators;
    [SerializeField] private WeightChecker[] weightCheckers;
    [SerializeField] private ElevatorSensor[] sensors;
    [SerializeField] private Rigidbody[] pulleys;
    [SerializeField] private float acceleration;
    [SerializeField] private float deacceleration;
    [SerializeField] private float maxSpeed;


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
    }
}
