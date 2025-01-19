using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FT_HangGliderControlledObj : FT_GenericControlledObj
{
    public float downwardSpeed = -.3f;
    public float forwardSpeed = 1.5f;

    public override void Move(Vector3 movement, float speed)
    {


        if (movement.x > movementThreshold || movement.x < -1 * movementThreshold)
        {
            //    
            this.transform.Rotate(0, movement.x * Time.deltaTime * rotationSpeed, 0);

        }
        //base.Move(movement, speed);
        this.transform.Translate(0, downwardSpeed * Time.deltaTime, forwardSpeed * Time.deltaTime);
        Debug.Log("Movement" + movement + "   speed" + speed);
        base.ControlVignette(forwardSpeed);
        audioSource.volume = 2f;

        // if (speed < triggerThreshold && movement.x == 0 && movement.y == 0)
        // {
        //     Vector3 downwardForce = Vector3.down * gravityEffect * Time.fixedDeltaTime;
        //     //trigger is not pressed and they are not moving in any direction
        //     if (rigidbodiesInZone.Count > 0)
        //     {
        //         downwardForce = downwardForce + (rigidbodiesInZone.Count * (forcePerCarriedObject / downwardCompensationForcePerObject) * Vector3.down);
        //     }
        //     Debug.Log("DownwardForce: " + downwardForce + " y:" + downwardForce.y);



        //     rb.AddForce(downwardForce);
        // }
        // else
        // {
        //     Vector3 upwardForce = Vector3.up * speed * Time.fixedDeltaTime * upwardsSpeedAdjustment;

        //     // account for extra payload
        //     if (rigidbodiesInZone.Count > 0)
        //     {
        //         upwardForce = upwardForce * rigidbodiesInZone.Count * forcePerCarriedObject;
        //     }
        //     rb.AddForce(upwardForce);
        // }
    }


}
