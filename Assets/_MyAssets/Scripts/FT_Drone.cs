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

    public float DelayBetweenRoutes = 3.0f;
    public FT_GamePiece gamePiece;

    Vector3 startingPosition;
    Quaternion startingRotation;

    Vector3 topCorner = new Vector3(4.58f, 0.91f, -63.66f);
    Vector3 bottomCorner = new Vector3(-2.6f, 0.91f, -86.80f);




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

    }

    private void OnCollisionEnter(Collision other)
    {
       // Debug.Log("just got hit by" + other.gameObject.name + " with tag " + other.gameObject.tag);
        if (other.gameObject.tag == "FT_GamePiece")
        {
            RegisterHitOnDrone();
        }
    }


    private void RegisterHitOnDrone()
    {
        FindASpotOnTheLevel();
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
        Debug.Log("Drone starting position is " + startingPosition);
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
        Debug.Log("UpdateWander "+Time.time);
        while (this.agent.enabled)
        {
            FindASpotOnTheLevel();
            yield return new WaitForSecondsRealtime(DelayBetweenRoutes);
           
        }
    }



    void FindASpotOnTheLevel()
    {
///        Debug.Log("FindASpotOnTheLevel: "+Time.time);
        Vector3 newDestination = new Vector3(Random.Range(bottomCorner.x, topCorner.x), topCorner.y, Random.Range(bottomCorner.z, topCorner.z));
        agent.SetDestination(newDestination);
    }


    //// NOT USED YET
    void Evade(Transform target)
    {
        Debug.Log("Navmesh: Evade Player Position: " + target.position);
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
            Debug.Log("Navmesh: Evade Drone Target Position: " + (this.transform.position - fleeVector));

        }
    }

    Vector3 wanderTarget = Vector3.zero;
    void Wander()
    {
        float wanderRadius = 10;
        float wanderDistance = 10;
        float wanderJitter = 1;

        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter,
                                        0,
                                        Random.Range(-1.0f, 1.0f) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = this.gameObject.transform.InverseTransformVector(targetLocal);

        Seek(targetWorld);
    }

    void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }
}


