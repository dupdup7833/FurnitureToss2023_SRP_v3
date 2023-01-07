using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using HurricaneVR.Framework.Core.Utils;
public class FT_GameStage : MonoBehaviour
{
    public  GameObject[] gamePieces;
     public GameObject[] dropZones;

     public TextMeshPro scoreResult;

    public bool stageInProgress = false;
    public float startTime;
    public float timerVal;

    public int piecesPlaced = 0;

    public string stageName;

     public AudioClip AudioStageComplete;

    // Start is called before the first frame update
    void Start()
    {
        // disable all the game pieces
        findGamePiecesAndDropZones(); 
        Debug.Log("gamePieces: "+gamePieces.Length+"  dropZones: "+dropZones.Length);
        SetupGamePieces(false);
        SetupDropZones(false);
        

    }

    private void findGamePiecesAndDropZones() {
        gamePieces = GameObject.FindGameObjectsWithTag("FT_GamePiece");
      /*  foreach (GameObject go in gamePiecesInScene ) {
            Debug.Log("Added game piece");
            gamePieces.Add((FT_GamePiece)go);
        }
        */
        dropZones =  GameObject.FindGameObjectsWithTag("FT_DropZone");

       /* foreach (GameObject go in dropZonesInScene ) {
             Debug.Log("Added drop zone");
            dropZones.Add(go.GetComponent<FT_DropZone>());
        }
        */
        
        
       
    }

    private void SetupGamePieces(bool status)
    {
        for (int i = 0; i < gamePieces.Length; i++)
        {
            gamePieces[i].SetActive(status);
            //Debug.Log("disabling:" + stagePieces[i].name);
        }
    }


    private void SetupDropZones(bool status)
    {
        for (int i = 0; i < dropZones.Length; i++)
        {
            dropZones[i].SetActive(status);
            //Debug.Log("disabling:" + stagePieces[i].name);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator UpdateTimer()
    {

        while (stageInProgress)
        {
            timerVal = Time.time - startTime;
          //  Debug.Log("Current Time: " + FormatTime(timerVal));
            yield return new WaitForSeconds(0.1f);
        }

    }

    public string GetFormattedTime()
    {
        return ((int)timerVal / 60).ToString("00") + ":" +
       (timerVal % 60).ToString("00.0");
    }

    public void StartStage()
    {
        stageInProgress = true;
        SetupGamePieces(true);
        SetupDropZones(true);
        startTime = Time.time;
        StartCoroutine(UpdateTimer());
        FT_GameController.GC.currentStage = this;

    }

    public void EndStage() {
        Debug.Log("EndStage");
        stageInProgress = false;
         if (AudioStageComplete) {
                if(SFXPlayer.Instance) SFXPlayer.Instance.PlaySFX(AudioStageComplete, FT_GameController.playerTransform.position);
         }
    }

    public void CheckIfComplete()
    {
        piecesPlaced = 0;
        bool anyLeftToPlace = false;
        //Debug.Log("Dropzones.Lenth: "+dropZones.Length);
        for (int i = 0; i < dropZones.Length; i++)
        {
            if (!dropZones[i].GetComponent<FT_DropZone>().objectPlaced)
            {
//                Debug.Log("Found one not placed");
                anyLeftToPlace = true;
            }  else {
                piecesPlaced++;
            }

        }
        Debug.Log(piecesPlaced+" out of "+dropZones.Length+" placed.");
        if (anyLeftToPlace) {
            return;
        } else {
            EndStage();
            Debug.Log("ALL OBJECTS PLACED!  Time Elapsed "+GetFormattedTime());
        }
        

    }

    private void OnTriggerEnter(Collider other)
    {
        //  Debug.Log("on trigger enter" + other.gameObject.name);
        if (other.CompareTag("Player") && !stageInProgress) {
           Debug.Log("on trigger enter PLAYER");
           StartStage();
        } 
    }
}
