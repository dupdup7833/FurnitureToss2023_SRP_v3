using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FT_DropZoneObstacle : MonoBehaviour
{
    // Start is called before the first frame update

    //public Transform startPoint;
    public Transform startPoint;
    private Vector3 startPointSaved;
    public Transform endPoint;


    public GameObject obstacle;

    public float stoppingDistance = 0.01f;

    public float speed = 3f;
    private Vector3 currentDestination;

    private bool isActive = false;
    void Start()
    {
        // save the starting point of the obstacle
        startPointSaved = obstacle.transform.position;

        // make the obstacle start with going towards the end point
        currentDestination = endPoint.position;

        // turn off guides in game
        startPoint.GetComponent<MeshRenderer>().enabled = false;
        startPoint.gameObject.SetActive(false);
        endPoint.gameObject.SetActive(false);
    }

    public void SetObstacleStatus(bool active)
    {
        Debug.Log("SetObstacleStatus: "+active);
        isActive = active;
        startPoint.gameObject.SetActive(active);
    }


    private void Update()
    {
        if (isActive)
        {
            // get the direction to current destination
            Vector3 direction = currentDestination - startPoint.transform.position;

            // move towards the target using speed and direction
            startPoint.transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

            // if close enough to the end target, within stopping distance, flip the direction
            if (direction.magnitude < stoppingDistance)
            {
                if (currentDestination == endPoint.position)
                {
                    currentDestination = startPointSaved;
                }
                else
                {
                    currentDestination = endPoint.position;
                }
            }
        }
    }
}
