using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HurricaneVR.Framework.Core.Grabbers;

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

        audioSource.volume = idleVolume * 2.0f;

        if (speed < triggerThreshold && movement.x == 0 && movement.y == 0)
        {
            Vector3 downwardForce = Vector3.down * gravityEffect * Time.fixedDeltaTime;
            //trigger is not pressed and they are not moving in any direction
            if (rigidbodiesInZone.Count > 0)
            {
                downwardForce = downwardForce + (rigidbodiesInZone.Count * (forcePerCarriedObject / downwardCompensationForcePerObject) * Vector3.down);
            }
            Debug.Log("DownwardForce: " + downwardForce + " y:" + downwardForce.y);



            rb.AddForce(downwardForce);
        }
        else
        {
            Vector3 upwardForce = Vector3.up * speed * Time.fixedDeltaTime * upwardsSpeedAdjustment;

            // account for extra payload
            if (rigidbodiesInZone.Count > 0)
            {
                Debug.Log("extra objects in the zone "+rigidbodiesInZone.Count);
                upwardForce = upwardForce * rigidbodiesInZone.Count * forcePerCarriedObject;
            }
            rb.AddForce(upwardForce);
        }
    }

private void OnTriggerEnter(Collider other)
    {
        


    }

    private void OnTriggerExit(Collider other)
    {
         
         

    }


    public override void ReleasePlayerFromMountPosition()
    {
        EjectAllCarriedRigidBodies();
        base.ReleasePlayerFromMountPosition();

    }

    private void EjectAllCarriedRigidBodies()
    {
       HVRSocket socket = this.GetComponentInChildren<HVRSocket>();
       socket.ForceRelease();
       /* foreach (Rigidbody rb in rigidbodiesInZone.Values)
        {
            rb.gameObject.transform.SetParent(rb.gameObject.GetComponent<FT_GamePiece>().originalParent);
            rb.GetComponent<Rigidbody>().isKinematic = false;
            rb.AddForce(Vector3.forward * ejectForce);

        }
        */
    }

    private void OnCollisionEnter(Collision other)
    {
//        Debug.Log("Drone Hit: " + other.gameObject.name);
        if (other.gameObject.tag == "Terrain")
        {
            HandleDroneCrash();
        }

    }
    private void HandleDroneCrash()
    {
        ReleasePlayerFromMountPosition();
    }
}

