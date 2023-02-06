using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FT_Boat : MonoBehaviour
{
 

 private void OnTriggerEnter(Collider other) {
    Debug.Log("Boat onTriggerEnter other.tag >"+other.gameObject.tag+"  other.gameObject.name>"+other.gameObject.name);
    if (other.gameObject.tag == "Terrain") {
        this.gameObject.SetActive(false);

    } else if (other.gameObject.tag == "Water") {
        this.gameObject.SetActive(true);

    }
 }

 private void OnCollisionEnter(Collision other) {
    Debug.Log("Boat onCollisionEnter other.tag >"+other.gameObject.tag+"  other.gameObject.name>"+other.gameObject.name);
 }

 
}
