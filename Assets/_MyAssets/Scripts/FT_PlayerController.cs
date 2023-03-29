using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HurricaneVR.Framework.Core.Player;

public class FT_PlayerController : HVRPlayerController
{
    // Start is called before the first frame update

    [Header("Furniture Toss")]
    public bool overridePlayerMovement = false;

    public MobilePostProcessing postProcessing;
    //private bool inBoat = true;
    // public FT_Boat boat;

    //  [Range(0.01f, 0.03f)]
    //  public float boatSpeed = 0.02f;

    Vector3 lastGoodPosition;
//     void OnControllerColliderHit(ControllerColliderHit hit)
//     {
//         if (hit.gameObject.tag == "Water")
//         {
//             //CharacterController.Move(hit.normal);//new Vector3(hit.normal.x,.25f,hit.normal.z)
             
//             CharacterController.enabled = false;
// CharacterController.transform.position =  lastGoodPosition+new Vector3(hit.normal.x,hit.normal.y*.50f,hit.normal.z);
// CharacterController.enabled = true;
//         } else {
//             lastGoodPosition = transform.position;
//         }
        
//         Debug.Log("OnControllerColliderHit " + hit.gameObject.tag+" "+hit.normal);

//     }

    protected override void HandleRotation()
    {
       
        if (overridePlayerMovement)
        {

        }
        else
        {
            base.HandleRotation();
        }
    }

    protected override void HandleHorizontalMovement()
    {
        if (overridePlayerMovement)
        {

        }
        else
        {
            base.HandleHorizontalMovement();
        }

    }


    /*
        //  protected virtual void CheckTriggerPull()
        //     {

        //         if (!Grabbable.IsHandGrabbed)
        //             return;

        //         var controller = Grabbable.HandGrabbers[0].Controller;

        //         if (controller.Trigger <= TriggerResetThreshold)
        //         {
        //             IsTriggerReset = true;
        //         }

        //         if (controller.Trigger > TriggerPullThreshold && IsTriggerReset)
        //         {
        //             TriggerPulled();
        //             IsTriggerReset = false;
        //             IsTriggerPulled = true;
        //         }
        //         else if (controller.Trigger < TriggerPullThreshold && IsTriggerPulled)
        //         {
        //             IsTriggerPulled = false;
        //             TriggerReleased();
        //         }
        //     }


        protected override void Update()
        {
            //        Debug.Log("Trigger: "+ LeftHand.Controller.Trigger);
            if (boat.inBoat)
            {
                if (RightHand.Controller.Trigger > 0.6f)
                {
                    GetMovementDirection(out var forward, out var right);

                    // Vector3 move = new Vector3(1, 0, 0);
                    CharacterController.Move(
                        boat.transform.forward * RightHand.Controller.Trigger * boatSpeed);
                }
            }
        }
        */
}
