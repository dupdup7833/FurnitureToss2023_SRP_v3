// A longer example of Vector3.Lerp usage.
// Drop this script under an object in your scene, and specify 2 other objects in the "startMarker"/"endMarker" variables in the script inspector window.
// At play time, the script will move the object along a path between the position of those two markers.

using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Core.Grabbers;
using TMPro;

public class FT_DropZone : MonoBehaviour
{
    public UnityEvent Snapped = new UnityEvent();
    public int comboBonus = 100;
    public int doubleDropBonus = 50;
    public int bankShotBonus = 50;
    public float distanceMultiplierBonus = 25.0f;

    public TextMeshPro scoreResult;

    // Transforms to act as start and end markers for the journey.
    public GameObject dropZone;
    public GameObject guideGamePiece;
    private Mesh guideGamePieceMesh;

    // Movement speed in units per second.
    public float speed = 1.0F;

    AudioSource snapToZoneSound;


    // Total distance between the markers
    private float journeyLength;

    public bool objectPlaced = false;




    void Start()
    {
        // set up guide game piece
        guideGamePiece.SetActive(false);
        guideGamePieceMesh = guideGamePiece.GetComponent<MeshFilter>().sharedMesh;

        snapToZoneSound = GetComponent<AudioSource>();

    }



    private void releaseIt(HVRGrabberBase basestuff, HVRGrabbable grabble)
    {
        // if (!objectPlaced && other.tag == "FT_GamePiece" && guideGamePieceMesh == other.GetComponent<MeshFilter>().sharedMesh)
        // {

        StartCoroutine(SnapToZone(grabble.gameObject));
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

                //Debug.Log("is being held " + grabbable.IsBeingHeld);
                // it is not held a entering the zone, so snap to drop zone
                if (!grabbable.IsBeingHeld)
                {
                    // wasn't being held and is the right game piece so snap to the drop zone
                    Destroy(grabbable);
                    StartCoroutine(SnapToZone(other.gameObject));

                }
                else
                {
                    // listen for the piece to be dropped
                    grabbable.Released.AddListener(releaseIt);

                    // show the game piece guide
                    guideGamePiece.SetActive(true);
                }
            }
            else
            {

                Debug.Log(other.gameObject.name);
            }
        }

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
    }

    IEnumerator SnapToZone(GameObject otherGameObject)
    {
        Debug.Log("Snap to Zone");

        
        snapToZoneSound.Play(0);
        objectPlaced = true;

        // destroy the components so it can't be picked up again
        Rigidbody rb = otherGameObject.GetComponent<Rigidbody>();
        Destroy(rb);
        HVRGrabbable grabbable = otherGameObject.GetComponent<HVRGrabbable>();
        Destroy(grabbable);

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
        guideGamePiece.SetActive(false);
        dropZone.SetActive(false);

        string scoreMessage = CalculateScore(otherGameObject);
        FT_GameController.GamePiecePlaced(scoreMessage);
        FT_GameController.GC.currentStage.CheckIfComplete();

        ShowScore(scoreMessage);

        // invoke the snapped event so that listening scoring tiles can turn on
        Snapped.Invoke();
    }

    private void ShowScore(string scoreMessageIn)
    {
        Destroy(scoreResult,5f);
        scoreResult.transform.LookAt(FT_GameController.playerTransform);
        scoreResult.SetText(scoreMessageIn);
        scoreResult.gameObject.SetActive(true);
        Quaternion q = scoreResult.transform.rotation;
        scoreResult.transform.rotation = Quaternion.Euler(q.eulerAngles.x, q.eulerAngles.y + 180, q.eulerAngles.z);
    }

    private string CalculateScore(GameObject otherGameObject)
    {
        int currentStylePoints = 0;
        FT_GamePiece ftGamePiece = otherGameObject.GetComponent<FT_GamePiece>();
        string scoreMessageToReturn = "";

        // DISTANCE BONUS
        float distance = Vector3.Distance(FT_GameController.playerTransform.position, this.transform.position);
        Debug.Log("Distance of throw: " + distance + " Player Position: " + FT_GameController.playerTransform.position);
        if (distance > 3)
        {
            currentStylePoints += (int)(distanceMultiplierBonus * distance);
            scoreMessageToReturn += "Distance Bonus: " + currentStylePoints + "\n";
        }
        else
        {
            // scoreMessageToReturn += "Too short for distance bonus\n";
        }


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
        Debug.Log("Distance Thrown:" + distance);


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

        // BANK SHOT
        if (ftGamePiece.surfacesTouchedSet.Count>0)
        {
            int surfacesTouched = ftGamePiece.surfacesTouchedSet.Count;
            if (surfacesTouched == 1)
            {
                scoreMessageToReturn += "\nSingle Bank Shot Bonus! +" + bankShotBonus;
                currentStylePoints += bankShotBonus;
            }
            else
            {
                scoreMessageToReturn += "\n" + surfacesTouched + " Surface Bank Shot Bonus! +" + (surfacesTouched * bankShotBonus)*2;
                currentStylePoints += (surfacesTouched * bankShotBonus)*2;
                Debug.Log("Surfaces touched: "+ftGamePiece.surfacesTouchedSet);
                
            }

        }
        // scoreMessageToReturn += "good stuff!!!";

        FT_GameController.GC.lastPlacement = Time.time;
        FT_GameController.GC.stylePointsTotal += currentStylePoints;

        return scoreMessageToReturn;
    }
}