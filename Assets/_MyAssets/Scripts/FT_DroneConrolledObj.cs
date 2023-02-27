using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FT_DroneConrolledObj : FT_GenericControlledObj
{

    public float gravityEffect = 20f;
    public float upwardsSpeedAdjustment = 100f;

    public float triggerThreshold = 0.1f;

    public float forcePerCarriedObject = 11f;
    public float downwardCompensationForcePerObject = 11f;
    public float ejectForce = 2.0f;

     

    public override void Move(Vector3 movement, float speed)
    {

        base.Move(movement, speed);

        if (speed < triggerThreshold && movement.x == 0 && movement.y == 0)
        {
            Vector3 downwardForce = Vector3.down * gravityEffect * Time.fixedDeltaTime;
            //trigger is not pressed and they are not moving in any direction
            if (rigidbodiesInZone.Count > 0)
                    {
                      downwardForce = downwardForce + (rigidbodiesInZone.Count * (forcePerCarriedObject/downwardCompensationForcePerObject) *Vector3.down);
                     }
            Debug.Log("DownwardForce: "+downwardForce+ " y:"+downwardForce.y);

            
             
            rb.AddForce(downwardForce);
        }
        else
        {
            Vector3 upwardForce = Vector3.up * speed * Time.fixedDeltaTime * upwardsSpeedAdjustment;

            // account for extra payload
                   if (rigidbodiesInZone.Count > 0)
                 {
                   upwardForce = upwardForce * rigidbodiesInZone.Count * forcePerCarriedObject;
             }
            rb.AddForce(upwardForce);
        }
    }




    public override void ReleasePlayerFromMountPosition()
    {
        foreach (Rigidbody rb in rigidbodiesInZone.Values)
        {
            rb.gameObject.transform.SetParent(rb.gameObject.GetComponent<FT_GamePiece>().originalParent);
            rb.GetComponent<Rigidbody>().isKinematic = false;
            rb.AddForce(Vector3.forward * ejectForce);

        }
        base.ReleasePlayerFromMountPosition();

    }

    // protected override void RemoveFromRigidbodiesInZone(GameObject other)
    // {
    //     rigidbodiesInZone.Remove(other.GetInstanceID());
    //     other.GetComponent<Rigidbody>().isKinematic = false;
    //     Debug.Log("rigidbodiesInZone" + rigidbodiesInZone.Count);
    // }


    // protected override void AddToRigidbodiesInZone(GameObject other)
    // {
    //     rigidbodiesInZone.Add(other.gameObject.GetInstanceID(), other.GetComponent<Rigidbody>());
    //     other.GetComponent<Rigidbody>().isKinematic = true;
    //     Debug.Log("rigidbodiesInZone" + rigidbodiesInZone.Count);
    // }
}

