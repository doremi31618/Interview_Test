using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///流程：
///1.確定每一幀的操控力不大於最大值
///2.除以質量獲得加速度
///3.將加速度與原速度相加並確保不超過最大速度
///4.根據速度與每一針的時間來算出單桅時間的位移量
///5.與原來的位置相加使物體移動
public class Vehicle : MonoBehaviour
{
    Steering[] steerings;

    public float mass = 1;
    public float maxSpeed = 10;
    public float maxForce = 100;
    float sqrMaxForce;
    //the truning speed
    public float damping = 0.9f;
    public float computeInterval = 0.2f;
    public bool isPlaner = true;

    protected Vector3 steeringForce;
    protected Vector3 acceleration;
    [HideInInspector] public Vector3 velocity;
    float timer;
    // Start is called before the first frame update
    protected void Start()
    {
        steeringForce = Vector3.zero;
        sqrMaxForce = Mathf.Sqrt(maxForce);
        timer = 0;
        steerings = GetComponents<Steering>();
    
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        steeringForce = Vector3.zero;

        if(timer >= computeInterval)
        {
            foreach (Steering s in steerings)
            {
                if (s.enabled)
                    steeringForce += s.Force() * s.weight;
                //Debug.Log(s.Force());
            }

            timer = 0;
            steeringForce = Vector3.ClampMagnitude(steeringForce,maxForce);
            acceleration = steeringForce/mass;
            
        }
    }
}
