using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HurricaneVR.Framework.Core.Player;

public class FT_Teleporter : HVRTeleporter
{
    protected override bool CheckValidDestination(GameObject hitObject, Vector3 destination, Vector3 surfaceNormal){
        FT_PlayerController ftPlayerController = (FT_PlayerController)Player;
        Debug.Log("Teleport: hit object tag "+hitObject.tag);
        if (hitObject.tag == "Water") {
            ftPlayerController.InBoat(true);
            
        } else {
          ftPlayerController.InBoat(false);
        }
        return base.CheckValidDestination(hitObject,destination,surfaceNormal);
    }
}
