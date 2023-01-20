using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FT_Drone : MonoBehaviour
{
    Transform player;
    UnityEngine.AI.NavMeshAgent agent;

    public float DelayBetweenRoutes = 1.0f;
    public FT_GamePiece gamePiece;
    // Start is called before the first frame update
    void Start()
    {

        player = FT_GameController.playerTransform;

         Rigidbody rb = gamePiece.GetComponent<Rigidbody>();
        rb.isKinematic = true;

        agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        StartCoroutine(UpdateWander());
    }

    // Update is called once per frame
    void Update()
    {

    }


    IEnumerator UpdateWander()
    {

        while (true)
        {
            yield return new WaitForSeconds(DelayBetweenRoutes);



            Evade(player);
        }



    }


    void Evade(Transform target)
    {
        // Debug.Log("Evade");
        Vector3 targetDir = target.position - this.transform.position;
        float lookAhead = targetDir.magnitude * 3;
       // agent.speed = 3.0f;
        //anim.speed = 3.0f;
        Flee(target.position + target.forward * lookAhead);
    }

    void Flee(Vector3 location)
    {
        Vector3 fleeVector = location - this.transform.position;
        agent.SetDestination(this.transform.position - fleeVector);
    }
}
