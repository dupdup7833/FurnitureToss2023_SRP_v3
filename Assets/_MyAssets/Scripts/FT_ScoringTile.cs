using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FT_ScoringTile : MonoBehaviour
{
    public void DropZoneFilled(){
        Debug.Log("Drop Zone Filled Called");
        Light light = GetComponentInChildren<Light>();
        light.color = Color.yellow;

    }
}
