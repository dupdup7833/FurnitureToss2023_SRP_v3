using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class DoozyCharacterController : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 20f;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float checkInterval = 1f; // Time interval in seconds to check for player and pick new destination
    [SerializeField] private float walkSpeed = 1.5f; // Speed for walking
    [SerializeField] private float runSpeed = 3.5f; // Maximum speed for running
    [SerializeField] private GameObject childObject; // Child GameObject with mesh
    [SerializeField] private GameObject prefabToSpawn; // Prefab to spawn upon collision

    private NavMeshAgent agent;
    private Animator animator;
    private int currentWaypointIndex = 0;
    private bool playerDetected = false;
    private bool hasBeenHit = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.updateRotation = false; // Disable NavMeshAgent rotation

        // Find the player based on the tag
        GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");
        if (playerGameObject != null)
        {
            agent.SetDestination(playerGameObject.transform.position);
        }
        else
        {
            Debug.LogError("Player not found. Make sure there is a GameObject with the 'Player' tag in the scene.");
        }

        if (waypoints.Length > 0)
        {
            SetNextDestination();
        }
        StartCoroutine(CheckForPlayerAndUpdateDestination());
    }

    void Update()
    {
        // Update the animation parameters
        float speed = agent.velocity.magnitude;
        animator.SetFloat("Speed", speed);

        // Set walking and running animation states based on speed
        animator.SetBool("isRunning", playerDetected && speed > 0.1f);
        animator.SetBool("isWalking", !playerDetected && speed > 0.1f);
    }

    void SetNextDestination()
    {
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        agent.speed = walkSpeed;
        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    private IEnumerator CheckForPlayerAndUpdateDestination()
    {
        while (true)
        {
            // Find the player based on the tag
            GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");
            if (playerGameObject != null)
            {
                float distanceToPlayer = Vector3.Distance(playerGameObject.transform.position, transform.position);
                playerDetected = distanceToPlayer <= detectionRadius;

                if (playerDetected)
                {
                    agent.speed = runSpeed;
                    Vector3 directionAwayFromPlayer = transform.position - playerGameObject.transform.position;
                    Vector3 newPosition = transform.position + directionAwayFromPlayer.normalized * agent.speed * Time.deltaTime;
                    agent.SetDestination(newPosition);
                }
                else
                {
                    agent.speed = walkSpeed;
                    if (!agent.pathPending && agent.remainingDistance < 0.5f)
                    {
                        SetNextDestination();
                    }
                }
            }
            else
            {
                Debug.LogError("Player not found. Make sure there is a GameObject with the 'Player' tag in the scene.");
            }

            yield return new WaitForSeconds(checkInterval);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasBeenHit && collision.gameObject.CompareTag("FT_GamePiece"))
        {
            HideChildAndSpawnPrefab();
            hasBeenHit = true;
        }
    }

    private void HideChildAndSpawnPrefab()
    {
        if (childObject != null)
        {
            // Hide the child GameObject
            childObject.SetActive(false);

            // Spawn the prefab at the child's position
            if (prefabToSpawn != null)
            {
                Instantiate(prefabToSpawn, childObject.transform.position, childObject.transform.rotation);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
