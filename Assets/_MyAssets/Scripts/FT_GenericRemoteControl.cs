
using UnityEngine;
using System.Collections;
using Valve.VR;
using Valve.VR.InteractionSystem;
using HurricaneVR.Framework.Core.Player;
using HurricaneVR.Framework.Core.Grabbers;
using HurricaneVR.Framework.Core;

using HurricaneVR.Framework.ControllerInput;

public class FT_GenericRemoteControl : MonoBehaviour
{
    /// new

    public FT_GenericControlledObj controlledObject;




    /// existing




    private HVRGrabbable grabbable;

    //public Transform Joystick;

    // public float joyMove = 0.1f;

    //public bool flyWithUnicorn;





    public Renderer jumpHighlight;

    private Vector3 movement;


    private bool jump;
    private float glow;
    // private SteamVR_Input_Sources hand;
    // private Interactable interactable;
    float xMovement;
    float yMovement;



    private void Start()
    {
        grabbable = GetComponent<HVRGrabbable>();
        // StartCoroutine(CheckForControllerUsage(controlledObject.checkHowOftenSeconds));
    }

    private void FixedUpdate()
    {


        /*  Joystick.localPosition = movement * joyMove;

          float rot = transform.eulerAngles.y;

          movement = Quaternion.AngleAxis(rot, Vector3.up) * movement;

          jumpHighlight.sharedMaterial.SetColor("_EmissionColor", Color.white * glow);
          */

        //Debug.Log("Accelerate value:" + throttle);




        if (grabbable.IsHandGrabbed)
        {
            float throttle = 0;
        //    controlledObject.ftPlayerController.overridePlayerMovement = true;
            //   Debug.Log("return Inputs.MovementAxis"+Inputs.MovementAxis);
            var controller = grabbable.HandGrabbers[0].Controller;

       //     Debug.Log("controller.Trigger" + controller.Trigger);
        //    Debug.Log("joystick axis" + controller.JoystickAxis);

            /// hand = interactable.attachedToHand.handType;
           // Debug.Log("controller.GetType"+controller.Vive+" xxx "+controller.WMR);
            Vector2 m =  controller.Vive ? controller.TrackpadAxis: controller.JoystickAxis;
             
            
            xMovement = m.x;
            yMovement = m.y;

           // Debug.Log("X: " + xMovement + " Y:" + yMovement);
            movement = new Vector3(m.x, m.y, 0);
            throttle = controller.Trigger;
            //  jump = jumpAction[hand].stateDown;
            //glow = Mathf.Lerp(glow, jumpAction[hand].state ? 1.5f : 1.0f, Time.deltaTime * 20);
           // Debug.Log("controlled object" + controlledObject);
           // Debug.Log("controlledObject.ftPlayerController" + controlledObject.ftPlayerController);
            controlledObject.Move(movement, throttle);
        }
        else
        {
          
           // controlledObject.ftPlayerController.overridePlayerMovement = false;
            movement = Vector2.zero;
            jump = false;
            glow = 0;
        }

    }


}

