using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class Drag : MonoBehaviour
{
    [SerializeField] private Collider currentCollider;
    public Collider currentCollider2;
    private Camera mainCamera;
    private Plane dragPlane;
    private bool inputStart;
    private Vector3 offset;
    private Vector3 newPosition;
    private bool coroutineCalled = false;
    public string defaultLayerName = "Playing";
    private GameObject selectedObject;
    public string free = "free";
    public string busy = "busy";
    public GameObject parentObject;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    GameManagerScr GameManager;

    // Start is called before the first frame update
    private void Start()
    {
        mainCamera = Camera.main;
        ArrangeCards();
        GameManager = FindObjectOfType<GameManagerScr>();
    }

    // Update is called once per frame
    private void Update()
    {

        if (Input.GetMouseButtonDown(0) && currentCollider2 == null && GameManager.IsPlayerTurn)
        {
            SelectPart();
        }
        //if (Input.GetMouseButtonUp(0))
        //{
        //    Drop();
        //}
        if (Input.GetButtonDown("Jump") && currentCollider2 != null && mainCamera.transform.position.y < 2f && currentCollider2.CompareTag("Card") && selectedObject.layer == LayerMask.NameToLayer("Robot"))
        {
            StartCoroutine(StepFromAbove());
        }
        if (Input.GetButtonDown("Jump") && currentCollider2 != null && mainCamera.transform.position.y == 5.51f && currentCollider2.CompareTag("Card") && selectedObject.layer == LayerMask.NameToLayer("Robot"))
        {
            BackFromAbove();
        }
        if (Input.GetMouseButtonDown(0) && mainCamera.transform.position.y == 5.51f)
        {
            Teleportation();
        }
        //Wait();




        // if (currentCollider2 != null && mainCamera.transform.position.y > 2f && !coroutineCalled)
        // {
        //     newPosition = mainCamera.transform.position + mainCamera.transform.forward * 5f;

        //     currentCollider2.transform.position = newPosition;
        // }


        //DragAndDropObject();
    }
    private void SelectPart()
    {
        RaycastHit hit;
        Ray camRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(camRay, out hit, 2000f, LayerMask.GetMask("Robot")))
        {
            currentCollider = hit.collider;
            currentCollider2 = currentCollider;
            selectedObject = hit.collider.gameObject;
            if (mainCamera.transform.position.y == 5.51f)
            {
                newPosition = mainCamera.transform.position +
                                 mainCamera.transform.forward * 0.35f - mainCamera.transform.right * 0.5f;
                currentCollider2.transform.position = newPosition;
                Vector3 rotationAngles = new Vector3(90f, 0f, 0f);
                currentCollider2.transform.rotation = Quaternion.Euler(rotationAngles);
                GameObject a = currentCollider.transform.parent.gameObject;
                selectedObject.transform.parent = parentObject.transform;
                a.gameObject.tag = free;
            }
            dragPlane = new Plane(mainCamera.transform.forward, currentCollider.transform.position);
            float planeDist;
            dragPlane.Raycast(camRay, out planeDist);
            offset = currentCollider.transform.position - camRay.GetPoint(planeDist);
        }
        currentCollider = null;
    }

    //private void DragAndDropObject()
    //{
    //    if (currentCollider == null)
    //    {
    //        return;
    //    }
    //    Ray camRay = mainCamera.ScreenPointToRay(Input.mousePosition);
    //    float planeDist;
    //    dragPlane.Raycast(camRay, out planeDist);
    //    currentCollider.transform.position = camRay.GetPoint(planeDist) + offset;

    // if (currentCollider.transform.position.y < 0.5f)
    // {
    //     currentCollider.transform.position =
    //         new Vector3(currentCollider.transform.position.x,
    //             0.5f,
    //             currentCollider.transform.position.z);
    // }

    //}

    //private void Drop()
    //{
    //    if (currentCollider == null)
    //    {
    //        return;
    //    }
    //    // currentCollider.transform.position =
    //    //     new Vector3(currentCollider.transform.position.x,
    //    //         0.5f,
    //    //         currentCollider.transform.position.z);
    //    currentCollider = null;
    //}

    private IEnumerator StepFromAbove()
    {

        yield return new WaitForSeconds(2f);

        if (mainCamera.transform.position.y == 5.51f)
        {
            initialPosition = currentCollider2.transform.position;
            initialRotation = currentCollider2.transform.rotation;

            newPosition = mainCamera.transform.position +
                                     mainCamera.transform.forward * 0.35f - mainCamera.transform.right * 0.5f;

            currentCollider2.transform.position = newPosition;
            Vector3 rotationAngles = new Vector3(90f, 0f, 0f);
            currentCollider2.transform.rotation = Quaternion.Euler(rotationAngles);

            coroutineCalled = true;
        }
    }

    private void BackFromAbove()
    {
        currentCollider2.transform.SetPositionAndRotation(initialPosition, initialRotation);
        currentCollider2 = null;
    }


    private void Teleportation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;



        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag == free)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                currentCollider2.transform.position = hit.point;
                selectedObject.layer = LayerMask.NameToLayer("Playing");
                hit.collider.gameObject.tag = busy;
                if (selectedObject.layer == LayerMask.NameToLayer("Playing"))
                {
                    selectedObject = null;
                    currentCollider2 = null;
                }
            }

        }
    }

    // public void ArrangeCards()
    // {
    //     float distanceBetweenCards = 0.2f;

    //     int robotLayer = LayerMask.NameToLayer("Robot");
    //     List<GameObject> robotCards = new List<GameObject>();
    //     GameObject[] origin = GameObject.FindGameObjectsWithTag("Card");

    //     for (int i = 0; i < origin.Length; i++)
    //     {
    //         if (origin[i].layer == robotLayer)
    //         {
    //             robotCards.Add(origin[i]);
    //         }
    //     }
    //     GameObject[] cards = robotCards.ToArray();

    //     if (cards.Length == 0)
    //     {
    //         return;
    //     }

    //     float totalWidth = (cards.Length - 1) * distanceBetweenCards;
    //     Vector3 centerPosition = new Vector3(0, 0.54f, -0.36f);

    //     float startX = centerPosition.x - totalWidth / 2;

    //     for (int i = 0; i < cards.Length; i++)
    //     {
    //         float xPos = startX + i * distanceBetweenCards;
    //         Vector3 cardPosition = new Vector3(xPos, centerPosition.y, centerPosition.z);
    //         cards[i].transform.position = cardPosition;
    //     }
    // }
    public void ArrangeCards()
    {
        float distanceBetweenCards = 0.2f;
        int robotLayer = LayerMask.NameToLayer("Robot");

        GameObject[] origin = GameObject.FindGameObjectsWithTag("Card");

        // Use LINQ to filter cards by layer
        GameObject[] cards = origin.Where(card => card.layer == robotLayer).ToArray();

        if (cards.Length == 0)
        {
            return;
        }

        float totalWidth = (cards.Length - 1) * distanceBetweenCards;
        Vector3 centerPosition = new Vector3(0, 0.54f, -0.36f);

        // Use LINQ to calculate startX
        float startX = centerPosition.x - totalWidth / 2;

        // Use foreach loop for cleaner code
        foreach (var card in cards)
        {
            float xPos = startX + Array.IndexOf(cards, card) * distanceBetweenCards;
            Vector3 cardPosition = new Vector3(xPos, centerPosition.y, centerPosition.z);
            card.transform.position = cardPosition;
        }
    }
    public void ArrangeCardsToEnemy()
    {
        float distanceBetweenCards = 0.2f;
        int robotLayer = LayerMask.NameToLayer("Enemy");

        GameObject[] origin = GameObject.FindGameObjectsWithTag("Card");

        // Use LINQ to filter cards by layer
        GameObject[] cards = origin.Where(card => card.layer == robotLayer).ToArray();

        if (cards.Length == 0)
        {
            return;
        }

        float totalWidth = (cards.Length - 1) * distanceBetweenCards;
        Vector3 centerPosition = new Vector3(0, 0.54f, -0.36f);

        // Use LINQ to calculate startX
        float startX = centerPosition.x - totalWidth / 2;

        // Use foreach loop for cleaner code
        foreach (var card in cards)
        {
            float xPos = startX + Array.IndexOf(cards, card) * distanceBetweenCards;
            Vector3 cardPosition = new Vector3(xPos, centerPosition.y, centerPosition.z);
            card.transform.position = cardPosition;
        }
    }
}
