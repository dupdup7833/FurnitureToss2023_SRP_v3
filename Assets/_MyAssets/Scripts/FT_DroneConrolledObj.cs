using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FT_DroneConrolledObj : FT_GenericControlledObj
{

    public float gravityEffect = 1.5f;
    public float upwardsSpeedAdjustment = 2f;

    public float triggerThreshold = 0.1f;


    public override void Move(Vector3 movement, float speed)
    {

        base.Move(movement, speed);

        if (speed < triggerThreshold  && movement.x == 0 && movement.y == 0)
        {
            // trigger is not pressed and they are not moving in any direction
            
            rb.AddForce(Vector3.down * gravityEffect * Time.deltaTime);
        }
        else 
        {
            rb.AddForce(Vector3.up * speed * Time.deltaTime * upwardsSpeedAdjustment);
        }
    }

}
 
