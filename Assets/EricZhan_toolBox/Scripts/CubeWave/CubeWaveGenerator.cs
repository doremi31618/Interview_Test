using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeWaveGenerator : MonoBehaviour
{
    public GameObject prefab;
    public GameObject prefab2;
    public int width = 10;
    public int height = 10;
    public float waveSpeed = 1;
    public float interval = 0;
    public float tileDegree = 1;
    public float waveHeight = 1;
    public List<GameObject> cubeSea;
    public List<GameObject> cubeSea2;

    // Start is called before the first frame update
    void Start()
    {
        for(int x = -width/2;x<width/2;x++)
        {
            for(int y = -height/2;y<height/2;y++)
            {
                GameObject clone = Instantiate(prefab) as GameObject;
                clone.transform.parent = this.transform;
                clone.transform.position = new Vector3(x + interval ,0,y + interval);
                cubeSea.Add(clone);
            }
        }

        for(int x = -2 * width;x<2 * width;x++)
        {
            for(int y = -2 * height;y<2 * height;y++)
            {
                GameObject clone = Instantiate(prefab2) as GameObject;
                clone.transform.parent = this.transform;
                clone.transform.position = new Vector3(x + interval ,-10,y + interval);
                cubeSea2.Add(clone);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0;i<cubeSea.Count;i++)
        {
            cubeSea[i].transform.localScale= new Vector3(
                cubeSea[i].transform.localScale.x,
                waveHeight * Mathf.PerlinNoise((Time.time*waveSpeed + cubeSea[i].transform.position.x)*tileDegree,(Time.time*waveSpeed + cubeSea[i].transform.position.z)*tileDegree),
                cubeSea[i].transform.localScale.z );
        }
        for(int i = 0;i<cubeSea2.Count;i++)
        {
            cubeSea2[i].transform.localScale= new Vector3(
                cubeSea2[i].transform.localScale.x,
                waveHeight * Mathf.PerlinNoise((Time.time*waveSpeed + cubeSea2[i].transform.position.x)*tileDegree,(Time.time*waveSpeed + cubeSea2[i].transform.position.z)*tileDegree),
                cubeSea2[i].transform.localScale.z );
        }
    }
    private void OnGUI() {
        GUI.Label(new Rect(10, 25, 100, 50),"Tile Degree" );
        tileDegree = GUI.HorizontalSlider(new Rect(87, 30, 100, 20), tileDegree, 0.0F, 0.3F);

        GUI.Label(new Rect(10, 45, 100, 50),"Wave Height" );
        waveHeight = GUI.HorizontalSlider(new Rect(87, 50, 100, 20), waveHeight, 1.0F, 15F);

        GUI.Label(new Rect(10, 65, 100, 50),"Wave Speed" );
        waveSpeed = GUI.HorizontalSlider(new Rect(87, 70, 100, 20), waveSpeed, 1.0F, 5F);
    }
}
