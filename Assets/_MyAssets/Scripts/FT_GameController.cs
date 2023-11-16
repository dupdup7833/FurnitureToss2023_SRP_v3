using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

[System.Serializable]
public class GamePiecePlacedStringEvent : UnityEvent<string>
{
}

[System.Serializable]
public struct SpaceDoorEntry
{
    public string levelName;
    public FT_SpaceDoor spaceDoor;
}

public class FT_GameController : MonoBehaviour
{
    public static FT_GameController GC;

    public float lastPlacement = 0.0f;

    public int stylePointsTotal = 0;

    public int currentStageNumber = 0;

    public string currentSceneName = "";

    public FT_GameStage currentStage;

    public static Transform playerTransform;

    public FT_PlayerOptions playerOptions = new FT_PlayerOptions();

    public FT_GenericControlledObj currentVehicle;

    public List<SpaceDoorEntry> levelDoors = new List<SpaceDoorEntry>();


    public static string[] comfortSettingNames = { "Off", "Low", "Medium", "High" };
    public static float[] vignetteAmtSettings = { 0f, .4f, .65f, .75f };

    FT_PlayerController player;

    public static GamePiecePlacedStringEvent gamePiecePlacedEvent = new GamePiecePlacedStringEvent();

    void Start()
    {
        SetPlayerOptions();
        // player = GameObject.FindGameObjectWithTag("Player").GetComponent<FT_PlayerController>();
        LoadPlayerOptions();


    }
    void Awake()
    {
        if (GC != null)
            GameObject.Destroy(GC);
        else
            GC = this;

        DontDestroyOnLoad(this);


    }

    public void LoadPlayerOptions()
    {
        playerOptions.hudTimer = PlayerPrefs.GetInt("hudTimer") == 1;
        playerOptions.hudInfoText = PlayerPrefs.GetInt("hudInfoText") == 1;
        playerOptions.hudStylePointsTotal = PlayerPrefs.GetInt("hudStylePointsTotal") == 1;
        playerOptions.hudStylePointsTotalAlwaysOn = PlayerPrefs.GetInt("hudStylePointsTotalAlwaysOn") == 1;
        playerOptions.hudDuration = PlayerPrefs.GetFloat("hudDuration");
        playerOptions.comfortSetting = PlayerPrefs.GetInt("comfortSetting");
        //  Debug.Log("comfort setting:"+playerOptions.comfortSetting+player);
        //  player.postProcessing.VignetteAmount = vignetteAmtSettings[playerOptions.comfortSetting];

        Debug.Log("PlayerOptions.hudTimer:" + playerOptions.hudTimer);
        Debug.Log("PlayerOptions.hudInfoText:" + playerOptions.hudInfoText);
        Debug.Log("PlayerOptions.hudStylePointsTotal:" + playerOptions.hudStylePointsTotal);
        Debug.Log("PlayerOptions.hudStylePointsTotalAlwaysOn:" + playerOptions.hudStylePointsTotalAlwaysOn);
        Debug.Log("PlayerOptions.hudDuration:" + playerOptions.hudDuration);

    }

    private void SetPlayerOptions()
    {
        PlayerPrefs.SetInt("hudTimer", 0);
        PlayerPrefs.SetInt("hudInfoText", 1);
        PlayerPrefs.SetInt("hudStylePointsTotal", 0);
        PlayerPrefs.SetInt("hudStylePointsTotalAlwaysOn", 0);
        PlayerPrefs.SetFloat("hudDuration", 5.0f);
        PlayerPrefs.SetInt("comfortSetting", 0); //low
    }






    public static void GamePiecePlaced(string message)
    {
        gamePiecePlacedEvent.Invoke(message);
    }


    public void UnloadPreviousScene()
    {
        Debug.Log("UnLoading Scene " + currentSceneName);
        if (!string.IsNullOrEmpty(currentSceneName))
        {
            SceneManager.UnloadSceneAsync(currentSceneName);
        }
        currentSceneName = "";
    }
    public void LoadScene(string sceneName)
    {
        if (AllowNewSceneToLoad())
        {
            Debug.Log("Loading Scene " + sceneName);
            UnloadPreviousScene();

            currentSceneName = sceneName;

            //
            // SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            StartCoroutine(LoadYourAsyncScene(sceneName));
        }

    }

    private bool AllowNewSceneToLoad()
    {
        if (GC.currentStage != null && GC.currentStage.stageInProgress)
        {
            Debug.Log("There is a scene currently loaded you need to cancel before loading another scene");
            return false;
        }
        else
        {
            return true;
        }
    }

    private void OpenSpaceDoorForLevel(string sceneName)
    {
        // Close all doors that are not the one for the current level
        // Open the one that is for the level
        foreach (SpaceDoorEntry spaceDoorEntry in levelDoors)
        {
            if (spaceDoorEntry.levelName == sceneName)
            {
                spaceDoorEntry.spaceDoor.OpenDoor();
            }
            else
            {
                spaceDoorEntry.spaceDoor.CloseDoor();
            }

        }

    }

    IEnumerator LoadYourAsyncScene(string sceneName)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        //asyncLoad.allowSceneActivation = false;

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        // wait for the level to fully load and then open the space door
        OpenSpaceDoorForLevel(sceneName);
    }




}