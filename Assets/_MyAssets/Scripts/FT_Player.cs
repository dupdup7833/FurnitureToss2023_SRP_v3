using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FT_Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Setting Player Transform. Position: "+this.transform.position);
        FT_GameController.playerTransform = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
