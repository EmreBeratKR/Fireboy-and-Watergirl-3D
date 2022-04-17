using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class WindParticle : MonoBehaviour
{
    public WindTurbine windTurbine;
    public BoxCollider bound;
    public SubWindParticle[] subParticles;
    public Vector2 deltaVelocityRangeX;
    public Vector2 deltaVelocityRangeY;
    public Vector2 initialSpeedRangeY;
    public int subParticleCount;

    private bool isStarted = false;
    
    private float leftBorder => bound.transform.localPosition.x + bound.center.x - bound.size.x * 0.5f;
    private float rightBorder => bound.transform.localPosition.x + bound.center.x + bound.size.x * 0.5f;
    private float topBorder => bound.transform.localPosition.y + bound.center.y + bound.size.y * 0.5f;
    private float bottomBorder => bound.transform.localPosition.y + bound.center.y - bound.size.y * 0.5f;

    private Vector3 randomInitialVelocity
    {
        get => Vector3.up * Random.Range(initialSpeedRangeY.x, initialSpeedRangeY.y);
    }

    private Vector3 randomInitialPosition
    {
        get => new Vector3(Random.Range(leftBorder, rightBorder), bottomBorder, 0);
    }
    

    private void Update()
    {
        Move();
    }

    private void Init()
    {
        foreach (var subParticle in subParticles)
        {
            //subParticle.Randomize();
            subParticle.transform.localPosition = randomInitialPosition;
            subParticle.velocity = randomInitialVelocity;
            subParticle.Clear();
        }

        isStarted = true;
    }

    private void Stop()
    {
        foreach (var subParticle in subParticles)
        {
            subParticle.transform.localPosition = randomInitialPosition;
            subParticle.velocity = Vector3.zero;
            subParticle.Clear();
        }

        isStarted = false;
    }
    
    private void Move()
    {
        if (!windTurbine.IsActive)
        {
            if (isStarted)
            {
                Stop();
            }
            
            return;
        }

        if (!isStarted)
        {
            Init();
        }
        
        foreach (var subParticle in subParticles)
        {
            var deltaX = Random.Range(deltaVelocityRangeX.x, deltaVelocityRangeX.y);
            var deltaY = Random.Range(deltaVelocityRangeY.x, deltaVelocityRangeY.y);

            var newVelocity = subParticle.velocity + new Vector3(deltaX, deltaY, 0);

            if (newVelocity.y < 0)
            {
                newVelocity.y = 0;
            }

            subParticle.velocity = newVelocity;

            var lastPosition = subParticle.transform.localPosition;
            

            bool isResetted = false;

            if (lastPosition.y > topBorder)
            {
                //subParticle.Randomize();
                lastPosition = randomInitialPosition;
                subParticle.velocity = randomInitialVelocity;
                isResetted = true;
            }

            if (lastPosition.x > rightBorder)
            {
                lastPosition.x = rightBorder;
                newVelocity.x = Mathf.Abs(newVelocity.x) * -1;
                subParticle.velocity = newVelocity;
            }
            else if (lastPosition.x < leftBorder)
            {
                lastPosition.x = leftBorder;
                newVelocity.x = Mathf.Abs(newVelocity.x) * -1;
                subParticle.velocity = newVelocity;
            }

            subParticle.transform.localPosition = lastPosition;
            
            if (isResetted) subParticle.Clear();
        }
    }   
}
