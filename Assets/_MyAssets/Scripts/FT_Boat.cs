using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FT_Boat : MonoBehaviour
{
    public bool inBoat = false;
    public bool onTheWater = true;

    public bool isClearForward = true;
    public bool isClearBackward = true;
    public GameObject front;

    public float frontDistanceCheck = 2.0f;
    public GameObject back;
    MeshRenderer backMeshRenderer;

    private void Start()
    {
        front.GetComponent<MeshRenderer>().enabled = false;
        back.GetComponent<MeshRenderer>().enabled = false;

    }

    private void FixedUpdate()
    {
        CheckWhetherForwardAndBackAreClear();

    }

    private void CheckWhetherForwardAndBackAreClear()
    {
        RaycastHit hit;
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;
        Vector3 tiltedForward = Quaternion.Euler(120, 0, 0) * Vector3.forward;
        if (Physics.Raycast(front.transform.position, transform.TransformDirection(tiltedForward), out hit, frontDistanceCheck, layerMask))
        {
            // Debug.DrawRay(front.transform.position, transform.TransformDirection(tiltedForward) * hit.distance, Color.magenta, 30f, true);
            // Debug.Log("Did Hit >" + hit.transform.gameObject.tag);
            if (hit.transform.gameObject.tag == "Water")
            {
                isClearForward = true;
            }
            else
            {
                isClearForward = false;
            }

        }
        Vector3 tiltedBackward = Quaternion.Euler(210, 0, 0) * Vector3.forward;
        if (Physics.Raycast(back.transform.position, transform.TransformDirection(tiltedBackward), out hit, frontDistanceCheck, layerMask))
        {
            // Debug.DrawRay(back.transform.position, transform.TransformDirection(tiltedBackward) * hit.distance, Color.magenta, 30f, true);
            // Debug.Log("Did Hit >"+hit.transform.gameObject.tag);
            if (hit.transform.gameObject.tag == "Water")
            {
                isClearBackward = true;
            }
            else
            {
                isClearBackward = false;
            }

        }
    }
}
