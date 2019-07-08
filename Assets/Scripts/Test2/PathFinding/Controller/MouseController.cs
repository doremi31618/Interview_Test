using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public GameObject LeaderPrefab;
    public GameObjectManager m_manager;
    // Start is called before the first frame update
    void Start()
    {
        m_manager = GetComponent<GameObjectManager>();
    }

    private void OnMouseDown() {
        
    }
}
