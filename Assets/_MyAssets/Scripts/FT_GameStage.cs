using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using HurricaneVR.Framework.Core.Utils;
//using UnityEditor.ProjectWindowCallback;
using HeathenEngineering.SteamworksIntegration;
public class FT_GameStage : MonoBehaviour
{
    public LeaderboardManager fivePieceTimeLeaderboard;
    public GameObject[] gamePieces;

    public List<FT_GamePiece> revealedGamePieceList = new();
    public GameObject[] dropZones;

    public GameObject[] drones;

    public GameObject[] obstacles;

    public float stageMinimumHeight = -5.0f;

    public TextMeshPro scoreResult;

    public bool stageInProgress = false;
    public float startTime;
    public float timerVal;

    public int piecesPlaced = 0;

    public string stageName;

    public AudioClip AudioStageComplete;

    public enum GameType
    {
        Full,
        Quick5
    }

    GameType gameType = GameType.Quick5;

    //public FT_LeaderboardUI_ESL StylePointsLeaderboard;
    //public FT_LeaderboardUI_ESL TimeLeaderboard;

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
        //StartCoroutine(RefreshLeaderboards(2));



    }

    private void findGamePiecesAndDropZones()
    {
        gamePieces = GameObject.FindGameObjectsWithTag("FT_GamePiece");
        dropZones = GameObject.FindGameObjectsWithTag("FT_DropZone");
        drones = GameObject.FindGameObjectsWithTag("FT_Drone");
        obstacles = GameObject.FindGameObjectsWithTag("FT_Obstacle");
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
            //dropZones[i].SetActive(showDropZones);
           //if (showDropZones)
           // {
                dropZones[i].GetComponent<FT_DropZone>().ResetDropZone();
           // }

            //Debug.Log("disabling:" + stagePieces[i].name);
        }
    }

   public void TurnOffAllSolutions() {
         for (int i = 0; i < dropZones.Length; i++)
        {
            //dropZones[i].SetActive(showDropZones);
           //if (showDropZones)
           // {
                dropZones[i].GetComponent<FT_DropZone>().TurnOffSolution();
           // }

            //Debug.Log("disabling:" + stagePieces[i].name);
        }
   }

    private void ShowAllDropZoneSolutions()
    {
        int howManyToSolve = 0;
        List<int> randomNumberList = new List<int>();
        int randomNumber;

        do
        {
            randomNumber = Random.Range(0, dropZones.Length);
            if (!randomNumberList.Contains(randomNumber))
            {
                if (!dropZones[randomNumber].GetComponent<FT_DropZone>().isSecondaryDropZone)
                {
                    // don't add anything to subset that is a secondary drop zone.
                    randomNumberList.Add(randomNumber);
                }
                Debug.Log("random number " + randomNumber + " list length " + randomNumberList.Count);
            }
        } while (randomNumberList.Count < howManyToSolve);

          Debug.Log("random numbers"+randomNumberList);
        
        for (int i = 0; i < dropZones.Length; i++)
        {
            if (!randomNumberList.Contains(i))
            {
                dropZones[i].GetComponent<FT_DropZone>().ShowDropZoneSolution();
            }
        }
    }

    private void SetupObstacles(bool startObstacles)
    {
        for (int i = 0; i < obstacles.Length; i++)
        {
            obstacles[i].GetComponent<FT_DropZoneObstacle>().SetObstacleStatus(startObstacles);
        }
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
        TurnOffAllSolutions();
        SetupGamePieces(true);
        SetupDropZones(true);
        SetupDrones(true);
        SetupObstacles(true);
        startTime = Time.time;
        StartCoroutine(UpdateTimer());
        FT_GameController.GC.currentStage = this;
        FT_GameController.GC.stylePointsTotal = 0;
        Debug.Log("current stage " + FT_GameController.GC.currentStage + " " + this);
        //SteamLeaderboards.Init();
        ShowAllDropZoneSolutions();

    }

    public void EndStage()
    {
        Debug.Log("EndStage");
        stageInProgress = false;
        SetupObstacles(false);
        if (AudioStageComplete)
        {
            if (SFXPlayer.Instance) SFXPlayer.Instance.PlaySFX(AudioStageComplete, FT_GameController.playerTransform.position);
        }

        //SteamLeaderboards.UpdateScore(FT_GameController.GC.stylePointsTotal);
        UploadScoresToSteamLeaderboard();
        fivePieceTimeLeaderboard.UploadScore((int)timerVal);
        fivePieceTimeLeaderboard.GetTopEntries(10);
        //fivePieceTimeLeaderboard.evtQueryCompleted.Invoke(); 

    }


    // need to spread calls to steam out otherwise they will be denied.  Using a coroutine and wait for seconds to spread out
    // call to get the leaderboards and to upload to them.
    private void UploadScoresToSteamLeaderboard()
    {
        Debug.Log("TimerVal" + timerVal);
        //StylePointsLeaderboard.UploadScoreToLeaderboard(FT_GameController.GC.stylePointsTotal);
        //StartCoroutine(UploadAfterSeconds(1));

    }

    IEnumerator UploadAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        // TimeLeaderboard.UploadScoreToLeaderboard((int)timerVal);
    }
    IEnumerator RefreshLeaderboards(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        //StylePointsLeaderboard.FetchLeaderboard();
        // TimeLeaderboard.FetchLeaderboard();

        StartCoroutine(RefreshSecondLeaderboard(1));

    }

    IEnumerator RefreshSecondLeaderboard(float seconds)
    {

        yield return new WaitForSeconds(seconds);
        //TimeLeaderboard.FetchLeaderboard();

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

                if (gameType == GameType.Quick5 && (
                    dropZones[i].GetComponent<FT_DropZone>().isSecondaryDropZone || dropZones[i].GetComponent<FT_DropZone>().obstacle != null))
                {
                    // don't chekck for secondary or obstacle drop zones for quick games.
                    continue;
                }
                Debug.Log("Found one not placed" + dropZones[i].gameObject.name);
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
