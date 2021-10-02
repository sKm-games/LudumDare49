using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class RequesterController : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private Transform objectHolder;
    [SerializeField] private List<PickUpObject> pickUpObjects;

    [SerializeField] private ObjectInfoDataClass requestedObject;
    public ObjectInfoDataClass GetRequestedObject
    {
        get
        {
            return requestedObject;
        }
    }
       

    [SerializeField] private List<GameObject> faceses;
    [SerializeField] private List<GameObject> bodies;

    [SerializeField] private int angryStatus;

    private Transform mainBody;
    [SerializeField] Transform player;

    [SerializeField] private ParticleSystem shootParticle;
    [SerializeField] private AudioClip shotClip;
    private AudioSource source;

    [SerializeField] private float roundTimer;
    private float activeTimer;
    [SerializeField] private Image timerImage;
    [SerializeField] private TextMeshProUGUI requestedObjectText;

    bool runTimer;

    private void Awake()
    {
        mainBody = transform.GetChild(2);
        Transform canvas = this.transform.GetChild(3);
        requestedObjectText = canvas.GetChild(0).GetComponent<TextMeshProUGUI>();
        timerImage = canvas.GetChild(2).GetComponent<Image>();
        pickUpObjects = new List<PickUpObject>(objectHolder.GetComponentsInChildren<PickUpObject>());
        source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        SelectRequestedObject();        
    }

    private void Update()
    {
        Vector3 target = new Vector3(player.position.x, mainBody.position.y, player.position.z);
        mainBody.LookAt(target);

        if (runTimer)
        {
            activeTimer -= Time.deltaTime;
            float fill = activeTimer / roundTimer;
            timerImage.fillAmount = fill;
            if (fill <= 0.5f && angryStatus == 1)
            {
                angryStatus++;
                UpdateAnger(angryStatus);
            }
            if (fill <= 0.25f && angryStatus == 2)
            {
                angryStatus++;
                UpdateAnger(angryStatus);
            }
            if (activeTimer <= 0)
            {
                runTimer = false;
                GameOver();
            }
        }
    }

    private void SelectRequestedObject()
    {
        angryStatus = 1;
        UpdateAnger(angryStatus);

        activeTimer = roundTimer;
        timerImage.fillAmount = 1;

        if (pickUpObjects.Count == 0)
        {
            requestedObjectText.text = "Done!";
            return;
        }
        pickUpObjects.Shuffle();
        requestedObject = pickUpObjects[0].GetObjectInfo;
        pickUpObjects.RemoveAt(0);
        requestedObjectText.text = requestedObject.Object.GetObjectString();

        runTimer = true;
    }

    public void Correct()
    {
        runTimer = false;
        //happy animation and score increase        
        angryStatus--;
        UpdateAnger(angryStatus);
        requestedObjectText.text = "Correct";
        Invoke("SelectRequestedObject", 1f);
    }

    public void Wrong(ObjectInfoDataClass obi)
    {
        //angry animation and score decrease?
        RemoveObjectFromList(obi);
        angryStatus++;
        if (angryStatus > 3)
        {
            GameOver();
        }   
        else
        {
            UpdateAnger(angryStatus);
        }        
    }   

    public void RemoveObjectFromList(ObjectInfoDataClass obi) //for wrong and out of map objects
    {
        pickUpObjects.Remove(obi.Object);
    }

    private void UpdateAnger(int i)
    {
        foreach (GameObject f in faceses)
        {
            f.SetActive(false);
        }

        foreach (GameObject b in bodies)
        {
            b.SetActive(false);
        }

        if (i == 0)
        {
            faceses[0].SetActive(true);
            bodies[0].SetActive(true);
        }
        else if (i == 1)
        {
            faceses[1].SetActive(true);
            bodies[0].SetActive(true);
        }
        else if (i == 2)
        {
            faceses[2].SetActive(true);
            bodies[0].SetActive(true);
        }
        else if (i == 3)
        {
            faceses[2].SetActive(true);
            bodies[1].SetActive(true);
        }
    }

    private void GameOver()
    {
        float p = Random.Range(0.9f, 1.2f);
        source.pitch = p;
        source.PlayOneShot(shotClip);
        shootParticle.Play();
        gameController.GameOver();
    }
}
