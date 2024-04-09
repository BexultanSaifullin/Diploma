using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class Drag : MonoBehaviour
{
    public Collider currentCollider2;
    private Camera mainCamera;
    private Plane dragPlane;
    private bool inputStart;
    private Vector3 offset;
    private Vector3 newPosition;
    [SerializeField] private GameObject selectedObject;
    string free = "free";
    string busy = "busy";
    GameObject parentObject;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    GameManagerScr GameManager;
    public GameObject[] cardModels;


    private void Start()
    {
        mainCamera = Camera.main;
        ArrangeCards();
        GameManager = FindObjectOfType<GameManagerScr>();
    }


    private void Update()
    {

        if (Input.GetMouseButtonDown(0) && GameManager.IsPlayerTurn)
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
            currentCollider2 = hit.collider;
            selectedObject = hit.collider.gameObject;
            if (selectedObject.GetComponent<CardInfoScr>().SelfCard.Mana > GameManager.PlayerMana)
            {
                selectedObject = null;
                currentCollider2 = null;
                return;
            }
            //if (mainCamera.transform.position.y == 5.51f)
            //{
            //    newPosition = mainCamera.transform.position +
            //                     mainCamera.transform.forward * 0.35f - mainCamera.transform.right * 0.5f;
            //    currentCollider2.transform.position = newPosition;
            //    Vector3 rotationAngles = new Vector3(90f, 0f, 0f);
            //    currentCollider2.transform.rotation = Quaternion.Euler(rotationAngles);
            //    GameObject a = currentCollider.transform.parent.gameObject;
            //    selectedObject.transform.parent = parentObject.transform;
            //    a.gameObject.tag = free;
            //} COD CHTOBI SDELAT POLE NA KOTOROM STOYALA KARTA SVOBODNIM PRI PODNYTII
            dragPlane = new Plane(mainCamera.transform.forward, currentCollider2.transform.position);
            float planeDist;
            dragPlane.Raycast(camRay, out planeDist);
            offset = currentCollider2.transform.position - camRay.GetPoint(planeDist);
        }
    }

    private IEnumerator StepFromAbove()
    {

        yield return new WaitForSeconds(1f);

        if (mainCamera.transform.position.y == 5.51f)
        {
            initialPosition = currentCollider2.transform.position;
            initialRotation = currentCollider2.transform.rotation;

            newPosition = mainCamera.transform.position +
                                     mainCamera.transform.forward * 0.35f - mainCamera.transform.right * 0.4f;

            currentCollider2.transform.position = newPosition;
            Vector3 rotationAngles = new Vector3(90f, 0f, 0f);
            currentCollider2.transform.rotation = Quaternion.Euler(rotationAngles);
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
                Vector3 selPos = hit.collider.gameObject.transform.position;
                selPos.y += 0.01f;
                currentCollider2.transform.position = selPos;
                selectedObject.layer = LayerMask.NameToLayer("Played");
                hit.collider.gameObject.tag = busy;
                selectedObject.transform.parent = hit.collider.gameObject.transform;
                ArrangeCards();
                if (selectedObject.layer == LayerMask.NameToLayer("Played"))
                {
                    GameManager.PlayerMana -= selectedObject.GetComponent<CardInfoScr>().SelfCard.Mana;
                    GameManager.ShowMana();
                    selectedObject.transform.localScale = new Vector3(9.47f, 8.95f, 3.73f);
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

        GameObject[] cards = origin.Where(card => card.layer == robotLayer).ToArray();

        if (cards.Length == 0)
        {
            return;
        }

        float totalWidth = (cards.Length - 1) * distanceBetweenCards;
        Vector3 centerPosition = new Vector3(0, 0.54f, -0.36f);

        float startX = centerPosition.x - totalWidth / 2;

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

        GameObject[] cards = origin.Where(card => card.layer == EnemyLayer).ToArray();

        if (cards.Length == 0)
        {
            return;
        }

        float totalWidth = (cards.Length - 1) * distanceBetweenCards;
        Vector3 centerPosition = new Vector3(0, 0.54f, 9.429f);

        float startX = centerPosition.x - totalWidth / 2;

        foreach (var card in cards)
        {
            float xPos = startX + Array.IndexOf(cards, card) * distanceBetweenCards;
            Vector3 cardPosition = new Vector3(xPos, centerPosition.y, centerPosition.z);
            card.transform.position = cardPosition;
        }
    }
}
