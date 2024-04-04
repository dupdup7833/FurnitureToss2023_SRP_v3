using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Core.Grabbers;





public class FT_GamePiece : MonoBehaviour
{

    public float lastTouchedTime = 0.0f;
    private GameObject lastItemTouched;

    private Vector3 startingPositionVec3;
    private Vector3 startingScaleVec3;

    public Transform originalParent;

    private int surfacesTouched = 0;

    private bool gamePiecePlaced = false;

    public bool projectileGamePiece = false;

    public HashSet<string> surfacesTouchedSet = new HashSet<string>();

    public string lastPossessedByDisplayName;

    // Surfaces that don't count for bank shots
    public HashSet<string> objectNamesToNotCountForSurfacesTouchedSet = new HashSet<string> { "Physics LeftHand", "Physics RightHand", "FloorCollision", "FT_Painting1_GamePiece(Clone)" };

    public string pieceName;

    private AudioSource bounceSound;

    private Rigidbody rb;
  
    // Start is called before the first frame update
    public void Start()
    {
        this.startingPositionVec3 = this.transform.localPosition;
        this.startingScaleVec3 = this.transform.localScale;
        this.originalParent = this.transform.parent;
        this.bounceSound = this.GetComponent<AudioSource>();
        this.rb = GetComponent<Rigidbody>();


    }

    public void Grabbed()
    {
        lastPossessedByDisplayName = "Player";
    }
    public void ResetGamePiece()
    {
        this.transform.localPosition = this.startingPositionVec3;
        this.transform.localScale = this.startingScaleVec3;
        
        if (rb!=null) {rb.isKinematic = false;}

        HVRGrabbable grabbable = GetComponent<HVRGrabbable>();
        grabbable.enabled = true;
        this.PlacePiece(false);
        AddGlassSphere();
   

    }
    public void ResetPosition()
    {
        this.transform.localPosition = this.startingPositionVec3;
    }
    public void Released(HVRGrabberBase hvrbase, HVRGrabbable grabbable)
    {
        lastTouchedTime = Time.time;
        //        Debug.Log("Time Touched Object: " + lastTouchedTime);
        lastItemTouched = null;
        surfacesTouched = 0;
        surfacesTouchedSet.Clear();

    }

    public bool IsGamePiecePlaced()
    {
        return gamePiecePlaced;
    }
    public void PlacePiece(bool isItPlaced)
    {
        // destroy the components so it can't be picked up again
        if (isItPlaced)
        {
             
            rb.isKinematic = true;
            Debug.Log("Game piece is placed");
            this.gamePiecePlaced = true;
            HVRGrabbable grabbable = GetComponent<HVRGrabbable>();
            grabbable.enabled = false;
            // if being carried by a vehicle it might be parented to it.
            if (originalParent != this.transform.parent)
            {
                Debug.Log("Reparenting to original parent!");
                this.transform.SetParent(originalParent);
            }

            RemoveGlassSphere();
            
        }
        else
        {
            Debug.Log("This piece was UN placed");
            this.gamePiecePlaced = false;
        }
        Debug.Log("IsGamePiecePlaced " + this.IsGamePiecePlaced());
    }
    private void OnCollisionEnter(Collision other)
    {
        //  Debug.Log("Last Item Touched: " + other.gameObject.name);
        CalculateSurfacesTouched(other);
        MakeBounceSound();
        // TODO keep a set of surfaces touched.  Insure that a surface is only counted once.  Exclude certain surfaces like the floor.

    }

    private void MakeBounceSound() {
        /*Debug.Log("make bounce"+this.bounceSound+" rb "+rb+" this "+gameObject.name);
        if (!this.bounceSound.isPlaying  && rb!=null) {
            this.bounceSound.volume = Mathf.Clamp(rb.velocity.magnitude / 4.0f,0f,1f);
            this.bounceSound.Play();
        }
        */
    }
 
    public void RemoveGlassSphere() {
        this.transform.Find("GlassSphereHolder").GetComponent<MeshRenderer>().enabled = false;
        this.GetComponent<SphereCollider>().enabled = false;

    }

    private void AddGlassSphere() {
        this.transform.Find("GlassSphereHolder").GetComponent<MeshRenderer>().enabled = true;
        this.GetComponent<SphereCollider>().enabled = true;
    }

    private void CalculateSurfacesTouched(Collision other)
    {
        lastItemTouched = other.gameObject;
        if (!(objectNamesToNotCountForSurfacesTouchedSet.Contains(lastItemTouched.name)))
        {

            surfacesTouchedSet.Add(lastItemTouched.name);
            surfacesTouched = surfacesTouchedSet.Count;
            //  Debug.Log("ADDED: " + lastItemTouched.name);
        }
    }

 
}
