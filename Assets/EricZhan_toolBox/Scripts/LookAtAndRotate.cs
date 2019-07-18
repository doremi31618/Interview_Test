using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtAndRotate : MonoBehaviour
{
    public GameObject lookAt;
    public float speed = 1;
    public float radius = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right*Time.deltaTime;
        transform.position = transform.position.normalized * radius;
        transform.LookAt(lookAt.transform);
    }
}
