using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Core.Grabbers;





public class FT_GamePiece : MonoBehaviour
{
    public float lastTouchedTime = 0.0f;
    private string lastItemTouched = "";

    private int surfacesTouched = 0;

    public HashSet<string> surfacesTouchedSet = new HashSet<string>();

    // Surfaces that don't count for bank shots
    public HashSet<string> noCountedSurfacesTouchedSet = new HashSet<string>();

    public string pieceName;
    // Start is called before the first frame update
    void Start()
    {
        noCountedSurfacesTouchedSet.Add("Physics LeftHand");
        noCountedSurfacesTouchedSet.Add("Physics RightHand");
    }



    public void Released(HVRGrabberBase hvrbase, HVRGrabbable grabbable)
    {
        lastTouchedTime = Time.time;
        Debug.Log("Time Touched Object: " + lastTouchedTime);
        lastItemTouched = "";
        surfacesTouched = 0;
        surfacesTouchedSet.Clear();
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Last Item Touched: " + other.gameObject.name);
        lastItemTouched = other.gameObject.name;
        if (!lastItemTouched.Contains("Floor") && !(noCountedSurfacesTouchedSet.Contains(lastItemTouched)))
        {
            surfacesTouched += 1;
            surfacesTouchedSet.Add(lastItemTouched);
            Debug.Log("ADDED: "+lastItemTouched);
        }
        // TODO keep a set of surfaces touched.  Insure that a surface is only counted once.  Exclude certain surfaces like the floor.

    }
}
