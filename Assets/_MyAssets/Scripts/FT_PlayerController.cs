using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HurricaneVR.Framework.Core.Player;

public class FT_PlayerController : HVRPlayerController
{
    // Start is called before the first frame update

    [Header("Furniture Toss")]
    private bool inBoat = true;
    public GameObject boat;

    [Range(0.01f, 0.03f)]
    public float boatSpeed = 0.02f;



    protected override void HandleHorizontalMovement()
    {
        if (!inBoat)
        {
            //      Debug.Log("not in boat");
            base.HandleHorizontalMovement();
         //   base.xzVelocity*=10f;


        }
        else
        {
            base.HandleHorizontalMovement();
            //        Debug.Log("in boat");
        }
    }

    public void InBoat(bool shouldShowBoat)
    {
        Debug.Log("Teleport: InBoat " +shouldShowBoat);
        if (shouldShowBoat)
        {
            inBoat = true;
            boat.SetActive(true);
        }
        else
        {
            inBoat = false;
           // boat.SetActive(false);
        }
    }

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

 
    protected override void Update() {
        Debug.Log("Trigger: "+ LeftHand.Controller.Trigger);
        if (RightHand.Controller.Trigger>0.2f){
             GetMovementDirection(out var forward, out var right);
             
        // Vector3 move = new Vector3(1, 0, 0);
        CharacterController.Move(forward*RightHand.Controller.Trigger*boatSpeed);
        }
    }
}
