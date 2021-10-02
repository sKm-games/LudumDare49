using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class KillZone : MonoBehaviour
{
    [SerializeField] private FirstPersonController player;
    [SerializeField] private RequesterController requester;

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("kill zone hit by " + other.tag);
        if (other.tag == "Player")
        {         
            player.Respawn();
        }
        else if (other.tag == "PickUp")
        {            
            PickUpObject pu = other.GetComponent<PickUpObject>();
            requester.RemoveObjectFromList(pu.GetObjectInfo);
        }
    }
}
