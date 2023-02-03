using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestNaviMeshAgent : MonoBehaviour
{
    UnityEngine.AI.NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.SetDestination(new Vector3(-0.338f,0.909676313f,-69.8209991f));
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
