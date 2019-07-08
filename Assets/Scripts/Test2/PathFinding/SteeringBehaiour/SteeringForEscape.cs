using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringForEscape : Steering
{
    RayPerception m_rayPerception;
    Vector3 IdealVelocity;
    Vehicle m_vehicle;
    float maxSpeed;
    bool isPlaner = false;

    public float avoidanceForce = 2.0f;

    // Start is called before the first frame update
    void Start()
    {

        m_vehicle = GetComponent<Vehicle>();
        m_rayPerception = GetComponent<RayPerception>();
        maxSpeed = m_vehicle.maxSpeed;
        isPlaner = m_vehicle.isPlaner;

    }

    public override Vector3 Force()
    {
        if(!m_rayPerception.isDetectSomething)
        {
            return Vector3.zero;
        }

        RayPerception.detectObject closestTarget = m_rayPerception.closestTarget;

        float closetDistance = closestTarget.distanceToDetectPoint;

        Vector3 velocity = m_vehicle.velocity;
        Vector3 normalizeVelocity = velocity.normalized;
        Vector3 ahead = transform.position +  normalizeVelocity * closetDistance * (velocity.magnitude/maxSpeed);
        
        

        Vector3 force = (ahead  - closestTarget.hitPoint );
        Debug.DrawRay(transform.position,force * 10,Color.red);
        force *= avoidanceForce;
        if(isPlaner)force.y = 0;

        //m_rayPerception.isDetectSomething ? force : Vector3.zero
        return  force ;

    }
    
}
