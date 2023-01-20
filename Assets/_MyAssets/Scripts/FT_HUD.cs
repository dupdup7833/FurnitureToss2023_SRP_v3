using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FT_HUD : FT_Scoreboard
{

    protected override void Update()
    {
        if (FT_GameController.GC.playerOptions.hudTimer)
        {
            base.Update();
        }
    }

    protected override void UpdateScorboard(string message)
    {
        if (FT_GameController.GC.playerOptions.hudInfoText)
        {
            base.UpdateScorboard(message);
        }
         ShowStylePointsText();
    }

    protected override void ShowStylePointsText()
    {

        if (FT_GameController.GC.playerOptions.hudStylePointsTotal)
        {
            stylePointsTotalText.text = "Style Points Total: " + FT_GameController.GC.stylePointsTotal;
            if (!FT_GameController.GC.playerOptions.hudStylePointsTotalAlwaysOn)
            {
                StartCoroutine(HideStylePointsText());
            }

        }
    }

    IEnumerator HideStylePointsText()
    {
        yield return new WaitForSeconds(FT_GameController.GC.playerOptions.hudDuration);
        stylePointsTotalText.text = "";

    }



}
