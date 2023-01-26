using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using HurricaneVR.Framework.Core.Utils;
public class FT_GameStage : MonoBehaviour
{
    public GameObject[] gamePieces;
    public GameObject[] dropZones;

    public GameObject[] drones;

    public float stageMinimumHeight = -5.0f;

    public TextMeshPro scoreResult;

    public bool stageInProgress = false;
    public float startTime;
    public float timerVal;

    public int piecesPlaced = 0;

    public string stageName;

    public AudioClip AudioStageComplete;

    public FT_LeaderboardUI_ESL StylePointsLeaderboard;
    public FT_LeaderboardUI_ESL TimeLeaderboard;

    public List<GameObject> projectileGamePieces = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // disable all the game pieces
        findGamePiecesAndDropZones();
        Debug.Log("gamePieces: " + gamePieces.Length + "  dropZones: " + dropZones.Length);
        SetupGamePieces(false);
        SetupDropZones(false);
        SetupDrones(false);
        RefreshLeaderboards();


    }

    private void findGamePiecesAndDropZones()
    {
        gamePieces = GameObject.FindGameObjectsWithTag("FT_GamePiece");
        dropZones = GameObject.FindGameObjectsWithTag("FT_DropZone");
        drones = GameObject.FindGameObjectsWithTag("FT_Drone");
    }


    /** Loop through the game pieces that were setup via configuration and put them back to where they started.
        Then remove the game pieces that were shot from the gun (projectile game pieces).  They are spawned in game.
    */
    private void SetupGamePieces(bool status)
    {
        // gamePieces = GameObject.FindGameObjectsWithTag("FT_GamePiece");
        for (int i = 0; i < gamePieces.Length; i++)
        {
            gamePieces[i].SetActive(status);
            gamePieces[i].GetComponent<FT_GamePiece>().ResetGamePiece();
        }

        foreach (var item in projectileGamePieces)
        {
            Destroy(item);

        }

    }


    private void SetupDrones(bool status)
    {
        for (int i = 0; i < drones.Length; i++)
        {
            drones[i].SetActive(status);
            if (status)
            {
                drones[i].GetComponent<FT_Drone>().ResetDrone();
            }


        }
    }
    private void SetupDropZones(bool showDropZones)
    {
        // dropZones = GameObject.FindGameObjectsWithTag("FT_DropZone");
        for (int i = 0; i < dropZones.Length; i++)
        {
            dropZones[i].SetActive(showDropZones);
            if (showDropZones)
            {
                dropZones[i].GetComponent<FT_DropZone>().ResetDropZone();
            }

            //Debug.Log("disabling:" + stagePieces[i].name);
        }
    }
    // Update is called once per frame


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
        SetupDrones(true);
        startTime = Time.time;
        StartCoroutine(UpdateTimer());
        FT_GameController.GC.currentStage = this;
        FT_GameController.GC.stylePointsTotal = 0;
        Debug.Log("current stage " + FT_GameController.GC.currentStage + " " + this);
        SteamLeaderboards.Init();

    }

    public void EndStage()
    {
        Debug.Log("EndStage");
        stageInProgress = false;
        if (AudioStageComplete)
        {
            if (SFXPlayer.Instance) SFXPlayer.Instance.PlaySFX(AudioStageComplete, FT_GameController.playerTransform.position);
        }

        //SteamLeaderboards.UpdateScore(FT_GameController.GC.stylePointsTotal);
        UploadScoresToSteamLeaderboard();

    }


    // need to spread calls to steam out otherwise they will be denied.  Using a coroutine and wait for seconds to spread out
    // call to get the leaderboards and to upload to them.
    private void UploadScoresToSteamLeaderboard()
    {
        Debug.Log("TimerVal" + timerVal);
        StylePointsLeaderboard.UploadScoreToLeaderboard(FT_GameController.GC.stylePointsTotal);
        StartCoroutine(UploadAfterSeconds(2));

    }

    IEnumerator UploadAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        TimeLeaderboard.UploadScoreToLeaderboard((int)timerVal);
    }
    private void RefreshLeaderboards()
    {
        StylePointsLeaderboard.FetchLeaderboard();
        StartCoroutine(RefreshSecondLeaderboard(3));

    }

    IEnumerator RefreshSecondLeaderboard(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        TimeLeaderboard.FetchLeaderboard();

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
            }
            else
            {
                piecesPlaced++;
            }

        }
        Debug.Log(piecesPlaced + " out of " + dropZones.Length + " placed.");
        if (anyLeftToPlace)
        {
            return;
        }
        else
        {
            EndStage();
            Debug.Log("ALL OBJECTS PLACED!  Time Elapsed " + GetFormattedTime());
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        //  Debug.Log("on trigger enter" + other.gameObject.name);
        if (other.CompareTag("Player") && !stageInProgress)
        {
            Debug.Log("on trigger enter PLAYER");
            // StartStage();
        }
    }
}
