using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FT_Boat : MonoBehaviour
{
    public bool inBoat = false;
    MeshRenderer boatMeshRenderer;

private void Start() {
    
      boatMeshRenderer = GetComponent<MeshRenderer>();
}
/*
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Boat onTriggerEnter other.tag >" + other.gameObject.tag + "  other.gameObject.name>" + other.gameObject.name);
        if (other.gameObject.tag == "Terrain")
        {
            this.gameObject.SetActive(false);

        }
        else if (other.gameObject.tag == "Water")
        {
            this.gameObject.SetActive(true);

        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Boat onCollisionEnter other.tag >" + other.gameObject.tag + "  other.gameObject.name>" + other.gameObject.name);
    }

*/


    private void FixedUpdate()
    {
        RaycastHit hit;
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;
        Vector3 tilted = Quaternion.Euler(120, 0, 0) * Vector3.forward;
        if (Physics.Raycast(transform.position, transform.TransformDirection(tilted), out hit, 3.0f, layerMask))
        {
            Debug.DrawRay(transform.position+new Vector3(0,2,0), transform.TransformDirection(tilted) * hit.distance, Color.yellow);
             Debug.Log("Did Hit >"+hit.transform.gameObject.tag);
            if (hit.transform.gameObject.tag == "Water")
            {
                InBoat(true);
            }
            else
            {
                InBoat(false);
            }

        }
        else
        {
            // Debug.DrawRay(transform.position, transform.TransformDirection(tilted) * 1000, Color.white);
            //  Debug.Log("Did not Hit");
        }
    }

    public void InBoat(bool shouldShowBoat)
    {
        Debug.Log("Teleport: InBoat " + shouldShowBoat);
        if (shouldShowBoat)
        {
            inBoat = true;
           boatMeshRenderer.enabled = true;
            this.gameObject.SetActive(true);
        }
        else
        {
            inBoat = false;
           boatMeshRenderer.enabled = false;
        }
    }

}
