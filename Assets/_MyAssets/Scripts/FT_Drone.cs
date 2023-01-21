using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FT_Drone : MonoBehaviour
{
    Transform player;
    UnityEngine.AI.NavMeshAgent agent;
    int startingHealthPoints = 3;
    int healthPoints;

    public float DelayBetweenRoutes = 1.0f;
    public FT_GamePiece gamePiece;

    Vector3 startingPosition;
    Quaternion startingRotation;

    private bool carryingTheFurniturePiece = true;

    public GameObject gamePieceRider;
    // Start is called before the first frame update
    void Start()
    {

        player = FT_GameController.playerTransform;
        startingPosition = this.transform.position;
        startingRotation = this.transform.rotation;

        healthPoints = startingHealthPoints;

        //  Rigidbody rb = gamePiece.GetComponent<Rigidbody>();
        // rb.isKinematic = true;

        agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        StartCoroutine(UpdateWander());
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("just got hit by" + other.gameObject.name + " with tag " + other.gameObject.tag);
        if (other.gameObject.tag == "FT_GamePiece")
        {
            RegisterHitOnDrone();
        }
    }


    private void RegisterHitOnDrone()
    {
        healthPoints -= 1;
        Debug.Log("health points: " + healthPoints);
        if (healthPoints <= 0)
        {
            Debug.Log("You killed the drone");
            KillTheDrone();
        }
    }

    private void KillTheDrone()
    {
        if (carryingTheFurniturePiece)
        {
            carryingTheFurniturePiece = false;
            gamePieceRider.GetComponent<MeshRenderer>().enabled = false;
            this.agent.enabled = false;
            DropTheFurniturePiece();
        }

    }

    private void DropTheFurniturePiece()
    {
        FT_GamePiece instantiatedGamePiece = Instantiate(gamePiece, gamePieceRider.transform.position, Quaternion.identity);//
        instantiatedGamePiece.gameObject.SetActive(true);
        instantiatedGamePiece.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        FT_GameController.GC.currentStage.projectileGamePieces.Add(instantiatedGamePiece.gameObject);
    }

    public void ResetDrone()
    {
        Debug.Log("Reseting the drone");
        Debug.Log("Drone starting position is "+startingPosition);
        Rigidbody rb = this.GetComponent<Rigidbody>();
       // rb.velocity = new Vector3(0,0,0);
        rb.isKinematic = true;
        rb.position = startingPosition;
        rb.rotation = startingRotation;
        agent.Warp(startingPosition);
        rb.isKinematic = false;

        healthPoints = startingHealthPoints;
        gamePieceRider.GetComponent<MeshRenderer>().enabled = true;
        this.agent.enabled = true;
        carryingTheFurniturePiece = true;
        StartCoroutine(UpdateWander());
        
    }
    IEnumerator UpdateWander()
    {

        while (this.agent.enabled)
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
        if (agent.enabled)
        {
            agent.SetDestination(this.transform.position - fleeVector);
        }
    }
}
