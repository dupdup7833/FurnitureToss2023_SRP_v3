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
    private Vector3 startingScaleVec3;

    private int surfacesTouched = 0;

    public bool gamePiecePlaced = false;

    public bool projectileGamePiece = false;

    public HashSet<string> surfacesTouchedSet = new HashSet<string>();

    // Surfaces that don't count for bank shots
    public HashSet<string> noCountedSurfacesTouchedSet = new HashSet<string>();

    public string pieceName;
    // Start is called before the first frame update
    void Start()
    {
        this.startingPositionVec3 = this.transform.position;
        this.startingScaleVec3 = this.transform.localScale;
        noCountedSurfacesTouchedSet.Add("Physics LeftHand");
        noCountedSurfacesTouchedSet.Add("Physics RightHand");

    }

    public void ResetGamePiece()
    {
        this.transform.position = this.startingPositionVec3;
        this.transform.localScale = this.startingScaleVec3;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        HVRGrabbable grabbable = GetComponent<HVRGrabbable>();
        grabbable.enabled = true;
        this.gamePiecePlaced = false;

    }
    public void ResetPosition()
    {
        this.transform.position = this.startingPositionVec3;
    }
    public void Released(HVRGrabberBase hvrbase, HVRGrabbable grabbable)
    {
        lastTouchedTime = Time.time;
        //        Debug.Log("Time Touched Object: " + lastTouchedTime);
        lastItemTouched = "";
        surfacesTouched = 0;
        surfacesTouchedSet.Clear();
    }

    public void PlacePiece()
    {
        // destroy the components so it can't be picked up again
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        HVRGrabbable grabbable = GetComponent<HVRGrabbable>();
        grabbable.enabled = false;
    }
    private void OnCollisionEnter(Collision other)
    {
        //  Debug.Log("Last Item Touched: " + other.gameObject.name);
        lastItemTouched = other.gameObject.name;
        if (!lastItemTouched.Contains("Floor") && !(noCountedSurfacesTouchedSet.Contains(lastItemTouched)))
        {
            surfacesTouched += 1;
            surfacesTouchedSet.Add(lastItemTouched);
            //  Debug.Log("ADDED: " + lastItemTouched);
        }
        // TODO keep a set of surfaces touched.  Insure that a surface is only counted once.  Exclude certain surfaces like the floor.

    }
}
