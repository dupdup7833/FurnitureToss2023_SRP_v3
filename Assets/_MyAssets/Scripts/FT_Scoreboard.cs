using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FT_Scoreboard : MonoBehaviour
{
    public TextMeshPro timerText;
    public TextMeshPro stylePointsTotalText;
    public TextMeshPro informationText;


    private FT_GameStage gameStage;

   


    // Start is called before the first frame update
    void Start()
    {
        //textItems = GetComponentsInChildren<TextMeshPro>();
        //textmeshPro.text = "Example of text to be displayed.";

        // gameStage = GameObject.FindWithTag("FT_GameStage").GetComponent<FT_GameStage>();
        gameStage = FT_GameController.GC.currentStage;
        if (gameStage is null)
        {
            Debug.Log
            ("Could not find the game object");

            gameStage = GameObject.FindWithTag("FT_GameStage").GetComponent<FT_GameStage>();
        }
        Debug.Log("My Stage is: " + gameStage.stageName);
        FT_GameController.gamePiecePlacedEvent.AddListener(UpdateScorboard);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (gameStage.stageInProgress)
        {
            timerText.text = "Elapsed Time: " + gameStage.GetFormattedTime();
        }
    }

    protected virtual void UpdateScorboard(string message)
    {
        Debug.Log("Update the scorboard");
        StartCoroutine(ShowInformationText(FT_GameController.GC.playerOptions.hudDuration, message));
        ShowStylePointsText();

    }

    IEnumerator ShowInformationText(float displayDuration, string message)
    {
        informationText.text = message;
        yield return new WaitForSeconds(displayDuration);
        informationText.text = "";

    }

    protected virtual void ShowStylePointsText()
    {
        stylePointsTotalText.text = "Style Points Total: " + FT_GameController.GC.stylePointsTotal;
    }
}
