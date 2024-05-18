using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using Cinemachine;

public class Drag : InformationManagerScr
{
    public Collider currentCollider2;
    private Camera mainCamera;
    private Plane dragPlane;
    private Vector3 offset;
    private Vector3 newPosition;
    [SerializeField] private GameObject selectedObject;
    private GameObject instantiatedPrefab;
    string free = "free";
    string busy = "busy";
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    GameManagerScr GameManager;
    CardSpawnerScr Spawner;
    private Animator jutSpellAnimation, arrowsSpellAnimation;
    private GameEntryMenu gameEntryMenu;
    public Transform PlayerHand;
    public Transform[] predefinedObjects;



    private void Start()
    {
        mainCamera = Camera.main;
        GameManager = FindObjectOfType<GameManagerScr>();
        Spawner = FindObjectOfType<CardSpawnerScr>();
        gameEntryMenu = FindObjectOfType<GameEntryMenu>();
        jutSpellAnimation = gameEntryMenu.jutSpellPlayer.GetComponent<Animator>();
        arrowsSpellAnimation = gameEntryMenu.arrowsSpellPlayer.GetComponent<Animator>();
    }


    private void Update()
    {

        if (Input.GetMouseButtonDown(0) && base.IsPlayerTurn)
        {
            SelectPart();
        }
        if (Input.GetButtonDown("Jump") && currentCollider2 != null && mainCamera.transform.position.y == 11.950000762939454f && currentCollider2.CompareTag("Card") && selectedObject.layer == LayerMask.NameToLayer("Robot"))
        {
            StepFromAbove();
        }
        if (Input.GetButtonDown("Jump") && currentCollider2 != null && mainCamera.transform.position.y == 37.88f && currentCollider2.CompareTag("Card") && selectedObject.layer == LayerMask.NameToLayer("Robot"))
        {
            BackFromAbove();
        }
        if (Input.GetMouseButtonDown(0) && mainCamera.transform.position.y == 37.88f)
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
            if (selectedObject.GetComponent<CardInfoScr>().SelfCard.Mana > PlayerMana)
            {
                selectedObject = null;
                currentCollider2 = null;
                return;
            }
            //if (mainCamera.transform.position.y == 37.88f)
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
            Debug.Log(selectedObject.GetComponent<CardInfoScr>().SelfCard.Name);
        }
    }

    private void StepFromAbove()
    {
        selectedObject.GetComponent<CardOnHover>().enabled = false;
        
        initialPosition = currentCollider2.transform.position;
        initialRotation = currentCollider2.transform.rotation;

        newPosition = new Vector3(1.588f, 34.428f, 15.976f);
        currentCollider2.transform.position = newPosition;
        Vector3 rotationAngles = new Vector3(0f, 0f, 0f); 
        currentCollider2.transform.rotation = Quaternion.Euler(rotationAngles);
    }

    private void BackFromAbove()
    {
        selectedObject.GetComponent<CardOnHover>().enabled = true;
        currentCollider2.transform.SetPositionAndRotation(initialPosition, initialRotation);
        
        currentCollider2 = null;
    }


    private void Teleportation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;



        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag == free && hit.collider.gameObject.layer == LayerMask.NameToLayer("Default") && selectedObject.GetComponent<CardInfoScr>().SelfCard.Type == "Unit")
        {
            if (selectedObject.GetComponent<CardInfoScr>().SelfCard.Abyllity == true)
            {
                if (selectedObject.GetComponent<CardInfoScr>().SelfCard.Name == "Ensign")
                {
                    GameManager.BaffAbillity();
                }
            }
            Vector3 selPos = hit.collider.gameObject.transform.position;
            selPos.y += 0.01f;
            currentCollider2.transform.position = selPos;
            selectedObject.layer = LayerMask.NameToLayer("Played");
            hit.collider.gameObject.tag = busy;
            selectedObject.transform.parent = hit.collider.gameObject.transform;
            ArrangeCards();
            PlayerMana -= selectedObject.GetComponent<CardInfoScr>().SelfCard.Mana;
            GameManager.ShowMana();

            selectedObject.transform.localScale = new Vector3(8f, 8f, 8f);
            CardModelSpawn(selPos, selectedObject);
            instantiatedPrefab.transform.parent = selectedObject.transform;

            selectedObject = null;
            currentCollider2 = null;


        }
        else if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag == free && hit.collider.gameObject.layer == LayerMask.NameToLayer("PlayerBuildings") && selectedObject.GetComponent<CardInfoScr>().SelfCard.Type == "Building")
        {
            Vector3 selPos = hit.collider.gameObject.transform.position;
            selPos.y += 0.01f;
            selectedObject.transform.parent = hit.collider.gameObject.transform;
            selectedObject.layer = LayerMask.NameToLayer("Played");
            currentCollider2.transform.position = selPos;
            hit.collider.gameObject.tag = busy;

            ArrangeCards();

            PlayerMana -= selectedObject.GetComponent<CardInfoScr>().SelfCard.Mana;
            GameManager.ShowMana();

            if (selectedObject.GetComponent<CardInfoScr>().SelfCard.Name == "Yurt")
            {
                PlayerCardsCount++;
                Spawner.NotRandomSpawn();

            }
            else if (selectedObject.GetComponent<CardInfoScr>().SelfCard.Name == "Barak" || selectedObject.GetComponent<CardInfoScr>().SelfCard.Name == "Bowrange")
            {
                GameManager.BaffUnits(selectedObject.GetComponent<CardInfoScr>().SelfCard.Name);

            }

            selectedObject.transform.localScale = new Vector3(8f, 8f, 8f);
            CardModelSpawn(selPos, selectedObject);
            instantiatedPrefab.transform.parent = selectedObject.transform;

            selectedObject = null;
            currentCollider2 = null;

        }
        else if (Physics.Raycast(ray, out hit) && (hit.collider.gameObject.layer == LayerMask.NameToLayer("EnemyPlaying") || hit.collider.gameObject.layer == LayerMask.NameToLayer("EnemyPlayed")) && selectedObject.GetComponent<CardInfoScr>().SelfCard.Type == "Spell")
        {

            if (selectedObject.GetComponent<CardInfoScr>().SelfCard.Name == "Jut" && hit.collider.gameObject.GetComponent<CardInfoScr>().SelfCard.Type == "Building")
            {
                hit.collider.gameObject.GetComponent<CardInfoScr>().SelfCard.GetDamage(selectedObject.GetComponent<CardInfoScr>().SelfCard.Attack);
                hit.collider.gameObject.GetComponent<CardInfoScr>().RefreshData();
                JutSpellSpawn(hit.collider.transform.parent.gameObject);
                PlayerMana -= selectedObject.GetComponent<CardInfoScr>().SelfCard.Mana;
                GameManager.ShowMana();
                DestroyImmediate(selectedObject);
                if (hit.collider.gameObject.GetComponent<CardInfoScr>().SelfCard.Defense <= 0)
                {
                    Transform parentTransform = hit.collider.gameObject.transform.parent;
                    GameObject childTransform = parentTransform.gameObject;
                    childTransform.tag = "free";
                    DestroyImmediate(hit.collider.gameObject);
                }
            }
            else if (selectedObject.GetComponent<CardInfoScr>().SelfCard.Name == "Arrows")
            {
                hit.collider.gameObject.GetComponent<CardInfoScr>().SelfCard.GetDamage(selectedObject.GetComponent<CardInfoScr>().SelfCard.Attack);
                hit.collider.gameObject.GetComponent<CardInfoScr>().RefreshData();
                ArrowsSpellSpawn(hit.collider.transform.parent.gameObject);
                PlayerMana -= selectedObject.GetComponent<CardInfoScr>().SelfCard.Mana;
                GameManager.ShowMana();
                DestroyImmediate(selectedObject);
                if (hit.collider.gameObject.GetComponent<CardInfoScr>().SelfCard.Defense <= 0)
                {
                    Transform parentTransform = hit.collider.gameObject.transform.parent;
                    GameObject childTransform = parentTransform.gameObject;
                    childTransform.tag = "free";
                    DestroyImmediate(hit.collider.gameObject);
                }
            }
            ArrangeCards();
        }
    }

    public void CardModelSpawn(Vector3 selPos, GameObject selectedObject)
    {
        GameObject prefab = GameManager.Models[selectedObject.GetComponent<CardInfoScr>().SelfCard.Id];
        instantiatedPrefab = Instantiate(prefab, selPos, Quaternion.identity);
        if (selectedObject.GetComponent<CardInfoScr>().SelfCard.Id == 2)
        {
            instantiatedPrefab.transform.rotation = Quaternion.Euler(new Vector3(instantiatedPrefab.transform.rotation.x, instantiatedPrefab.transform.rotation.y + 180f, instantiatedPrefab.transform.rotation.z));
        }
        else if (selectedObject.GetComponent<CardInfoScr>().SelfCard.Id == 3 || selectedObject.GetComponent<CardInfoScr>().SelfCard.Id == 4)
        {
            instantiatedPrefab.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        }
        Animator anim = instantiatedPrefab.GetComponent<Animator>();
        anim.Play("SpawnAnimationTest");
    }

    public void JutSpellSpawn(GameObject posToSpell)
    {
        Debug.Log(posToSpell.name);
        // gameEntryMenu.jutSpell.transform.parent.transform.rotation = Quaternion.Euler(new Vector3(0, 180f, 0));
        // jutSpellAnimation.Play(posToSpell.name);
        
        if (posToSpell.name == "A")
            jutSpellAnimation.Play("D");
        if (posToSpell.name == "B")
            jutSpellAnimation.Play("C");
        if (posToSpell.name == "C")
            jutSpellAnimation.Play("B");
        if (posToSpell.name == "D")
            jutSpellAnimation.Play("A");
    }

    public void ArrowsSpellSpawn(GameObject posToSpell)
    {
        
        Debug.Log(posToSpell.name);
        arrowsSpellAnimation.Play(posToSpell.name);
    }

    public void ArrangeCards()
    {
        
        int numChildren = PlayerHand.childCount;  // Количество дочерних объектов
        int numPredefined = predefinedObjects.Length;  // Количество предопределенных объектов

        for (int i = 0; i < numChildren; i++)
        {
            Transform child = PlayerHand.GetChild(i);

            // Получаем объект из списка предопределенных объектов по модулю,
            // чтобы избежать выхода за границы массива
            Transform predefined = predefinedObjects[i % numPredefined];

            // Присваиваем позицию и поворот из предопределенного объекта
            child.position = predefined.position;
            child.rotation = predefined.rotation;
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
        Vector3 centerPosition = new Vector3(0.513f, 10.132f, 9.884f);

        float startX = centerPosition.x - totalWidth / 2;

        foreach (var card in cards)
        {
            float xPos = startX + Array.IndexOf(cards, card) * distanceBetweenCards;
            Vector3 cardPosition = new Vector3(xPos, centerPosition.y, centerPosition.z);
            card.transform.position = cardPosition;
        }
    }
}
