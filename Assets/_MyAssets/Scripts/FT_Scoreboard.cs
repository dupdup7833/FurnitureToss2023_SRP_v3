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

    public float informationDisplayDuration = 5.0f;


    // Start is called before the first frame update
    void Start()
    {
        //textItems = GetComponentsInChildren<TextMeshPro>();
        //textmeshPro.text = "Example of text to be displayed.";

        gameStage = GameObject.FindWithTag("FT_GameStage").GetComponent<FT_GameStage>();
        if (gameStage is null){
             Debug.LogError("Could not find the game object");
        }
        Debug.Log("My Stage is: "+gameStage.stageName);
        FT_GameController.gamePiecePlacedEvent.AddListener(UpdateScorboard);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStage.stageInProgress)
        {
            timerText.text = "Elapsed Time: " + gameStage.GetFormattedTime();
        }
    }

    public void UpdateScorboard(string message)
    {
        Debug.Log("Update the scorboard");
        StartCoroutine(ShowInformationText(informationDisplayDuration, message));
        stylePointsTotalText.text = "Style Points Total: "+ FT_GameController.GC.stylePointsTotal;
    }

    IEnumerator ShowInformationText(float displayDuration, string message)
    {
        informationText.text = message;
        yield return new WaitForSeconds(displayDuration);
        informationText.text = "";

    }
}
