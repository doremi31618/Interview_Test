using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// 追蹤力是三種力的混合
/// 1.逃避
/// 2.抵達
/// 3.離開
public class SteeringForFollowLeader : Steering
{
    public float followForce = 10;
    public float minDistanceToLeader = 1;
    public float slowdownDistance = 2;
    RayPerception m_rayPerception;
    GameObjectManager m_manager;
    Vehicle m_vehicle;

    bool isPlaner ;
    float maxSpeed;
    Vector3 desiredVelocity;

    void Start()
    {
        m_manager = transform.parent.GetComponent<GameObjectManager>();
        m_rayPerception = GetComponent<RayPerception>();
        m_vehicle = GetComponent<Vehicle>();

        isPlaner = m_vehicle.isPlaner;
        maxSpeed = m_vehicle.maxSpeed;
    }

    Vector3 forllowForce()
    {
        GameObject leader = m_manager.all_CreatureData.Leader;
        
        if(leader == null)
            return Vector3.zero;
        else
        {
            Vector3 vecotrToLeader = leader.transform.position - transform.position;
            Vector3 returnForce = Vector3.zero;
            desiredVelocity.y = isPlaner ? 0 : desiredVelocity.y ;
            float distanceToLeader = vecotrToLeader.magnitude;
            desiredVelocity = (distanceToLeader > slowdownDistance)?vecotrToLeader.normalized * maxSpeed:vecotrToLeader - m_vehicle.velocity;
            returnForce = desiredVelocity - m_vehicle.velocity;
            return returnForce;
            // if(m_rayPerception.closestTarget.detectObjectTransform == null)
            // {
            //     return returnForce;
            // }
            // else 
            // {
                
            //     Vector3 velocity = m_vehicle.velocity;
            //     Vector3 normalizedVelocity = velocity.normalized;
            //     Vector3 ahead = transform.position + normalizedVelocity * slowdownDistance;
            //     Vector3 force = ahead - m_rayPerception.closestTarget.detectObjectTransform.position;
            //     force *= followForce;
            //     return force;

            // }
            
        }
    }

    public override Vector3 Force()
    {
        return forllowForce();
    }

}
