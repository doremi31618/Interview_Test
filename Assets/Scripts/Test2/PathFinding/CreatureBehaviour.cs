using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RayPerception),typeof(Rigidbody))]
public class CreatureBehaviour : MonoBehaviour
{
    GameObjectManager creatureObjectManager;
    RayPerception m_rayPerception;

    //the thinking time interval
    public float thinkingSpeed = 0.2f;
    public float maxForce = 2;

    public bool isAutoAvoidCollision = true;
    [Tooltip("the bigger the more jagged")]public float smoothFactor = 0.5f;
    float seed;
    Rigidbody m_rig;

#region self move attribute
    Vector3 velocity = new Vector3();
    float acceleration;
    Vector3 moveForce;

#endregion

#region avoiding move attribute
    Vector3 avoidingForce;

#endregion

    // Start is called before the first frame update
    void Start()
    {
        m_rig = GetComponent<Rigidbody>();
        seed = Random.Range(-1.6382f,-1.6382f);
        creatureObjectManager = transform.parent.GetComponent<GameObjectManager>();
        m_rayPerception = transform.GetComponent<RayPerception>();
    }

    // Update is called once per frame
    void Update()
    {
       RandomMove();
       FinalCalcilate();
    }
    List<RayPerception.detectObject> getDataList(RayPerception _rayPerception)
    {
        return _rayPerception.detectList;
    }

    //process of decide avoiding force magnitude and direction
    IEnumerator AvoidingForce()
    {
        while(isAutoAvoidCollision)
        {
            if(getDataList(m_rayPerception).Count==0)continue;

            avoidingForce = new Vector3();
            for(int i = 0 ;i < getDataList(m_rayPerception).Count;i++)
            {   
                avoidingForce += getDataList(m_rayPerception)[i].obstacleDirection * -1 ;///寫到這裡
            }

            yield return new WaitForSeconds(thinkingSpeed);
        }
        
    }

    
   
    void RandomMove()
    {
        //self move 
         moveForce = new Vector3(
            //value -1 ~ 1
            ((Mathf.PerlinNoise(Time.time * smoothFactor  + seed ,Time.time * smoothFactor  + 1997 * seed)-0.5f) * 2),
            0,
            (Mathf.PerlinNoise(Time.time  * smoothFactor+ 1997 * seed ,Time.time * smoothFactor + seed)-0.5f) * 2
         ) * maxForce;
        
    }

    void FinalCalcilate()
    {
        transform.forward = moveForce ;
        
        acceleration = moveForce.magnitude ;
        velocity = transform.forward * acceleration * creatureObjectManager.all_CreatureData.speed;
        m_rig.velocity = velocity;
    }

    
}
