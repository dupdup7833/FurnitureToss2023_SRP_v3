using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FT_GenericControlledObj : MonoBehaviour
{
    Animator anim;
    //public Transform player;

    AudioSource audioSource;

    public Transform mountPosition;

    public FT_PlayerController ftPlayerController;

    public float rotationSpeed = 500.0f;
    public float speedAdjustment = 2f;

    Vector3 startPos;
    Quaternion startRot;

    Transform playerPrevParent;

    /// public bool resetPositionOnRide = true;
    public bool resetPlayerRotationOnDismount = false;
    public bool rotateUpAndDown = false;

    public bool playerMovesWithTheControlledObj = true;

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
    public string validSurfaceTag = "Water";

    [Header("Debugging")]
    public bool drawFrontandBackCheckers = true;

    // Start is called before the first frame update
    void Start()
    {
        ftPlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<FT_PlayerController>();
        SaveControlledObjectStartingValues();
        StartAnimation();
        previousParent = ftPlayerController.transform.parent;
        audioSource = this.GetComponent<AudioSource>();
        audioSource.volume = idleVolume;
    }


    private void FixedUpdate()
    {
        CheckWhetherForwardAndBackAreClear();
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "FT_GamePiece")
        {
            HandleParentingCapturedObjects(other, shouldRelease: false);
        }



    }

    private void OnTriggerExit(Collider other)
    {
        HandleParentingCapturedObjects(other, shouldRelease: true);

    }

    private void HandleParentingCapturedObjects(Collider other, bool shouldRelease)
    {
        if (other.gameObject.tag == "FT_GamePiece")
        {
            if (shouldRelease)
            {
                other.gameObject.transform.SetParent(other.gameObject.GetComponent<FT_GamePiece>().originalParent);
            }
            else
            {
                other.gameObject.transform.SetParent(this.transform);
            }
        }
    }





    public void Move(Vector3 movement, float speed)
    {

        // Debug.Log("moving object");
        int x = 0;
        int y = 0;
        if (movement.x > 0.9f)
        {
            x = 1;
        }
        else if (movement.x < -0.9f)
        {
            x = -1;
        }
        else if (movement.y > 0.9f)
        {
            y = 1;
        }
        else if (movement.y < -0.9f)
        {
            y = -1;
        }

        speed *= speedAdjustment;
        if (x == 1 || x == -1)
        {
            //    
            this.transform.Rotate(0, x * Time.deltaTime * rotationSpeed, 0);
        }
        else if (y == 1 || y == -1)
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

        //Debug.Log("about to translate" + speed * Time.deltaTime);

        if (isClearForward && y > -1)
        {
            this.transform.Translate(0, 0, speed * Time.deltaTime);
            
         //   Debug.Log("about to translate FORWARD" + speed * Time.deltaTime);
        }
        else if (isClearBackward && y == -1)

        {
            this.transform.Translate(0, 0, speed * Time.deltaTime * y);
             
          //  Debug.Log("about to translate BACKWARD" + speed * Time.deltaTime * y);
        }  

        audioSource.volume = System.Math.Max(speed,idleVolume);

        


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

            //            Debug.Log("Did Hit >" + hit.transform.gameObject.tag);
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
        ftPlayerController.CharacterController.enabled = false;
        if (playerMovesWithTheControlledObj)
        {
            // previousRotation = ftPlayerController.transform;
            ftPlayerController.transform.SetParent(this.transform, true);
            //  FT_GameController.GC.currentVehicle = this;
            //  playerInTheControlledObj = true;


            ftPlayerController.transform.position = mountPosition.position;
            ftPlayerController.transform.rotation = mountPosition.rotation;
        }
    }

    public void ReleasePlayerFromMountPosition()
    {
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
            // FT_GameController.GC.currentVehicle = null;
            // playerInTheControlledObj = false;

            Debug.Log("they left the " + this.gameObject.name + " and everything should have been cleaned up");
        }
        audioSource.volume = idleVolume;

    }

}
