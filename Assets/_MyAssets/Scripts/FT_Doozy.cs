using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FT_Doozy : MonoBehaviour
{
 
    public Transform playerTransform; // Reference to the player's transform
    public float detectionRange = 35f; // The range within which the player is considered close
    public float minEvadeDistance = 5f; // Minimum distance for evading the player
    public float maxEvadeDistance = 10f; // Maximum distance for evading the player
    public float wanderRadius = 10f; // Radius for wandering behavior
    public float wanderInterval = .05f; // Interval for changing wander destination
    public float checkInterval = .05f; // Interval for checking player proximity

     private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private Animator animator;
    private Vector3 wanderTarget;

    void Start()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Start checking player proximity and wander behavior
        InvokeRepeating("CheckPlayerProximity", 0f, checkInterval);
        InvokeRepeating("SetRandomWanderTarget", 0f, wanderInterval);
    }

    // Function to check if the player is close to the character
    void CheckPlayerProximity()
    {
        // Check if the player is close to the character
        if (IsPlayerClose())
        {
            // Calculate a random direction away from the player
            Vector3 randomDirection = Random.insideUnitSphere * Random.Range(minEvadeDistance, maxEvadeDistance);
            randomDirection += transform.position; // Add character's position to the direction to get a point in world space

            // Set the destination for NavMeshAgent to evade the player
            if (!navMeshAgent.pathPending )
            {
                navMeshAgent.SetDestination(randomDirection);
            }

            // Set animation speed for evading
            navMeshAgent.speed = 4.5f;
            animator.SetFloat("Speed", navMeshAgent.speed);

        }
        else
        {
            // Set destination for NavMeshAgent to wander target
            if (!navMeshAgent.pathPending )
            {
                navMeshAgent.SetDestination(wanderTarget);
            }

            // Set animation speed for walking
             navMeshAgent.speed = 1.5f;
            animator.SetFloat("Speed", 1f);
        }
    }

    // Function to check if the player is close to the character
    bool IsPlayerClose()
    {
        // Calculate the distance between the character and the player
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        // Check if the distance is within the detection range
        return distance <= detectionRange;
    }

    // Function to set a random wander target within the wander radius
    void SetRandomWanderTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;
        UnityEngine.AI.NavMeshHit hit;
        UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1);
        wanderTarget = hit.position;
    }
}
