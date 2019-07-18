using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//use Craig Reynoids
public class SteeringForWnader : Steering
{
    bool isPlaner;
    public float wanderRadius;
    public float wnaderDistance;

    //max random movement per second
    public float wanderJitter;

    Vector3 IdealVelocity;
    Vehicle m_vehicle;
    float maxSpeed;
    Vector3 circleTarget;
    Vector3 wanderTarget;
    RayPerception m_rayPerception;


    // Start is called before the first frame update
    void Start()
    {
        m_vehicle = GetComponent<Vehicle>();
        m_rayPerception = GetComponent<RayPerception>();
        maxSpeed = m_vehicle.maxSpeed;
        isPlaner = m_vehicle.isPlaner;
        circleTarget = new Vector3(wanderRadius * 0.707f, 0 ,wanderRadius * 0.707f);

    }
    public override Vector3 Force()
    {
        if(m_rayPerception.isDetectSomething)
        {
            return Vector3.zero;
        }
        Vector3 randomDisplacement = new Vector3(
            (Random.value-0.5f) * 2 * wanderJitter,
            (Random.value-0.5f) * 2 * wanderJitter,
            (Random.value-0.5f) * 2 * wanderJitter
        );

        if(isPlaner)randomDisplacement.y = 0;
        circleTarget += randomDisplacement;
        circleTarget = wanderRadius * circleTarget.normalized;
        wanderTarget = m_vehicle.velocity.normalized * wnaderDistance + transform.position + circleTarget;
        IdealVelocity = (wanderTarget - transform.position).normalized * maxSpeed;
        //Debug.Log(IdealVelocity - m_vehicle.velocity);
        Debug.DrawRay(transform.position ,IdealVelocity - m_vehicle.velocity,Color.cyan);
        return IdealVelocity - m_vehicle.velocity;
    }
}
