using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FT_GenericControlledObj : MonoBehaviour
{
    Animator anim;
    public Transform player;

    public FT_Boat boat;

    public FT_PlayerController ftPlayerController;
    public Transform mountPosition;
    public float rotationSpeed = 500.0f;
    public float speedAdjustment = 1.25f;

    public float distanceToLeaveControlledObj = 1.7f;

    public enum controlledObjectType { Unicorn, Boat };
    Vector3 startPos;
    Quaternion startRot;
    Vector3 playerStartPos;
    Quaternion playerStartRot;
    Transform playerPrevParent;

    public bool resetPositionOnRide = true;
    public bool rotateUpAndDown = true;

    public bool playerInTheControlledObj = false;

    private Transform previousParent;


    private void Awake()
    {


    }


    // Start is called before the first frame update
    void Start()
    {
        startRot = Quaternion.identity;
        startPos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        startPos = transform.position;
        anim = this.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetInteger("animation", 9);
        }
        playerPrevParent = player.transform.parent;


    }





    public void MoveWithControlledObj(bool shouldMoveWithControlledObj)
    {
        if (shouldMoveWithControlledObj)
        {
            // Debug.Log("ftPlayers parent " + ftPlayerController.transform.parent);
            previousParent = ftPlayerController.transform.parent.parent;
            ftPlayerController.transform.parent.SetParent(this.transform, true);
            FT_GameController.GC.currentVehicle = this;
            playerInTheControlledObj = true;
            StartCoroutine(CheckIfPlayerLeavesTheControlledObj());

        }
        else
        {
            ftPlayerController.transform.parent.SetParent(previousParent, true);
            FT_GameController.GC.currentVehicle = null;
            playerInTheControlledObj = false;
            StopCoroutine(CheckIfPlayerLeavesTheControlledObj());
            //  Debug.Log("they left the boat");
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // Debug.Log("boat trigger: Entered the trigger" + other.gameObject.tag);

            MoveWithControlledObj(true);

        }
        else if (other.gameObject.tag == "FT_GamePiece")
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

    // private void OnTriggerStay(Collider other)
    // {
    //     if (other.gameObject.tag == "Player")
    //     {
    //         Debug.Log("boat trigger: stayed in the trigger" + other.gameObject.tag);
    //         //MoveWithControlledObj(true);
    //     }
    // }


    IEnumerator CheckIfPlayerLeavesTheControlledObj()
    {
        while (playerInTheControlledObj)
        {
            // Debug.Log("CheckIfPlayerLeavesTheControlledObj is still running");
            if (Vector3.Distance(this.transform.position, ftPlayerController.transform.position) > distanceToLeaveControlledObj)
            {
                MoveWithControlledObj(false);
            }
            yield return new WaitForSeconds(1);
        }
        // Debug.Log("Boat: distance check "+Vector3.Distance(this.transform.position, ftPlayerController.transform.position));
        // MyCollisions();
    }



    public void ResetPosition(bool flyWithUnicorn)
    {
        Debug.Log("Reset position");
        this.transform.rotation = startRot;
        transform.position = startPos;
        if (flyWithUnicorn)
        {
            player.SetParent(playerPrevParent);
            player.transform.localPosition = Vector3.zero;
            player.transform.localRotation = Quaternion.identity;

            //player.transform.position = playerStartPos;
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

        if (boat.isClearForward && y > -1)
        {
            this.transform.Translate(0, 0, speed * Time.deltaTime);
        }
        else if (boat.isClearBackward && y == -1)
        {
            this.transform.Translate(0, 0, speed * Time.deltaTime * y);
        }


    }
    public void StartFlying(bool flyWithUnicorn)
    {
        if (flyWithUnicorn)
        {
            playerStartRot = Quaternion.identity;
            playerStartPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
            player.SetParent(mountPosition.transform);
            if (resetPositionOnRide)
            {
                player.transform.localPosition = Vector3.zero;
                player.transform.localRotation = Quaternion.identity;
            }
        }
    }
}
