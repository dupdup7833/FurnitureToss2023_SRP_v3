using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FT_DroneConrolledObj : FT_GenericControlledObj
{

    public float gravityEffect = 20f;
    public float upwardsSpeedAdjustment = 100f;

    public float triggerThreshold = 0.1f;

    public float forcePerCarriedObject = 15f;
    public float ejectForce = 2.0f;

    public override void Move(Vector3 movement, float speed)
    {

        base.Move(movement, speed);

        if (speed < triggerThreshold && movement.x == 0 && movement.y == 0)
        {
            Vector3 downwardForce =Vector3.down * gravityEffect * Time.fixedDeltaTime;
            // trigger is not pressed and they are not moving in any direction
 //if (rigidbodiesInZone.Count > 0)
   //         {
     //           downwardForce = downwardForce + (rigidbodiesInZone.Count * (forcePerCarriedObject/1.2f) *Vector3.up);
  //          }
//
            rb.AddForce(downwardForce);
        }
        else
        {
            Vector3 upwardForce = Vector3.up * speed * Time.fixedDeltaTime * upwardsSpeedAdjustment;

            // account for extra payload
    //        if (rigidbodiesInZone.Count > 0)
      //      {
        //        upwardForce = upwardForce * rigidbodiesInZone.Count * forcePerCarriedObject;
          //  }
            rb.AddForce(upwardForce);
        }
    }




    public override void ReleasePlayerFromMountPosition()
    {
        foreach (Rigidbody rb in rigidbodiesInZone.Values)
        {
            rb.gameObject.transform.SetParent(rb.gameObject.GetComponent<FT_GamePiece>().originalParent);
            rb.mass = 1.0f;
            rb.AddForce(Vector3.forward * ejectForce);

        }
        base.ReleasePlayerFromMountPosition();

    }
}

