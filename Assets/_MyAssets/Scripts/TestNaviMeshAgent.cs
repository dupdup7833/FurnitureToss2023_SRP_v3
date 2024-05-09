using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestNaviMeshAgent : MonoBehaviour
{
    UnityEngine.AI.NavMeshAgent agent;
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        //agent.SetDestination(new Vector3(-0.338f,0.909676313f,-69.8209991f));
        agent.SetDestination(this.target.position);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
