using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalAI : MonoBehaviour
{
    public Transform destination;
    public Transform player;

    public string playerTag; 
    [SerializeField]
   
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()      
    {
        audioSource = this.GetComponent<AudioSource>();   
       

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == playerTag)
        {
            player.transform.position = destination.position;
            audioSource.Play();
        }
    }
}
