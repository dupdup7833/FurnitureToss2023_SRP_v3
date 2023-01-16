using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FT_DropZoneObstacle : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform startPoint;
    public Transform endPoint;
    public GameObject obstacle;

    public float stoppingDistance = 0.25f;

    public float speed = 50f;
    private Vector3 currentDestination;
    void Start()
    {
        obstacle.transform.position = startPoint.position;
        currentDestination = endPoint.position;
        startPoint.gameObject.SetActive(false);
        endPoint.gameObject.SetActive(false);
    }



    private void Update()
    {

        //this.transform.position += new Vector3(1f, 0f, 0f);
        Vector3 direction = currentDestination - obstacle.transform.position;


        obstacle.transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        if (direction.magnitude < stoppingDistance)
        {
            if (currentDestination == endPoint.position)
            {
                currentDestination = startPoint.position;
            }
            else
            {
                currentDestination = endPoint.position;
            }
        }
    }
}
