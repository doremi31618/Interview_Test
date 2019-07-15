using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeWaterSurface : MonoBehaviour
{
    public Vector2 textureResolution;
    public List<GameObject> cubeSurface;
    public int maxCubeNumber = 10000;
    public float Magnification = 5;
    public WaterSimulation waterData;
    public Material cubeMaterial;
    // Start is called before the first frame update
    void Start()
    {
        waterData = GetComponent<WaterSimulation>();
        //water = GetComponent<Renderer>().material.GetTexture(0);
        //textureResolution = new Vector2(waterData.myTexture2D.width,waterData.myTexture2D.height);
        for(int x = (int)-textureResolution.x/2;x<(int)textureResolution.x/2;x++)
        {
            for(int y = (int)textureResolution.y/2;y>(int)-textureResolution.y/2;y--)
            {
                if(cubeSurface.Count >= maxCubeNumber)break;
                GameObject cubePixel = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cubePixel.transform.parent = this.transform;
                cubePixel.transform.position = new Vector3(x,-5,y);
                cubePixel.layer = 0;
                cubePixel.GetComponent<Renderer>().material = cubeMaterial;
                cubeSurface.Add(cubePixel);
            }

            if(cubeSurface.Count >= maxCubeNumber)break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        for(int i = 0 ;i<cubeSurface.Count;i++)
        {
            Color pixelColor =  waterData.myTexture2D.GetPixel(
                (int)(-(cubeSurface[i].transform.position.x + waterData.myTexture2D.width/2)),
                (int)(waterData.myTexture2D.height/2 - cubeSurface[i].transform.position.z));
            float yValue = ((pixelColor.g)) * Magnification + 0.1f;
            //Debug.Log(yValue);
            cubeSurface[i].transform.localScale = new Vector3(
                cubeSurface[i].transform.localScale.x,
                yValue,
                cubeSurface[i].transform.localScale.z);
        }

    }
    private void OnGUI() {
        GUI.Label(new Rect(10, 25, 100, 50),"Magnification" );
        Magnification = GUI.HorizontalSlider(new Rect(87, 30, 100, 20), Magnification, 1F, 30F);

        // GUI.Label(new Rect(10, 45, 100, 50),"Wave Height" );
        // waveHeight = GUI.HorizontalSlider(new Rect(87, 50, 100, 20), waveHeight, 1.0F, 15F);

        // GUI.Label(new Rect(10, 65, 100, 50),"Wave Speed" );
        // waveSpeed = GUI.HorizontalSlider(new Rect(87, 70, 100, 20), waveSpeed, 1.0F, 5F);
    }
}
