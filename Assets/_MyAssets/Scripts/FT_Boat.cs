using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FT_Boat : MonoBehaviour
{
    public bool inBoat = false;
   // MeshRenderer boatMeshRenderer;
    public bool onTheWater = true;

    public bool isClearForward = true;
    public bool isClearBackward = true;
    public GameObject front;

    //MeshRenderer frontMeshRenderer;
    public float frontDistanceCheck = 2.0f;
    public GameObject back;
    MeshRenderer backMeshRenderer;

    private void Start()
    {

       // boatMeshRenderer = GetComponent<MeshRenderer>();
        front.GetComponent<MeshRenderer>().enabled=false;
         back.GetComponent<MeshRenderer>().enabled=false;

    }

    //     private void OnTriggerEnter(Collider other)
    //     {
    //           if (other.gameObject.tag == "Terrain")
    //          {
    //             onTheWater = false;
    //             }    else if  (other.gameObject.tag == "Water") {
    //             onTheWater=true;
    //          }

    //         Debug.Log("Boat onTriggerEnter other.tag >" + other.gameObject.tag + "  other.gameObject.name>" + other.gameObject.name);


    //     }

    // private void OnTriggerExit(Collider other) {
    //       if (other.gameObject.tag == "Terrain")
    //          {
    //     onTheWater = true;
    //          }

    // }
    /*
         void OnCollisionEnter(Collision c)
      {
         Debug.Log("Boat OnCollisionEnter other.tag >" + c.gameObject.tag);
          // force is how forcefully we will push the player away from the enemy.
          float force = 5;

          // If the object we $$anonymous$$t is the enemy

              // Calculate Angle Between the collision point and the player
              Vector3 dir = c.contacts[0].point - transform.position;
              // We then get the opposite (-Vector3) and normalize it
              dir = -dir.normalized;
              // And finally we add force in the direction of dir and multiply it by force. 
              // T$$anonymous$$s will push back the player

               Debug.Log("Boat OnCollisionEnter APPLYING FORCE");
             GetComponent<Rigidbody>().AddForce(dir*force);
            //  this.transform.Translate(5,0,5);

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
        Vector3 tiltedForward = Quaternion.Euler(120, 0, 0) * Vector3.forward;
        if (Physics.Raycast(front.transform.position, transform.TransformDirection(tiltedForward), out hit, frontDistanceCheck, layerMask))
        {
            Debug.DrawRay(front.transform.position, transform.TransformDirection(tiltedForward) * hit.distance, Color.magenta, 30f, true);
            
             	             Debug.Log("Did Hit >"+hit.transform.gameObject.tag);
            if (hit.transform.gameObject.tag == "Water")
            {
                //onTheWater = true;
                 isClearForward = true;
                //InBoat(true);
            }
            else
            {
                isClearForward = false;
                //  onTheWater = true;
                //  InBoat(false);
            }

        }
        Vector3 tiltedBackward = Quaternion.Euler(210, 0, 0) * Vector3.forward;
        if (Physics.Raycast(back.transform.position, transform.TransformDirection(tiltedBackward), out hit, frontDistanceCheck, layerMask))
        {
            Debug.DrawRay(back.transform.position, transform.TransformDirection(tiltedBackward) * hit.distance, Color.magenta, 30f, true);
            //	             Debug.Log("Did Hit >"+hit.transform.gameObject.tag);
            if (hit.transform.gameObject.tag == "Water")
            {
                // onTheWater = false;
                isClearBackward = true;
                //InBoat(true);
            }
            else
            {
                isClearBackward = false;
                //  onTheWater = true;
                //  InBoat(false);
            }

        }

    }


    // public void InBoat(bool shouldShowBoat)
    // {
    //     Debug.Log("Teleport: InBoat " + shouldShowBoat);
    //     if (shouldShowBoat)
    //     {
    //         inBoat = true;
    //         boatMeshRenderer.enabled = true;
    //         this.gameObject.SetActive(true);
    //     }
    //     else
    //     {
    //         inBoat = false;
    //         boatMeshRenderer.enabled = false;
    //     }
    // }

}
