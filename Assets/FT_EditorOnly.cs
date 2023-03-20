using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FT_EditorOnly : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] editorOnlyObjects = GameObject.FindGameObjectsWithTag("EditorOnly");

        foreach (GameObject editorOnlyObject in editorOnlyObjects)
        {
            editorOnlyObject.SetActive(false);
        }
    }

}
