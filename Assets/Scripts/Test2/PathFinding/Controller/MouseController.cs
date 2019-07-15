using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public GameObject LeaderPrefab;
    GameObjectManager m_manager;
    public LayerMask detectLayer;
    
    float timer = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        m_manager = GetComponent<GameObjectManager>();
        timer = 0;
        LeaderPrefab.transform.position = new Vector3(LeaderPrefab.transform.position.x,
                                                    Mathf.Lerp(-LeaderPrefab.transform.localScale.y/2, LeaderPrefab.transform.localScale.y/2,timer/3),
                                                    LeaderPrefab.transform.position.z);
    }

    private void Update() {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, detectLayer))
        {
            Debug.DrawLine(Camera.main.transform.position, hit.point);
            float _y = Mathf.Lerp(-LeaderPrefab.transform.localScale.y / 2, LeaderPrefab.transform.localScale.y / 2, timer / 3);
            LeaderPrefab.transform.position = new Vector3(hit.point.x, _y, hit.point.z);
        }

        if(Input.GetMouseButton(0))
        {

            //Debug.DrawLine(Camera.main.transform.position, hit.point);
            if (timer <= 3)
                timer += Time.deltaTime;
            float _y = Mathf.Lerp(-LeaderPrefab.transform.localScale.y / 2, LeaderPrefab.transform.localScale.y / 2, timer / 3);
            LeaderPrefab.transform.position = new Vector3(hit.point.x, _y, hit.point.z);

            m_manager.all_CreatureData.Leader = LeaderPrefab;
            
        }
        else if(Input.GetMouseButtonUp(0))
        {
            timer = 0;
            LeaderPrefab.transform.position = new Vector3(LeaderPrefab.transform.position.x,
                                                    Mathf.Lerp(-LeaderPrefab.transform.localScale.y/2, LeaderPrefab.transform.localScale.y/2,timer/3),
                                                    LeaderPrefab.transform.position.z);
            m_manager.all_CreatureData.Leader = null;
        }
        
    }
}
