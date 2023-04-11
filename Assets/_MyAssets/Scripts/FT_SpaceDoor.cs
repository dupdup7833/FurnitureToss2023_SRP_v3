using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FT_SpaceDoor : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update

    private AnimatorClipInfo[] clipInfo;
    void Start()
    {
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    public void OpenDoor()
    {
         Debug.Log("Open door "+anim.GetBool("IsDoorOpen"));
        if (!anim.GetBool("IsDoorOpen"))
        {
            anim.SetTrigger("OpenDoor");
        }

    }

    public void CloseDoor()
    {
        Debug.Log("Close door "+anim.GetBool("IsDoorOpen"));
        if (anim.GetBool("IsDoorOpen"))
        {
            anim.SetTrigger("CloseDoor");
        }
    }

    public string GetCurrentClipName()
    {
        int layerIndex = 0;
        clipInfo = anim.GetCurrentAnimatorClipInfo(layerIndex);
        return clipInfo[0].clip.name;
    }
}
