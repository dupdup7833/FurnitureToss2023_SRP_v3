using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;

[System.Serializable]
public class GamePiecePlacedStringEvent : UnityEvent<string>
{
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

    

    public static GamePiecePlacedStringEvent gamePiecePlacedEvent = new GamePiecePlacedStringEvent();

    void Start()
    {


    }
    void Awake()
    {
        if (GC != null)
            GameObject.Destroy(GC);
        else
            GC = this;

        DontDestroyOnLoad(this);


    }

    void Update()
    {
        // TEMPORARY FOR GAME TESTING
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("Started level " + currentStageNumber);
            //  GC.startStage(currentStageNumber);
        }
        // END TEMPORARY FOR GAME TESTING
    }





    public static void GamePiecePlaced(string message)
    {
        gamePiecePlacedEvent.Invoke(message);
    }


    public void UnloadPreviousScene()
    {
         Debug.Log("UnLoading Scene "+currentSceneName);
        if (!string.IsNullOrEmpty(currentSceneName))
        {
            SceneManager.UnloadSceneAsync(currentSceneName);
        }
        currentSceneName = "";
    }
    public void LoadScene(string sceneName)
    {
        Debug.Log("Loading Scene "+sceneName);
        UnloadPreviousScene();

        currentSceneName = sceneName;
        //
       // SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
       StartCoroutine(LoadYourAsyncScene(sceneName));
    }


    IEnumerator LoadYourAsyncScene(string sceneName)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName,  LoadSceneMode.Additive);
       //asyncLoad.allowSceneActivation = false;

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}