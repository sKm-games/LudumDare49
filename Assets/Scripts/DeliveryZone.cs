using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeliveryZone : MonoBehaviour
{

    [SerializeField] private RequesterController requester;
    [SerializeField] private PlayerInteraction playerInteraction;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PickUp")
        {

            PickUpObject puo = other.transform.GetComponent<PickUpObject>();
            ObjectInfoDataClass oid = puo.GetObjectInfo;

            ObjectInfoDataClass rid = requester.GetRequestedObject;

            if ((rid.ObjectType == oid.ObjectType && rid.ObjectColor == oid.ObjectColor))
            {
                Debug.Log("Correct object");
                Destroy(puo.gameObject);
                requester.Correct();
            }
            else
            {
                Debug.Log("Wrong object");
                Destroy(puo.gameObject);
                requester.Wrong(oid);
            }

            playerInteraction.CheckObject();
        }
    }

}
