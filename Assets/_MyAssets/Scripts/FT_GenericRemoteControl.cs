 
using UnityEngine;
using System.Collections;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace Valve.VR.InteractionSystem.Sample
{
    public class FT_GenericRemoteControl : MonoBehaviour
    {
        public Transform Joystick;
        public float joyMove = 0.1f;
        public bool flyWithUnicorn;

        public SteamVR_Action_Vector2 moveAction = SteamVR_Input.GetAction<SteamVR_Action_Vector2>("platformer", "Move");
        public SteamVR_Action_Boolean jumpAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("platformer", "Jump");
        public SteamVR_Action_Single actionThrottle = SteamVR_Input.GetAction<SteamVR_Action_Single>("platformer", "Accelerate");

        public FT_GenericControlledObj character;

        public Renderer jumpHighlight;

        private Vector3 movement;
        private bool jump;
        private float glow;
        private SteamVR_Input_Sources hand;
        private Interactable interactable;
        float xMovement;
        float yMovement;


        private void Start()
        {
            interactable = GetComponent<Interactable>();
        }

        private void Update()
        {
            float throttle = 0;
            if (interactable.attachedToHand)
            {
                hand = interactable.attachedToHand.handType;
                Vector2 m = moveAction[hand].axis;
                xMovement = m.x;
                yMovement = m.y;

               // Debug.Log("X: " + xMovement + " Y:" + yMovement);
                movement = new Vector3(m.x, m.y,0);
                throttle = actionThrottle.GetAxis(hand);
                jump = jumpAction[hand].stateDown;
                glow = Mathf.Lerp(glow, jumpAction[hand].state ? 1.5f : 1.0f, Time.deltaTime * 20);
                character.Move(movement , throttle);
            }
            else
            {
                movement = Vector2.zero;
                jump = false;
                glow = 0;
            }

          /*  Joystick.localPosition = movement * joyMove;

            float rot = transform.eulerAngles.y;

            movement = Quaternion.AngleAxis(rot, Vector3.up) * movement;

            jumpHighlight.sharedMaterial.SetColor("_EmissionColor", Color.white * glow);
            */

            //Debug.Log("Accelerate value:" + throttle);
            
        }
    }
}