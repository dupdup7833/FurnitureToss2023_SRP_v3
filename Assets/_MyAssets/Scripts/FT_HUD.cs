using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FT_Scoreboard_HUD : FT_Scoreboard
{
    
    public bool showTimer = true;
    // Update is called once per frame
    protected override void Update()
    {
        // HUD Doesn't have the timer
        if (showTimer) {
            base.Update();
        }
    }
}
