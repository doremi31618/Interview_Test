using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//refence : https://www.storynest.com/pix/_5exp/e_2014_0417_roach/p0.php?lang=ch

[System.Serializable]
public struct CreatureData
{
    [Header("Creature Perception Attribbute")]
    [Tooltip("the acurate of ray")][Range(3,100)]public int rayNumber;
    [Range(0.01f,1)]public float rayWidth;
    [Tooltip("the angle of eyes perception")][Range(90,270)]public float eyeSight;
    [Tooltip("the distance of eyes perception")][Range(1,15)]public float viewDistance;
    public Color rayColor;
    public bool debugVisualizer;

    [Header("Creature Motion Atrribte")]
    public float speed;
    public bool trailVisualizer;
}
public class GameObjectManager : MonoBehaviour
{
    
    [Header("Creature Generate")]
    public int nuberOfCreatures = 30;
    public GameObject prefab;

    public CreatureData all_CreatureData;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
