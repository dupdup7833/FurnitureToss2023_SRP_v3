using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FT_Boat : MonoBehaviour
{
    public bool inBoat = false;
    MeshRenderer boatMeshRenderer;
    public bool onTheWater = true;

private void Start() {
    
      boatMeshRenderer = GetComponent<MeshRenderer>();
}
 
    private void OnTriggerEnter(Collider other)
    {
          if (other.gameObject.tag == "Terrain")
         {
            onTheWater = false;
            }    else if  (other.gameObject.tag == "Water") {
            onTheWater=true;
         }
        
        Debug.Log("Boat onTriggerEnter other.tag >" + other.gameObject.tag + "  other.gameObject.name>" + other.gameObject.name);
             
     
    }

private void OnTriggerExit(Collider other) {
      if (other.gameObject.tag == "Terrain")
         {
    onTheWater = true;
         }
    
}
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


   
	//  private void FixedUpdate()
	//     {
	//         RaycastHit hit;
	//         // Bit shift the index of the layer (8) to get a bit mask
	//         int layerMask = 1 << 8;
	
	//         // This would cast rays only against colliders in layer 8.
	//         // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
	//         layerMask = ~layerMask;
	//         Vector3 tilted = Quaternion.Euler(120, 0, 0) * Vector3.forward;
	//         if (Physics.Raycast(transform.position, transform.TransformDirection(tilted), out hit, 3.0f, layerMask))
	//         {
	//             Debug.DrawRay(transform.position+new Vector3(0,2,0), transform.TransformDirection(tilted) * hit.distance, Color.yellow);
	//              Debug.Log("Did Hit >"+hit.transform.gameObject.tag);
	//             if (hit.transform.gameObject.tag == "Terrain")
	//             {
    //                 onTheWater = false;
	//                 //InBoat(true);
	//             }
	//             else
	//             {
	//                onTheWater = true;
    //               //  InBoat(false);
	//             }
	
	//         }
	//         else
	//         {
	//             // Debug.DrawRay(transform.position, transform.TransformDirection(tilted) * 1000, Color.white);
	//             //  Debug.Log("Did not Hit");
	//         }
	//     }
 

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
