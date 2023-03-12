using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using HurricaneVR.Framework.Core.Player;

public class FT_Portal : MonoBehaviour
{
   public Transform destination;
    

    public string playerTag = "Player"; 
    [SerializeField]
   
    private AudioSource audioSource;
public HVRTeleporter Teleporter { get; set; }
    // Start is called before the first frame update
    void Start()      
    {
        audioSource = this.GetComponent<AudioSource>();   
        
            Teleporter = GameObject.FindObjectsOfType<HVRTeleporter>().FirstOrDefault(e => e.gameObject.activeInHierarchy);
         

    }

 

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == playerTag)
        {
         //  other.gameObject.transform.position =  destination.position + new Vector3(1.5f,0,1.5f);

         Teleporter.Teleport(destination.position + new Vector3(1.5f,0,1.5f), destination.forward);
            audioSource.Play();
        }
    }
}
