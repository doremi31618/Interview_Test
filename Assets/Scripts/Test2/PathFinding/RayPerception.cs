using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode][RequireComponent(typeof(LineRenderer))]
public class RayPerception : MonoBehaviour
{

    public  GameObjectManager creatureObjectManager;
    public LineRenderer m_lineRender;
    public LayerMask detectLayer;

    public bool isDetectSomething= false;
    //public List<detectObject> detectList = new List<detectObject>();
    public detectObject closestTarget;
    public List<Vector3> nonDetectLaserList = new List<Vector3>();
    [System.Serializable]public struct detectObject
    {
        public Transform detectObjectTransform;
        public float distanceToDetectPoint;
        public Vector3 hitPoint;
        public Vector3 obstacleDirection;

        public Vector3 obstacleVelocity;
    }

#region private attribute
    float adjustmentAngle;
   
#endregion
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        RayGenorator();
    }

    void Initialize()
    {
        //Initialize parent GameObject
        if(creatureObjectManager == null)creatureObjectManager = transform.parent.gameObject.GetComponent< GameObjectManager>();

        //Initialize attribute;
        
    }

    void RayGenorator()
    {
        if(creatureObjectManager == null)creatureObjectManager = transform.parent.gameObject.GetComponent< GameObjectManager>();
        // if(!creatureObjectManager.all_CreatureData.debugVisualizer)
        // {
        //     m_lineRender.enabled = false;
        //     return;
        // }
        // else if(creatureObjectManager.all_CreatureData.debugVisualizer && !m_lineRender.enabled)
        // {
        //     m_lineRender.enabled = true;
        // }
        float adjustmentAngle = (180 - creatureObjectManager.all_CreatureData.eyeSight) * 0.5f - transform.rotation.eulerAngles.y;
        
        ///setting up line render
        if(creatureObjectManager.all_CreatureData.debugVisualizer)
        {
            m_lineRender.positionCount = (creatureObjectManager.all_CreatureData.rayNumber ) * 2;

            //setting width
            m_lineRender.startWidth = creatureObjectManager.all_CreatureData.rayWidth;
            m_lineRender.endWidth = creatureObjectManager.all_CreatureData.rayWidth;

            //setting color
            Color _rayColor = creatureObjectManager.all_CreatureData.rayColor;
            _rayColor.a = creatureObjectManager.all_CreatureData.debugVisualizer ? 255 : 0;
            m_lineRender.startColor = _rayColor;
            m_lineRender.endColor = _rayColor;
        }
        
        //detectList.Clear();
        nonDetectLaserList.Clear();

        //temp region
        int detectNumber = 0;
        //
        //render laser
        closestTarget = new detectObject();
        for(int i = 0 ; i<creatureObjectManager.all_CreatureData.rayNumber;i++)
        {

            float a = creatureObjectManager.all_CreatureData.eyeSight * i/(creatureObjectManager.all_CreatureData.rayNumber-1) + adjustmentAngle ;
            
            float x = creatureObjectManager.all_CreatureData.viewDistance * Mathf.Cos(Mathf.Deg2Rad * a) ;
            float z = creatureObjectManager.all_CreatureData.viewDistance * Mathf.Sin(Mathf.Deg2Rad * a) ;
            float y = 0;
            Vector3 dir = new Vector3(x,y,z);

            Ray ray = new Ray(transform.position,dir);
            RaycastHit hit;

            

            //detect if hit something
            if(Physics.Raycast(ray,out hit, creatureObjectManager.all_CreatureData.viewDistance, detectLayer))
            {
                

                if(creatureObjectManager.all_CreatureData.debugVisualizer)
                {
                    m_lineRender.SetPosition(i*2, transform.position);
                    m_lineRender.SetPosition(i*2+1, hit.point);
                }
                
                
                detectObject _detectObject = new detectObject();
                _detectObject.detectObjectTransform = hit.transform;
                _detectObject.distanceToDetectPoint = (hit.point - transform.position).magnitude;
                _detectObject.hitPoint = hit.point;
                _detectObject.obstacleDirection = hit.point - transform.position;
                _detectObject.obstacleVelocity = hit.transform.GetComponent<Rigidbody>().velocity;

                if(closestTarget.distanceToDetectPoint>_detectObject.distanceToDetectPoint || closestTarget.detectObjectTransform == null)
                {
                    closestTarget = _detectObject;
                }
                // detectList.Add(_detectObject);
                detectNumber +=1;

                //Debug.DrawRay(transform.position,hit.point - transform.position,creatureObjectManager.all_CreatureData.rayColor,creatureObjectManager.all_CreatureData.viewDistance);
            }
            else
            {
                if(creatureObjectManager.all_CreatureData.debugVisualizer)
                {
                    m_lineRender.SetPosition(i*2, transform.position);
                    m_lineRender.SetPosition(i*2+1, dir + transform.position);
                }

                nonDetectLaserList.Add(dir);
                //Debug.DrawRay(transform.position,dir,creatureObjectManager.all_CreatureData.rayColor,creatureObjectManager.all_CreatureData.viewDistance);
            }
            
            
        }
        isDetectSomething = detectNumber !=0;
    }
    


}
