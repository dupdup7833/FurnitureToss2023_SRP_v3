using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HurricaneVR.Framework.Core.Player;

public class FT_PlayerController : HVRPlayerController
{
    // Start is called before the first frame update

    [Header("Furniture Toss")]
    private bool inBoat = true;
    public GameObject boat;


    protected override void HandleHorizontalMovement()
    {
        if (!inBoat)
        {
            //      Debug.Log("not in boat");
            base.HandleHorizontalMovement();

        }
        else
        {
            base.HandleHorizontalMovement();
            //        Debug.Log("in boat");
        }
    }

    public void InBoat(bool shouldShowBoat)
    {
        Debug.Log("Teleport: InBoat " +shouldShowBoat);
        if (shouldShowBoat)
        {
            inBoat = true;
            boat.SetActive(true);
        }
        else
        {
            inBoat = false;
            boat.SetActive(false);
        }
    }
}
