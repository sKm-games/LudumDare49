using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactDistance;
    [SerializeField] private LayerMask interactLayerMask;
    [SerializeField] private TextMeshProUGUI interactUIText;

    [SerializeField] private float pickUpSpeed;
    [SerializeField] private bool holdingObject;
    
    private Transform objectHolder;
    private Rigidbody heldObject;
    private Transform interactObject;

    private float throwPower;
    [SerializeField] private float maxThrowPower;
    private bool allowThrow;

    private void Awake()
    {
        objectHolder = transform.GetChild(0).GetChild(1);
    }

    private void Start()
    {
        interactUIText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!holdingObject)
        {
            InteractRay();
        }
        
        if (Input.GetButtonDown("Fire1"))
        {
            if (!holdingObject && interactObject != null)
            {
                PickUpObject();
            }
            /*else if (holdingObject && heldObject != null)
            {
                DropObject();
            }   */         
        }else if (Input.GetButton("Fire1") && holdingObject && allowThrow)
        {
            if (throwPower < maxThrowPower)
            {
                throwPower += 1000 * Time.deltaTime;
            }            
        }
        else if (Input.GetButtonUp("Fire1") && holdingObject)
        {
            if (!allowThrow)
            {
                allowThrow = true;
                return;
            }
            ThrowObjects();
        }
    }

    private void InteractRay()
    {
        RaycastHit hit;

        Vector3 rayPos = Camera.main.transform.position;
        rayPos = new Vector3(rayPos.x, rayPos.y, rayPos.z);

        Physics.Raycast(rayPos, Camera.main.transform.forward, out hit, interactDistance, interactLayerMask);
        //Debug.DrawRay(rayPos, Camera.main.transform.forward * interactDistance, Color.red, 600); 
        if (hit.collider != null)
        {
            if (hit.transform.tag == "PickUp")
            {
                interactObject = hit.transform;

                PickUpObject puo = interactObject.GetComponent<PickUpObject>();
                interactUIText.text = puo.GetObjectString();
                
                interactUIText.gameObject.SetActive(true);
                return;
            }            
        }
        interactUIText.gameObject.SetActive(false);
        interactObject = null;
    }

    private void PickUpObject()
    {
        heldObject = interactObject.GetComponent<Rigidbody>();
        heldObject.isKinematic = true;

        //heldObject.transform.DOMove(objectHolder.position, 0.5f);
        heldObject.transform.position = objectHolder.position;
        heldObject.transform.SetParent(objectHolder);        
        holdingObject = true;
        interactObject = null;
    }

    private void ThrowObjects()
    {
        //Debug.Log("Throw power: " + throwPower);
        heldObject.transform.SetParent(null);
        heldObject.isKinematic = false;
        heldObject.AddForce(Camera.main.transform.forward * throwPower);
        heldObject = null;
        holdingObject = false;
        throwPower = 0;
        allowThrow = false;
    }

    private void DropObject() //old
    {
        heldObject.transform.SetParent(null);
        heldObject.isKinematic = false;
        heldObject = null;
        holdingObject = false;
    }

    public void CheckObject()
    {
        if (holdingObject)
        {
            heldObject = null;
            holdingObject = false;
            throwPower = 0;
            allowThrow = false;
        }
    }
}
