// A longer example of Vector3.Lerp usage.
// Drop this script under an object in your scene, and specify 2 other objects in the "startMarker"/"endMarker" variables in the script inspector window.
// At play time, the script will move the object along a path between the position of those two markers.

using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Core.Grabbers;
using TMPro;
using System.Collections.Generic;

public class FT_DropZone : MonoBehaviour
{
    public UnityEvent Snapped = new UnityEvent();
    public float durationOfScoringMessageSeconds = 5.0f;

    [Header("Scoring Bonuses")]
    public int comboBonus = 100;
    public int doubleDropBonus = 50;
    public int bankShotBonus = 50;
    public float distanceMultiplierBonus = 25.0f;

    public float ejectForce = 700f;

    public int forceGrabDrop = 25;

    public int droneDropBonus = 200;
    public int miniATVDropBonus = 100;
    public FT_DropZone secondaryDropZone;

    public FT_DropZoneObstacle obstacle;
    public bool isSecondaryDropZone = false;


    public TextMeshPro scoreResult;

    // Transforms to act as start and end markers for the journey.
    public GameObject dropZone;
    public GameObject guideGamePiece;
    private Mesh guideGamePieceMesh;

    // Movement speed in units per second.


    AudioSource snapToZoneSound;


    // Total distance between the markers
    private float journeyLength;

    public bool objectPlaced = false;

    private Dictionary<int, Rigidbody> rigidbodiesInZone = new Dictionary<int, Rigidbody>();


    void Start()
    {
        // set up guide game piece
        guideGamePiece.SetActive(false);
        guideGamePieceMesh = guideGamePiece.GetComponent<MeshFilter>().sharedMesh;
        //  if (obstacle != null)
        // {
        //     obstacle.SetObstacleStatus(false);
        //  }
        snapToZoneSound = GetComponent<AudioSource>();
        //  if (secondaryDropZone != null)
        //  {
        //       secondaryDropZone.dropZone.SetActive(false);
        //  }

    }



    private void releaseIt(HVRGrabberBase basestuff, HVRGrabbable grabble)
    {
        // if (!objectPlaced && other.tag == "FT_GamePiece" && guideGamePieceMesh == other.GetComponent<MeshFilter>().sharedMesh)
        // {

        PreSnapToZone(grabble.gameObject, forceDrop: false);
        // }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("on trigger enter" + other.gameObject.transform.position);
        if (!objectPlaced && other.tag == "FT_GamePiece" && guideGamePieceMesh == other.GetComponent<MeshFilter>().sharedMesh)
        {
            HVRGrabbable grabbable = other.gameObject.GetComponent<HVRGrabbable>();



            if (grabbable != null)
            {
                if (grabbable.IsBeingForcedGrabbed)
                {
                    // pulled into the zone via a force grab
                    ForceGrabDrop(other, grabbable);
                }
                else if (!grabbable.IsBeingHeld)
                {
                    // it is not held a entering the zone, so snap to drop zone
                    DroppedIntoTheZone(other, grabbable);
                }
                else if (grabbable.IsBeingHeld)
                {
                    // Still being held so show the game piece guide and
                    // listen for the piece to be dropped
                    ShowHoverGamePieceAndListenForDrop(grabbable);
                }
            }
            else
            {

                Debug.Log(other.gameObject.name);
            }
        }
        else if (!objectPlaced && (other.tag == "FT_GamePiece" || other.tag == "FT_Vehicle"))
        {
            // it's a game object but not the one that matches
            AddToRigidbodiesInZone(other.gameObject);
        }

    }

    private void AddToRigidbodiesInZone(GameObject other)
    {
        rigidbodiesInZone.Add(other.gameObject.GetInstanceID(), other.GetComponent<Rigidbody>());
        Debug.Log("rigidbodiesInZone" + rigidbodiesInZone.Count);
    }

    private void RemoveFromRigidbodiesInZone(GameObject other)
    {
        rigidbodiesInZone.Remove(other.GetInstanceID());
        Debug.Log("rigidbodiesInZone" + rigidbodiesInZone.Count);
    }
    private void DroppedIntoTheZone(Collider other, HVRGrabbable grabbable)
    {
        grabbable.enabled = false;
        Debug.Log("this is the one");
        PreSnapToZone(other.gameObject, forceDrop: false);

    }

    private void PreSnapToZone(GameObject otherGameObject, bool forceDrop)
    {
        EjectOtherRigidBodies();
        StartCoroutine(SnapToZone(otherGameObject, forceDrop));
    }

    private void EjectOtherRigidBodies()
    {
        Debug.Log("ejecting");
        foreach (Rigidbody rb in rigidbodiesInZone.Values)
        {
            Debug.Log("ejecting and object: " + rb.gameObject.name);
            rb.AddForce(Vector3.up * ejectForce);
        }
    }
    private void ShowHoverGamePieceAndListenForDrop(HVRGrabbable grabbable)
    {
        grabbable.Released.AddListener(releaseIt);

        // show the game piece guide
        guideGamePiece.SetActive(true);
    }

    private void ForceGrabDrop(Collider other, HVRGrabbable grabbable)
    {
        Debug.Log("IsBeingForcedGrabbed:" + grabbable.IsBeingForcedGrabbed);
        grabbable.enabled = false;
        PreSnapToZone(other.gameObject, forceDrop: true);
    }

    private void OnTriggerExit(Collider other)
    {

        if (!objectPlaced && other.tag == "FT_GamePiece")
        {
            HVRGrabbable grabbable = other.gameObject.GetComponent<HVRGrabbable>();

            if (grabbable != null)
            {
                if (grabbable.IsBeingHeld)
                {

                    grabbable.Released.RemoveListener(releaseIt);
                }
            }
            guideGamePiece.SetActive(false);

        }
        if (other.tag == "FT_GamePiece" || other.tag == "FT_Vehicle")
        {
            RemoveFromRigidbodiesInZone(other.gameObject);
        }
    }

    IEnumerator SnapToZone(GameObject otherGameObject, bool forceDrop)
    {
        Debug.Log("Snap to Zone");



        snapToZoneSound.Play(0);
        objectPlaced = true;

        otherGameObject.GetComponent<FT_GamePiece>().PlacePiece();

        // lerp to the Game Piece Guide scale, position, rotation
        Vector3 startingPos = otherGameObject.transform.position;
        Quaternion startingRot = otherGameObject.transform.rotation;
        Vector3 startingScale = otherGameObject.transform.localScale;
        for (float f = 0.0f; f <= 1.0f; f += 0.02f)
        {
            otherGameObject.transform.position = Vector3.Lerp(startingPos, guideGamePiece.transform.position, f);
            otherGameObject.transform.localScale = Vector3.Lerp(startingScale, guideGamePiece.transform.localScale, f);
            otherGameObject.transform.rotation = Quaternion.Lerp(startingRot, guideGamePiece.transform.rotation, f);

            // Debug.Log("other object moving" + otherGameObject.transform.position);
            // yield return new WaitForSeconds(.01f);
            yield return null;
        }
        // hide everything related to the dropzone
        if (otherGameObject.transform.position != guideGamePiece.transform.position)
        {
            Debug.Log("IT DID NOT GET SNAPPED");
        }
        guideGamePiece.SetActive(false);
        dropZone.SetActive(false);
        if (obstacle != null)
        {
            obstacle.SetObstacleStatus(false);
        }

        FT_GamePiece ftGamePiece = otherGameObject.GetComponent<FT_GamePiece>();
        ftGamePiece.gamePiecePlaced = true;
        Debug.Log("about to check secondary drop zone");
        if (secondaryDropZone != null)
        {
            Debug.Log("Turning on secondary drop zone");
            secondaryDropZone.gameObject.SetActive(true);
            secondaryDropZone.dropZone.SetActive(true);

            // secondaryDropZone.gameObject.SetActive(true);
            // secondaryDropZone.dropZone.SetActive(true);
            Debug.Log("is the secondary drop zone active " + secondaryDropZone.dropZone.activeSelf);

            //  secondaryDropZone.objectPlaced = false;
        }

        string scoreMessage = CalculateScore(otherGameObject, forceDrop: forceDrop);
        FT_GameController.GamePiecePlaced(scoreMessage);
        FT_GameController.GC.currentStage.CheckIfComplete();

        ShowScore(scoreMessage);


        // invoke the snapped event so that listening scoring tiles can turn on
        Snapped.Invoke();

    }

    public virtual void ResetDropZone()
    {
        Debug.Log("Entering ResetDropZone.  gameObject " + gameObject.name + " isSecondaryDropZone " + isSecondaryDropZone);
        if (obstacle != null)
        {
            obstacle.SetObstacleStatus(true);
        }
        if (!isSecondaryDropZone)
        {
            dropZone.SetActive(true);
            this.gameObject.SetActive(true);
        }
        else
        {
            dropZone.SetActive(false);
            this.gameObject.SetActive(false);
        }
        objectPlaced = false;
        rigidbodiesInZone.Clear();


    }
    private void ShowScore(string scoreMessageIn)
    {

        StartCoroutine(HideAfterSeconds(durationOfScoringMessageSeconds, scoreResult.gameObject));
        scoreResult.transform.LookAt(FT_GameController.playerTransform);
        scoreResult.SetText(scoreMessageIn);
        scoreResult.gameObject.SetActive(true);
        Quaternion q = scoreResult.transform.rotation;
        scoreResult.transform.rotation = Quaternion.Euler(q.eulerAngles.x, q.eulerAngles.y + 180, q.eulerAngles.z);
    }

    IEnumerator HideAfterSeconds(float seconds, GameObject obj)
    {
        yield return new WaitForSeconds(seconds);
        obj.SetActive(false);
    }

    private string CalculateScore(GameObject otherGameObject, bool forceDrop)
    {
        int currentStylePoints = 0;
        FT_GamePiece ftGamePiece = otherGameObject.GetComponent<FT_GamePiece>();
        string scoreMessageToReturn = "";


        DistanceBonus(ref currentStylePoints, ref scoreMessageToReturn);

        ComboDrop(ref currentStylePoints, ftGamePiece, ref scoreMessageToReturn);

        DoubleDrop(ref currentStylePoints, ref scoreMessageToReturn);

        BankShot(ref currentStylePoints, ftGamePiece, ref scoreMessageToReturn);

        /// ADDITIVE BONUSES - added to any previous caculation
        ForceDrop(forceDrop, ref currentStylePoints, ref scoreMessageToReturn);

        DroneDropBonus(ref currentStylePoints, ftGamePiece, ref scoreMessageToReturn);

        MiniATVDropBonus(ref currentStylePoints, ftGamePiece, ref scoreMessageToReturn);




        // scoreMessageToReturn += "good stuff!!!";
        Debug.Log("Score Message to Return: " + scoreMessageToReturn);

        FT_GameController.GC.lastPlacement = Time.time;
        FT_GameController.GC.stylePointsTotal += currentStylePoints;

        return scoreMessageToReturn;


    }



    private void DistanceBonus(ref int currentStylePoints, ref string scoreMessageToReturn)
    {
        float distance = Vector3.Distance(FT_GameController.playerTransform.position, this.transform.position);
        Debug.Log("Distance of throw: " + distance + " Player Position: " + FT_GameController.playerTransform.position);
        if (distance > 3)
        {
            currentStylePoints += (int)(distanceMultiplierBonus * distance);
            scoreMessageToReturn += "Distance Bonus +" + currentStylePoints + "\n";
            FT_Steamworks_Integration.LongDistanceThrow100Points();
        }
        else
        {
            // scoreMessageToReturn += "Too short for distance bonus\n";
        }
    }

    private void ComboDrop(ref int currentStylePoints, FT_GamePiece ftGamePiece, ref string scoreMessageToReturn)
    {
        // COMBO BONUS
        if ((Time.time - ftGamePiece.lastTouchedTime) > 3)
        {
            // if it is getting dropped into a zone and hasn't been touched for a while
            // it must have been hit by another game piece, so combo bonus.
            Debug.Log("Time since it was touched:" + (Time.time - ftGamePiece.lastTouchedTime));
            currentStylePoints += comboBonus;
            scoreMessageToReturn += "\nCombo Bonus! +" + comboBonus;
        }
        else
        {
            Debug.Log("Time since it was touched:" + (Time.time - ftGamePiece.lastTouchedTime));
        }
    }

    private void DoubleDrop(ref int currentStylePoints, ref string scoreMessageToReturn)
    {
        // DOUBLE DROP BONUS
        if (FT_GameController.GC.lastPlacement > 0)
        {
            float timeBetween = Time.time - FT_GameController.GC.lastPlacement;
            Debug.Log("Time Difference: " + timeBetween);
            FT_GameController.GC.lastPlacement = Time.time;
            if (timeBetween < 1)
            {
                scoreMessageToReturn += "\nDouble Drop Bonus! +" + doubleDropBonus;
                currentStylePoints += doubleDropBonus;
            }
        }
    }

    private void ForceDrop(bool forceDrop, ref int currentStylePoints, ref string scoreMessageToReturn)
    {
        // Force Drop
        if (forceDrop)
        {
            scoreMessageToReturn += "\nForce Grab Drop +" + forceGrabDrop;
            currentStylePoints += forceGrabDrop;
        }
    }

    private void BankShot(ref int currentStylePoints, FT_GamePiece ftGamePiece, ref string scoreMessageToReturn)
    {
        if (ftGamePiece.surfacesTouchedSet.Count > 0)
        {
            int surfacesTouched = ftGamePiece.surfacesTouchedSet.Count;
            if (surfacesTouched == 1)
            {
                scoreMessageToReturn += "\nSingle Bank Shot Bonus! +" + bankShotBonus;
                currentStylePoints += bankShotBonus;
            }
            else
            {
                scoreMessageToReturn += "\n" + surfacesTouched + " Surface Bank Shot Bonus! +" + (surfacesTouched * bankShotBonus) * 2;
                currentStylePoints += (surfacesTouched * bankShotBonus) * 2;
                // Debug.Log("Surfaces touched: " + ftGamePiece.surfacesTouchedSet);
                foreach (string surface in ftGamePiece.surfacesTouchedSet)
                {
                    Debug.Log("Surfaces touched: " + surface);
                }

            }

        }
    }

    private void MiniATVDropBonus(ref int currentStylePoints, FT_GamePiece ftGamePiece, ref string scoreMessageToReturn)
    {
        if (ftGamePiece.lastPossessedByDisplayName == "Mini ATV")
        {
            scoreMessageToReturn += "Mini ATV Drop Bonus +" + miniATVDropBonus + " \n";
        }


    }

    private void DroneDropBonus(ref int currentStylePoints, FT_GamePiece ftGamePiece, ref string scoreMessageToReturn)
    {
        if (ftGamePiece.lastPossessedByDisplayName == "Drone")
        {
            scoreMessageToReturn += "Drone Drop Bonus +" + droneDropBonus + " \n";
            currentStylePoints += droneDropBonus;

        }


    }
}