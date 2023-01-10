using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Core.Grabbers;





public class FT_GamePiece : MonoBehaviour
{
    public float lastTouchedTime = 0.0f;
    private string lastItemTouched = "";

    private Vector3 startingPositionVec3;

    private int surfacesTouched = 0;

    public HashSet<string> surfacesTouchedSet = new HashSet<string>();

    // Surfaces that don't count for bank shots
    public HashSet<string> noCountedSurfacesTouchedSet = new HashSet<string>();

    public string pieceName;
    // Start is called before the first frame update
    void Start()
    {
        this.startingPositionVec3 = this.transform.position;
        noCountedSurfacesTouchedSet.Add("Physics LeftHand");
        noCountedSurfacesTouchedSet.Add("Physics RightHand");
      
    }
 

    void Update(){
        ResetIfNotInPlayArea();
    } 

    private void ResetIfNotInPlayArea()
    {
        // Debug.Log("in the coroutine begin");
       // while (true)
        //{
          // Debug.Log("in the coroutine");
            if (FT_GameController.GC.currentStage!=null)
            {
                //Debug.Log("checking position " + FT_GameController.GC.currentStage.stageMinimumHeight + "   " + this.transform.position.y);
                if (FT_GameController.GC.currentStage.stageMinimumHeight > this.transform.position.y)
                {
                    Debug.Log("Had to reset position");
                    this.transform.position = this.startingPositionVec3;
                     Debug.Log("new y "+this.transform.position.y);
                }

            }
           // yield return new WaitForSeconds(5.0f);
     //   }  
    }

    public void Released(HVRGrabberBase hvrbase, HVRGrabbable grabbable)
    {
        lastTouchedTime = Time.time;
        //        Debug.Log("Time Touched Object: " + lastTouchedTime);
        lastItemTouched = "";
        surfacesTouched = 0;
        surfacesTouchedSet.Clear();
    }

    private void OnCollisionEnter(Collision other)
    {
        //  Debug.Log("Last Item Touched: " + other.gameObject.name);
        lastItemTouched = other.gameObject.name;
        if (!lastItemTouched.Contains("Floor") && !(noCountedSurfacesTouchedSet.Contains(lastItemTouched)))
        {
            surfacesTouched += 1;
            surfacesTouchedSet.Add(lastItemTouched);
            Debug.Log("ADDED: " + lastItemTouched);
        }
        // TODO keep a set of surfaces touched.  Insure that a surface is only counted once.  Exclude certain surfaces like the floor.

    }
}
