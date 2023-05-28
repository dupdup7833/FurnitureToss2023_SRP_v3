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

    private bool gameStageInLevel = true;


    // Start is called before the first frame update
    void Start()
    {

        
        //textItems = GetComponentsInChildren<TextMeshPro>();
        //textmeshPro.text = "Example of text to be displayed.";

        // gameStage = GameObject.FindWithTag("FT_GameStage").GetComponent<FT_GameStage>();
       // Debug.Log("Game object name of the scoreboard:" +this.gameObject.name);
        gameStage = FT_GameController.GC.currentStage;
        FT_GameController.gamePiecePlacedEvent.AddListener(UpdateScorboard);
        if (gameStage is null)
        {
            Debug.Log
            ("Could not find the game object");

            gameStage = GameObject.FindWithTag("FT_GameStage")?.GetComponent<FT_GameStage>();
        }
        if (gameStage != null)
        {
            Debug.Log("My Stage is: " + gameStage.stageName+" about to add listener");
            
        }
        else
        {
            gameStageInLevel = false;
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    { 
        

        if (gameStageInLevel && gameStage.stageInProgress)
        {
            timerText.text = "Elapsed Time: " + gameStage.GetFormattedTime();
        }
    }

    protected virtual void UpdateScorboard(string message)
    {
       
        
        Debug.Log(this.gameObject.name+": Update the scorboard"+gameStageInLevel+"gameStageInLevel"+gameStageInLevel);
        // commented out the gameStageInLevel for the HUD.  Not sure if I need to bring this back later.
       // if (gameStageInLevel)
       // {
            Debug.Log("I set the message to "+message);

            
            Debug.Log("this is what it thinks it should be: "+informationText.text);
            StartCoroutine(ShowInformationText(FT_GameController.GC.playerOptions.hudDuration, message));
            ShowStylePointsText();
       // }
         

    }

    IEnumerator ShowInformationText(float displayDuration, string message)
    {
         // commented out the gameStageInLevel for the HUD.  Not sure if I need to bring this back later.
       
      //  if (gameStageInLevel)
       // {
             Debug.Log("In ShowInformationText"+message);
            informationText.text = message;
            yield return new WaitForSeconds(displayDuration);
            informationText.text = "";
       // }

    }

    protected virtual void ShowStylePointsText()
    {
        if (gameStageInLevel)
        {
            stylePointsTotalText.text = "Style Points Total: " + FT_GameController.GC.stylePointsTotal;
        }
    }
}
