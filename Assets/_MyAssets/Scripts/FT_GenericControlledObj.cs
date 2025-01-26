using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HurricaneVR.Framework.Core.Grabbers;

public class FT_GenericControlledObj : MonoBehaviour
{


    Animator anim;
    //public Transform player;

    protected AudioSource audioSource;

    public Transform mountPosition;

    public FT_PlayerController ftPlayerController;

    public ForwardMovementControlledBy forwardMovementControlledBy = ForwardMovementControlledBy.JoystickOrTrackpad;
    public float rotationSpeed = 500.0f;
    public float speedAdjustment = 2f;

    public float movementThreshold = 0.9f;

    public bool normalizeMovement = false;

    Vector3 startPos;
    Quaternion startRot;

    Transform playerPrevParent;

    /// public bool resetPositionOnRide = true;
    public bool resetPlayerRotationOnDismount = false;
    public bool rotateUpAndDown = false;

    public bool playerMovesWithTheControlledObj = true;

    public string displayName;
    private Transform previousParent;
    private Transform previousRotation;

    [Header("Collision Detection")]
    public GameObject frontChecker;
    public GameObject backChecker;
    public bool isClearForward = true;
    public bool isClearBackward = true;

    public float idleVolume = 0.5f;

    public float frontDistanceCheck = 2.0f;
    public float backDistanceCheck = 2.0f;

    [Tooltip("Use raycasts to look for surface from or back.  A boat would want it on while a flying object would not.")]
    public bool checkForValidSufaceTag = true;
    [Tooltip("Raycasts will look for tag on this object to determine if vehicle can go forward or back onto it.")]
    public string validSurfaceTag = "Water";

    [Tooltip("Amount of time to pause between checking for things like obstacles front and back.")]
    public float checkHowOftenSeconds = 0.1f;

    [Header("Debugging")]
    public bool drawFrontandBackCheckers = true;

    protected Dictionary<int, Rigidbody> rigidbodiesInZone = new Dictionary<int, Rigidbody>();

    protected Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        ftPlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<FT_PlayerController>();
        SaveControlledObjectStartingValues();
        StartAnimation();
        previousParent = ftPlayerController.transform.parent;
        audioSource = this.GetComponent<AudioSource>();
        audioSource.volume = idleVolume;
        rb = this.GetComponent<Rigidbody>();
        rb.centerOfMass += new Vector3(0, -.5f, 0);
    }


    // private void FixedUpdate()
    // {

    //     if (checkForValidSufaceTag)
    //     {
    //         CheckWhetherForwardAndBackAreClear();
    //     }

    // }

    IEnumerator DoChecksOnAnInterval()
    {
        while (true)
        {


            if (checkForValidSufaceTag)
            {
                CheckWhetherForwardAndBackAreClear();
            }
            yield return new WaitForSeconds(checkHowOftenSeconds);
        }

    }

    private void StartAnimation()
    {
        anim = this.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetInteger("animation", 9);
        }
    }

    private void SaveControlledObjectStartingValues()
    {
        startRot = Quaternion.identity;
        startPos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        startPos = transform.position;

        playerPrevParent = this.transform.parent;
    }
/*
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "FT_GamePiece")
        {
             HandleParentingCapturedObjects(other, shouldRelease: false);
        }



    }
*/
/*
    private void OnTriggerExit(Collider other)
    {
         HandleParentingCapturedObjects(other, shouldRelease: true);
         

    }
*/
/*
    protected virtual void RemoveFromRigidbodiesInZone(GameObject other)
    {
        rigidbodiesInZone.Remove(other.GetInstanceID());
        Debug.Log("rigidbodiesInZone" + rigidbodiesInZone.Count);
    }
*/
/*

    protected virtual void AddToRigidbodiesInZone(GameObject other)
    {
        rigidbodiesInZone.Add(other.gameObject.GetInstanceID(), other.GetComponent<Rigidbody>());
        other.GetComponent<Rigidbody>().mass = 0.1f;
        //        Debug.Log("rigidbodiesInZone" + rigidbodiesInZone.Count);
    }
    */
    /*
    private void HandleParentingCapturedObjects(Collider other, bool shouldRelease)
    {
        if (other.gameObject.tag == "FT_GamePiece" && !other.GetComponent<FT_GamePiece>().IsGamePiecePlaced())
        {
            Debug.Log("Captured by Generic Controlled Object, placed? " + other.GetComponent<FT_GamePiece>().IsGamePiecePlaced() + other.gameObject.transform.parent.name);
            if (shouldRelease)
            {
                other.gameObject.transform.SetParent(other.gameObject.GetComponent<FT_GamePiece>().originalParent);
                RemoveFromRigidbodiesInZone(other.gameObject);
            }
            else
            {
                if (!other.GetComponent<FT_GamePiece>().IsGamePiecePlaced())
                {
                    other.gameObject.transform.SetParent(this.transform);
                    AddToRigidbodiesInZone(other.gameObject);
                    other.GetComponent<FT_GamePiece>().lastPossessedByDisplayName = this.displayName;
                }
            }
        }
    }
*/




    public virtual void Move(Vector3 movement, float speed)
    {


        // Debug.Log("moving object");
        float x = 0;
        float y = 0;
        if (normalizeMovement)
        {
            if (movement.x > movementThreshold)
            {
                x = 1;
            }
            else if (movement.x < -movementThreshold)
            {
                x = -1;
            }
            else if (movement.y > movementThreshold)
            {
                y = 1;
            }
            else if (movement.y < -movementThreshold)
            {
                y = -1;
            }
        }
        else
        {
            x = movement.x;
            y = movement.y;
            movementThreshold = 0;



        }



        if (forwardMovementControlledBy == ForwardMovementControlledBy.JoystickOrTrackpad)
        {
            speed = speedAdjustment * y;
        }
        else if (forwardMovementControlledBy == ForwardMovementControlledBy.Trigger)
        {
            speed *= speedAdjustment;
        }


        if (x > movementThreshold || x < -1 * movementThreshold)
        {
            //    
            this.transform.Rotate(0, x * Time.deltaTime * rotationSpeed, 0);
        }
        if (y > movementThreshold || y < -1 * movementThreshold)
        {
            //  Debug.Log("rotation limit" + transform.rotation.x);
            /// this.transform.rotation = Quaternion.Euler(this.transform.rotation.x,0, this.transform.rotation.y);
            //  if (transform.rotation.x > -0.023f && transform.rotation.x <0.027f) { 

            if (rotateUpAndDown)
            {
                this.transform.Rotate(y * Time.deltaTime * rotationSpeed, 0, 0);
            }
            // }

            //  if (transform.rotation.x < -0.023f)
            // {
            //   this.transform.rotation = Quaternion.Euler(-0.023f, this.transform.rotation.y, this.transform.rotation.y);
            //   }
            //
            //  if (transform.rotation.x > 0.027f)
            //  {
            // this.transform.rotation = Quaternion.Euler(0.027f, this.transform.rotation.y, this.transform.rotation.y);
            //  }
        }
        /*else if (movement.y < 0.5f && movement.y < -0.5f)
        {

        }*/
        //Debug.Log("Movement Vector: " + movement.x + " " + movement.y + " " + movement.z);
        // Debug.Log("rotation" + this.transform.rotation.x + "," + this.transform.rotation.y + "," + this.transform.rotation.z);

        //        Debug.Log("about to translate" + speed + " " + y + " movement y " + movement.y);

        if ((!checkForValidSufaceTag || isClearForward) && y > movementThreshold)
        {
            this.transform.Translate(0, 0, speed * Time.deltaTime);

            //   Debug.Log("about to translate FORWARD" + speed * Time.deltaTime);
        }
        else if ((!checkForValidSufaceTag || isClearBackward) && y < -1 * movementThreshold)

        {
            this.transform.Translate(0, 0, speed * Time.deltaTime);

            Debug.Log("about to translate BACKWARD" + speed * Time.deltaTime);
        }
        ControlVignette(speed);
        ControlSound(speed);

    }

    protected void ControlVignette(float speed)
    {
        /*
        if (playerMovesWithTheControlledObj)
        {
            Debug.Log("ftPlayerController.postProcessing.VignetteAmount: "+ftPlayerController.postProcessing.VignetteAmount);
            if (speed > 0)
            {

                ftPlayerController.postProcessing.Vignette = true;

            }
            else
            {
                ftPlayerController.postProcessing.Vignette = false;
            }
        }
        */
    }

    private void ControlSound(float speed)
    {
        audioSource.volume = System.Math.Max(System.Math.Abs(speed), idleVolume);
    }

    /* public void StartFlying(bool flyWithUnicorn)
     {
         if (flyWithUnicorn)
         {
             startRot = Quaternion.identity;
             startPos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
             // TODO NEED TO FIX TO GET FLYING WORKING this.transform.SetParent(mountPosition.transform);
             if (resetPositionOnRide)
             {
                 this.transform.localPosition = Vector3.zero;
                 this.transform.localRotation = Quaternion.identity;
             }
         }
     }*/


    private void CheckWhetherForwardAndBackAreClear()
    {
        RaycastHit hit;
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        Vector3 tiltedForward = Quaternion.Euler(50, 0, 0) * Vector3.forward;
        if (drawFrontandBackCheckers)
        {
            Debug.DrawRay(frontChecker.transform.position, transform.TransformDirection(tiltedForward), Color.magenta, 1f, true);
        }
        if (Physics.Raycast(frontChecker.transform.position, transform.TransformDirection(tiltedForward), out hit, frontDistanceCheck, layerMask))
        {

                      Debug.Log("Did Hit >" + hit.transform.gameObject.tag+"   "+hit.transform.gameObject.name);
            if (hit.transform.gameObject.tag == validSurfaceTag)
            {
                isClearForward = true;
            }
            else
            {
                isClearForward = false;
            }

        }
        Vector3 tiltedBackward = Quaternion.Euler(120, 0, 0) * Vector3.forward;
        if (drawFrontandBackCheckers)
        {
            Debug.DrawRay(backChecker.transform.position, transform.TransformDirection(tiltedBackward), Color.magenta, 1f, true);
        }
        if (Physics.Raycast(backChecker.transform.position, transform.TransformDirection(tiltedBackward), out hit, backDistanceCheck, layerMask))
        {

            // Debug.Log("Did Hit >"+hit.transform.gameObject.tag);
            if (hit.transform.gameObject.tag == validSurfaceTag)
            {
                isClearBackward = true;
            }
            else
            {
                isClearBackward = false;
            }

        }
    }

    public void SnapPlayerToMountPosition()
    {
        ftPlayerController.overridePlayerMovement = true;
        StartCoroutine(DoChecksOnAnInterval());
        if (playerMovesWithTheControlledObj)
        {
            ftPlayerController.CharacterController.enabled = false;
            // previousRotation = ftPlayerController.transform;
            ftPlayerController.transform.SetParent(this.transform, true);
            FT_GameController.GC.currentVehicle = this;
            //  playerInTheControlledObj = true;


            ftPlayerController.transform.position = mountPosition.position;
            ftPlayerController.transform.rotation = mountPosition.rotation;
        }


    }

    public virtual void ReleasePlayerFromMountPosition()
    {
        ftPlayerController.overridePlayerMovement = false;

        StopCoroutine(DoChecksOnAnInterval());
        if (playerMovesWithTheControlledObj)
        {
            if (resetPlayerRotationOnDismount)
            {
                ftPlayerController.transform.rotation = Quaternion.identity;
            }
            ftPlayerController.CharacterController.enabled = true;
            Debug.Log("they left the " + this.gameObject.name);
            ftPlayerController.transform.SetParent(previousParent, true);
            //  ftPlayerController.transform.rotation = Quaternion.identity;
            FT_GameController.GC.currentVehicle = null;
            // playerInTheControlledObj = false;

            Debug.Log("they left the " + this.gameObject.name + " and everything should have been cleaned up");
        }
        audioSource.volume = idleVolume;
        ControlVignette(0);
        HVRSocket socket = this.GetComponentInChildren<HVRSocket>();
        socket.ForceRelease();


    }

}

public enum ForwardMovementControlledBy
{
    Trigger,
    JoystickOrTrackpad
}

