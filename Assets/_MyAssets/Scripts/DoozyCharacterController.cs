using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class DoozyCharacterController : MonoBehaviour
{
    public Transform[] waypoints;  // Array to hold the waypoints
    public float playerDetectionRange = 5f;  // Range within which the player will be detected
    public float avoidanceDistance = 3f;  // Distance at which the character avoids the player
    public float stoppingDistance = 0.1f;  // Distance at which the character stops at a waypoint
    public float maxSpeed = 3.5f; // Maximum speed of the character
    public float walkingSpeed = 1.5f; // Walking speed of the character
    public float speedIncreaseDistance = 10f; // Distance from the player at which speed starts increasing
    public float speedIncreaseFactor = 2f; // Factor by which speed increases
    public GameObject childComponentPrefab; // Prefab of the child component to drop

    private NavMeshAgent agent;
    private Transform player;  // Reference to the player GameObject
    private Animator animator; // Reference to the character's animator
    private bool playerDetected = false; // Flag to track if the player is detected

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;  // Assuming player tag is set to "Player"
        animator = GetComponent<Animator>();

        // Start coroutine to check for player every 1 second
        StartCoroutine(CheckForPlayerRoutine());

        // Set initial destination
        SetRandomDestination();

        // Debug log initial position
        Debug.Log("gpt Initial position: " + transform.position);

        // Debug log whether initial position intersects with water
        if (IntersectsWater(transform.position))
        {
            Debug.Log("gpt Initial position intersects with water.");
        }
    }

    void Update()
    {
        // Adjust speed based on distance from player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer < playerDetectionRange)
        {
            playerDetected = true;
        }
        else
        {
            playerDetected = false;
        }

        if (playerDetected)
        {
            // Increase speed
            agent.speed = Mathf.Lerp(agent.speed, maxSpeed * speedIncreaseFactor, Time.deltaTime);
        }
        else
        {
            // Reset speed to walking speed
            agent.speed = Mathf.Lerp(agent.speed, walkingSpeed, Time.deltaTime);
        }

        // Check if we have reached the current waypoint
        if (!agent.pathPending && agent.remainingDistance < stoppingDistance)
        {
            // Set a new random destination
            SetRandomDestination();
        }

        // Calculate normalized speed for animation
        float normalizedSpeed = agent.velocity.magnitude / maxSpeed;

        // Update animator parameter
        animator.SetFloat("Speed", normalizedSpeed);
    }

    void SetRandomDestination()
    {
        // Pick a random waypoint from the array
        int randomIndex = Random.Range(0, waypoints.Length);
        Debug.Log("gpt random index "+randomIndex+" waypoints.Length "+waypoints.Length);
        Vector3 randomPosition = waypoints[randomIndex].position;

        // Set the destination to the random waypoint
        agent.destination = randomPosition;

        // Debug log new destination
        Debug.Log("gpt New destination: " + randomPosition);

        // Check if the new destination intersects with an object tagged as "Water"
        if (IntersectsWater(agent.destination))
        {
            // If the destination intersects with water, pick a new destination
            SetRandomDestination();
        }
    }

    bool IntersectsWater(Vector3 position)
    {
        // Check if there is any collider tagged as "Water" at the given position
        Collider[] colliders = Physics.OverlapSphere(position, 1f);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Water"))
            {
                return true;
            }
        }
        return false;
    }

    IEnumerator CheckForPlayerRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // Wait for 1 second

            // Check if player is within detection range
            if (Vector3.Distance(transform.position, player.position) < playerDetectionRange)
            {
                // If player is within range, avoid the player
                AvoidPlayer();
            }
        }
    }

    void AvoidPlayer()
    {
        // Find the farthest waypoint from the player
        Transform farthestWaypoint = null;
        float maxDistance = float.MinValue;
        foreach (Transform waypoint in waypoints)
        {
            float distanceToPlayer = Vector3.Distance(waypoint.position, player.position);
            if (distanceToPlayer > maxDistance)
            {
                maxDistance = distanceToPlayer;
                farthestWaypoint = waypoint;
            }
        }

        if (farthestWaypoint != null)
        {
            // Set the destination to the farthest waypoint from the player
            agent.destination = farthestWaypoint.position;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object has the tag "FT_GamePiece"
        if (collision.gameObject.CompareTag("FT_GamePiece"))
        {
            // Drop the child component
            DropChildComponent();
        }
    }

    void DropChildComponent()
    {
        // Instantiate the child component prefab at the current position
        GameObject childComponent = Instantiate(childComponentPrefab, transform.position, Quaternion.identity, transform);

        // Find child objects on the character controller
        foreach (Transform child in transform)
        {
            // Find the MeshRenderer component on the child object
            MeshRenderer childMeshRenderer = child.GetComponent<MeshRenderer>();
            if (childMeshRenderer != null)
            {
                // Disable the MeshRenderer
                childMeshRenderer.enabled = false;
            }
        }
    }
}
