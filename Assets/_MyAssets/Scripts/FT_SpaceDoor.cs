using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FT_SpaceDoor : MonoBehaviour
{
    Animator anim;
    AudioSource audSource;

    void Start()
    {
        anim = GetComponent<Animator>();
        audSource = GetComponent<AudioSource>();
    }


    public void OpenDoor()
    {
        Debug.Log("Open door " + anim.GetBool("IsDoorOpen"));
        if (!anim.GetBool("IsDoorOpen"))
        {
            anim.SetTrigger("OpenDoor");
        }
    }

    public void CloseDoor()
    {
        Debug.Log("Close door " + anim.GetBool("IsDoorOpen"));
        if (anim.GetBool("IsDoorOpen"))
        {
            anim.SetTrigger("CloseDoor");
        }
    }

    private void PlayOpenSound()
    {
        audSource.Play();
    }
}
