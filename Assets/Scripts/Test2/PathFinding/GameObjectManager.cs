using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//refence : https://www.storynest.com/pix/_5exp/e_2014_0417_roach/p0.php?lang=ch

[System.Serializable]
public struct CreatureData
{
    [Header("Creature Perception Attribbute")]
    [Tooltip("the acurate of ray")][Range(3,30)]public int rayNumber;
    [Range(0.01f,1)]public float rayWidth;
    [Tooltip("the angle of eyes perception")][Range(90,270)]public float eyeSight;
    [Tooltip("the distance of eyes perception")][Range(1,15)]public float viewDistance;
    public Color rayColor;
    public bool debugVisualizer;

    [Header("Creature Motion Atrribte")]
    [Tooltip("directly adjust the creature global speed")][Range(0.5f,2)]public float speed;
    public bool trailVisualizer;
}
public class GameObjectManager : MonoBehaviour
{
    // 生產器有bug，應該是複製的時間跟初始化順序有衝突
    [Header("GameObject Generate")]
    public GameObject prefab;
    public int number = 10;

    public float RandomGenerateRadius = 5f;
    public CreatureData all_CreatureData;

    // // Start is called before the first frame update
    void Awake()
    {
        if(prefab == null)return;
        for(int i = 0 ;i<number;i++)
        {
            GameObject _clonePrefab = Instantiate(prefab) as GameObject;
            Vector3 newPosition = Random.insideUnitSphere * RandomGenerateRadius + prefab.transform.position;
            newPosition.y = 0;
            _clonePrefab.transform.position = newPosition;
            _clonePrefab.transform.parent = this.transform;
        }
    }
    // void Start()
    // {
       
    // }


}
