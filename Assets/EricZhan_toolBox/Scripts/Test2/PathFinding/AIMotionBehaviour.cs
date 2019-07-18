using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMotionBehaviour : Vehicle
{
    GameObjectManager m_manager;
    Vector3 moveDistance;
    Rigidbody m_rigbody;

    // Start is called before the first frame update
    void Start()
    {
        m_rigbody = GetComponent<Rigidbody>();
        m_manager = transform.parent.GetComponent<GameObjectManager>();
        moveDistance = Vector3.zero;
        if(!m_manager .all_CreatureData.trailVisualizer)GetComponent<TrailRenderer>().enabled = false;
        base.Start();
    }
    // void Update()
    // {
    //     timer += Time.deltaTime;
    //     steeringForce = Vector3.zero;

    //     if(timer >= computeInterval)
    //     {
    //         foreach (Steering s in steerings)
    //         {
    //             if (s.enabled)
    //                 steeringForce += s.Force() * s.weight;
    //         }

    //         timer = 0;
    //         steeringForce = Vector3.ClampMagnitude(steeringForce,maxForce);
    //         acceleration = steeringForce/mass;
            
    //     }
    // }
    void FixedUpdate()
    {
       //Debug.Log(acceleration);
       GetComponent<TrailRenderer>().enabled = m_manager.all_CreatureData.trailVisualizer;
        velocity += acceleration * Time.fixedDeltaTime;
        if(velocity.magnitude > maxSpeed)
        {
            velocity = velocity.normalized * maxSpeed;
            
        }
        //Debug.Log(velocity);
        moveDistance = velocity * Time.fixedDeltaTime * m_manager.all_CreatureData.speed;

        if(isPlaner)velocity.y = moveDistance.y = 0;

        if(m_rigbody == null || m_rigbody.isKinematic)
        {
            transform.position += moveDistance;
        }
        else
        { 
            m_rigbody.MovePosition(transform.position + moveDistance);
        }

        //prevent jitter 防止抖動！！！
        if(velocity.sqrMagnitude > 0.0001f)
        {
            Vector3 newforwrd = Vector3.Slerp(transform.forward,velocity,damping * Time.deltaTime);
            if(isPlaner)newforwrd .y = 0;
            transform.forward = newforwrd;
        }

    }
}
