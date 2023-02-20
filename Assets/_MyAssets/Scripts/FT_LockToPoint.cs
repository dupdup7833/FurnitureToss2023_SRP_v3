using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HurricaneVR.Framework.Core;


public class FT_LockToPoint : MonoBehaviour
{
    public Transform snapTo;
    
    private Rigidbody body;
    public float snapTime = 2;

    private float dropTimer;
    private HVRGrabbable grabbable;


    private void Start()
    {
        grabbable = GetComponent<HVRGrabbable>();
        body = GetComponent<Rigidbody>();
        
    }

    private void FixedUpdate()
    {
        bool used = false;
        if (grabbable != null){

            used = grabbable.IsHandGrabbed || grabbable.IsBeingForcedGrabbed;
        }

        if (used)
        {
            body.isKinematic = false;
            dropTimer = -1;
        }
        else
        {
            dropTimer += Time.deltaTime / (snapTime / 2);

            body.isKinematic = dropTimer > 1;

            if (dropTimer > 1)
            {
                //transform.parent = snapTo;
                transform.position = snapTo.position;
                transform.rotation = snapTo.rotation;
            }
            else
            {
                float t = Mathf.Pow(35, dropTimer);

                body.velocity = Vector3.Lerp(body.velocity, Vector3.zero, Time.fixedDeltaTime * 4);
                if (body.useGravity)
                    body.AddForce(-Physics.gravity);

                transform.position = Vector3.Lerp(transform.position, snapTo.position, Time.fixedDeltaTime * t * 3);
                transform.rotation = Quaternion.Slerp(transform.rotation, snapTo.rotation, Time.fixedDeltaTime * t * 2);
            }
        }
    }
}
