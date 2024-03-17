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
    public GameObject newParent;
    public GameObject parentObject;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    GameManagerScr GameManager;

    private void Start()
    {
        mainCamera = Camera.main;
        ArrangeCards();
        GameManager = FindObjectOfType<GameManagerScr>();
    }
    private void Update()
    {

        if (Input.GetMouseButtonDown(0) && currentCollider2 == null && GameManager.IsPlayerTurn)
        {
            SelectPart();
        }
       
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
            dragPlane = new Plane(mainCamera.transform.forward, currentCollider.transform.position);
            float planeDist;
            dragPlane.Raycast(camRay, out planeDist);
            offset = currentCollider.transform.position - camRay.GetPoint(planeDist);
        }
        currentCollider = null;
    }

    private IEnumerator StepFromAbove()
    {

        yield return new WaitForSeconds(2f);

        if (mainCamera.transform.position.y == 5.51f)
        {
            initialPosition = currentCollider2.transform.position;
            initialRotation = currentCollider2.transform.rotation;

            newPosition = mainCamera.transform.position +
                                     mainCamera.transform.forward * 0.35f - mainCamera.transform.right * 0.4f;

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

        selectedObject.transform.SetParent(newParent.transform);
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
        int EnemyLayer = LayerMask.NameToLayer("Enemy");

        GameObject[] origin = GameObject.FindGameObjectsWithTag("EnemyCard");

        // Use LINQ to filter cards by layer
        GameObject[] cards = origin.Where(card => card.layer == EnemyLayer).ToArray();

        if (cards.Length == 0)
        {
            return;
        }

        float totalWidth = (cards.Length - 1) * distanceBetweenCards;
        Vector3 centerPosition = new Vector3(0, 0.54f, 9.429f);

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
