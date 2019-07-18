
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;


public class  DrawLineAndVertex : MonoBehaviour
{
   // When added to an object, draws colored rays from the
    // transform position.
   
    public float radius = 3.0f;
    public float compute_Interval = 0.1f;
    public float radius_Magnification = 1.0f;
    public int horizontalDataNumber = 60;
    public float speed = 1;
    [Range(0,360)]public float displayAngle = 360;
    public GameObject centerObject;
    public GameObject StartPointParentObject;

    public bool isUseCustomData = false;
    public  Transform[] startPoints = new Transform[0];
    public vertexData[,] net_visualizer;

    public int lineCount = 100;
    static Material lineMaterial;
    
    [System.Serializable]public struct vertexData
    {
        //local position;
        public Vector3 position;
        public float radius;
        public float height;

    }
    private void Start() {

        if(centerObject == null)
        {
            GameObject center = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            center.name = "Center";
            center.transform.parent = this.transform;
            center.transform.eulerAngles = Vector3.down * 180;
            center.transform.localPosition = Vector3.forward * 6;

        }

        if(startPoints.Length == 0 && StartPointParentObject == null)
        {
           GameObject StartPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
           StartPoint.name = "StartPoint";
           StartPoint.transform.parent = this.transform;
           StartPoint.transform.localPosition = Vector3.zero;

           GameObject StartPoint_1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
           StartPoint_1.name = "StartPoint_1";
           StartPoint_1.transform.parent = StartPoint.transform;
           StartPoint_1.transform.localPosition = Vector3.left * 6;

        }
        else if(StartPointParentObject != null && !isUseCustomData)
        {
            startPoints = StartPointParentObject.transform.GetComponentsInChildren<Transform>();
        }

        net_visualizer = new vertexData[startPoints.Length,horizontalDataNumber];
        //ignore x = 0, because it will get parent transform
        for(int x =1;x<net_visualizer.GetLength(0);x++)
        {
            float height = startPoints[x].position.y;
            for(int y = 0;y<net_visualizer.GetLength(1);y++)
            {
                vertexData pointData = new vertexData();
                if(y==0)
                {
                    
                    Vector3 point = pointData.position = new Vector3(
                        startPoints[x].position.x,
                        startPoints[x].position.y,
                        startPoints[x].position.z);
                    
                    Vector3 towardVecotor = centerObject.transform.position - point;

                    pointData.height = height = point.y;
                    towardVecotor.y=0;
                    pointData.radius = towardVecotor.magnitude;
                }
                else
                {
                    
                    pointData.height = net_visualizer[x,y-1].height;
                    pointData.radius = net_visualizer[x,y-1].radius;

                    float degree = (360/horizontalDataNumber) * y;

                    if(degree > displayAngle)continue;
                    degree *= Mathf.Deg2Rad;

                    Vector3 pos = Quaternion.AngleAxis(-360/horizontalDataNumber,Vector3.up) * net_visualizer[x,y-1].position;

                    pointData.position = pos;
                }
                
                net_visualizer[x,y] = pointData;

            }
        }
        StartCoroutine(dataPassing());
        
    }
    IEnumerator dataPassing()
    {
        while(true)
        {
            for(int x =1;x<net_visualizer.GetLength(0);x++)
            {
                float height = startPoints[x].position.y;
                vertexData lastData = new vertexData();
                for(int y = 0;y<net_visualizer.GetLength(1);y++)
                {
                    //vertexData pointData = new vertexData();

                    if(y==0)
                    {
                        
                        // Vector3 point = new Vector3(
                        //     startPoints[x].position.x,
                        //     startPoints[x].position.y,
                        //     startPoints[x].position.z);
                        
                        Vector3 towardVecotor = startPoints[x].position - centerObject.transform.position ;

                        net_visualizer[x,y].height =startPoints[x].position.y;
                        towardVecotor.y=0;
                        net_visualizer[x,y].radius = towardVecotor.magnitude;
                        lastData = net_visualizer[x,y];
                    }
                    else
                    {
                        vertexData _pointData = new vertexData();
                        _pointData = net_visualizer[x,y];

                        net_visualizer[x,y].height = lastData.height;
                        net_visualizer[x,y].radius = lastData.radius;

                        float degree = (360/horizontalDataNumber) * y;

                        if(degree > displayAngle)continue;
                        degree *= Mathf.Deg2Rad;
                        lastData.position.y = 0;
                        Vector3 pos = Quaternion.AngleAxis(-360/horizontalDataNumber,Vector3.up) * lastData.position;

                        pos = pos.normalized *  net_visualizer[x,y].radius;
                        pos.y = net_visualizer[x,y].height;
                        net_visualizer[x,y].position = pos;
                        lastData = _pointData;

                    }
                    
                  

                }
            }
            yield return new WaitForSeconds(compute_Interval * 1/speed);
        }
       
    }
     

    static void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;

            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);

            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }

    // Will be called after all regular rendering is done
    public void OnRenderObject()
    {
        CreateLineMaterial();
        // Apply the line material
        lineMaterial.SetPass(0);

        GL.PushMatrix();
        
        // Set transformation matrix for drawing to
        // match our transform
        GL.MultMatrix(transform.localToWorldMatrix);

        
        GL.Begin(GL.LINES);
        for(int x =1;x<net_visualizer.GetLength(0);x++)
        {
            bool isCorner = (x == net_visualizer.GetLength(0)-1);
            for(int y = 0;y<net_visualizer.GetLength(1);y++)
            {
                //check out where the vertex is 
                //left up --------------right up 
                //|                        |
                //|                        |
                //|          other         |
                //|                        |
                //|                        |
                //left down--------------right down
                
                //leftup/other->        right up
                // |                       |
                // v                       v
                //left down->           right down(draw nothing)
                
                bool isLastOne = (y==net_visualizer.GetLength(1)-1);
                bool isFirstOne = (y==0);
                bool isVectorZero = (net_visualizer[x,y].position == Vector3.zero);
                
                //setting color 
                if(isLastOne||isFirstOne||isVectorZero)
                {
                    
                    GL.Color(new Color(0, 0, 0, 0));
                }
                else
                {
                    GL.Color(new Color(255, 255, 255, 0.8F));
                }

                //draw lines
                //left down
                if(isCorner && !isLastOne)
                {
                    if(net_visualizer[x,y+1].position.x +net_visualizer[x,y+1].position.y+net_visualizer[x,y+1].position.z ==0)
                        GL.Color(new Color(0, 0, 0, 0));
                    
                    GL.Vertex3(
                        net_visualizer[x,y+1].position.x,
                        net_visualizer[x,y+1].position.y,
                        net_visualizer[x,y+1].position.z
                    );

                }
                //right up
                else if(isLastOne && !isCorner) 
                {
                    GL.Vertex3(
                        net_visualizer[x+1,y].position.x,
                        net_visualizer[x+1,y].position.y,
                        net_visualizer[x+1,y].position.z
                    );
                }
                //right down
                else if(isLastOne && isCorner)
                {
                    GL.Vertex3(
                    net_visualizer[x,y].position.x,
                    net_visualizer[x,y].position.y,
                    net_visualizer[x,y].position.z
                    );
                    continue;
                }
                //leftup / other
                else
                {
                    if(net_visualizer[x,y+1].position.x +net_visualizer[x,y+1].position.y+net_visualizer[x,y+1].position.z ==0)
                        GL.Color(new Color(0, 0, 0, 0));

                    GL.Vertex3(
                        net_visualizer[x,y+1].position.x,
                        net_visualizer[x,y+1].position.y,
                        net_visualizer[x,y+1].position.z
                    );

                    
                    
                    GL.Vertex3(
                        net_visualizer[x,y].position.x,
                        net_visualizer[x,y].position.y,
                        net_visualizer[x,y].position.z
                    );

                    GL.Color(new Color(255, 255, 255, 0.8F));

                    GL.Vertex3(
                        net_visualizer[x+1,y].position.x,
                        net_visualizer[x+1,y].position.y,
                        net_visualizer[x+1,y].position.z
                    );
                }

                GL.Vertex3(
                    net_visualizer[x,y].position.x,
                    net_visualizer[x,y].position.y,
                    net_visualizer[x,y].position.z
                );
                
                
            }
        }
        GL.End();
        
        GL.PopMatrix();
    }
}
