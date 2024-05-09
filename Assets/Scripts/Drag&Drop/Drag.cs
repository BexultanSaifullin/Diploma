using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using Cinemachine;

public class Drag : MonoBehaviour
{
    public Collider currentCollider2;
    private Camera mainCamera;
    private Plane dragPlane;
    private bool inputStart;
    private Vector3 offset;
    private Vector3 newPosition;
    [SerializeField] private GameObject selectedObject;
    private GameObject instantiatedPrefab;
    string free = "free";
    string busy = "busy";
    GameObject parentObject;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    GameManagerScr GameManager;
    CardSpawnerScr Spawner;
    private Animator jutSpellAnimation, arrowsSpellAnimation;
    private GameEntryMenu gameEntryMenu;
    public CinemachineVirtualCamera CameraWoman;
    CameraChanger CameraMan;
    public Transform PlayerHand;
    public Transform[] predefinedObjects;



    private void Start()
    {
        mainCamera = Camera.main;
        //ArrangeCards();
        GameManager = FindObjectOfType<GameManagerScr>();
        Spawner = FindObjectOfType<CardSpawnerScr>();
        CameraMan = FindObjectOfType<CameraChanger>();
        gameEntryMenu = FindObjectOfType<GameEntryMenu>();
        jutSpellAnimation = gameEntryMenu.jutSpellPlayer.GetComponent<Animator>();
        arrowsSpellAnimation = gameEntryMenu.arrowsSpellPlayer.GetComponent<Animator>();
    }


    private void Update()
    {

        if (Input.GetMouseButtonDown(0) && GameManager.IsPlayerTurn)
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

        //UnityEditor.TransformWorldPlacementJSON:{ "position":{ "x":0.5799999833106995,"y":11.950000762939454,"z":26.05999755859375},"rotation":{ "x":0.00031384939211420715,"y":0.9330788254737854,"z":-0.3596709370613098,"w":0.0008142059668898582},"scale":{ "x":1.0,"y":1.0,"z":1.0} }

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
        initialPosition = currentCollider2.transform.position;
        initialRotation = currentCollider2.transform.rotation;

        newPosition = new Vector3(1.588f, 34.428f, 15.976f);
        //UnityEditor.TransformWorldPlacementJSON:{ "position":{ "x":1.5879707336425782,"y":34.42770004272461,"z":15.976553916931153},"rotation":{ "x":0.0,"y":0.0,"z":0.0,"w":1.0},"scale":{ "x":0.11999999731779099,"y":0.11999999731779099,"z":0.11999999731779099} }
        currentCollider2.transform.position = newPosition;
        //UnityEditor.TransformWorldPlacementJSON:{ "position":{ "x":-2.9802322387695315e-8,"y":8.940696716308594e-8,"z":9.5367431640625e-7},"rotation":{ "x":0.0,"y":0.0,"z":0.0,"w":1.0},"scale":{ "x":0.14999999105930329,"y":0.19349999725818635,"z":0.05999999865889549} }
        Vector3 rotationAngles = new Vector3(0f, 0f, 0f); //UnityEditor.TransformWorldPlacementJSON:{ "position":{ "x":0.6687134504318237,"y":36.89822769165039,"z":15.648843765258789},"rotation":{ "x":-3.090862321641907e-8,"y":0.7071068286895752,"z":-0.7071068286895752,"w":-3.090862321641907e-8},"scale":{ "x":0.14999999105930329,"y":0.19349999725818635,"z":0.05999999865889549} }
        currentCollider2.transform.rotation = Quaternion.Euler(rotationAngles);
    }

    private void BackFromAbove()
    {
        currentCollider2.transform.SetPositionAndRotation(initialPosition, initialRotation);
        //ArrangeCards();
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
            GameManager.PlayerMana -= selectedObject.GetComponent<CardInfoScr>().SelfCard.Mana;
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

            GameManager.PlayerMana -= selectedObject.GetComponent<CardInfoScr>().SelfCard.Mana;
            GameManager.ShowMana();

            if (selectedObject.GetComponent<CardInfoScr>().SelfCard.Name == "Yurt")
            {
                GameManager.PlayerCardsCount++;
                Spawner.Spawn();
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
                GameManager.PlayerMana -= selectedObject.GetComponent<CardInfoScr>().SelfCard.Mana;
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
                GameManager.PlayerMana -= selectedObject.GetComponent<CardInfoScr>().SelfCard.Mana;
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
        Animator anim = instantiatedPrefab.GetComponent<Animator>();
        anim.Play("SpawnAnimationTest");
    }
    public void JutSpellSpawn(GameObject posToSpell)
    {
        Debug.Log(posToSpell.name);
        // gameEntryMenu.jutSpell.transform.parent.transform.rotation = Quaternion.Euler(new Vector3(0, 180f, 0));
        // jutSpellAnimation.Play(posToSpell.name);
        //ArrangeCards();
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
        //ArrangeCards();
        Debug.Log(posToSpell.name);
        arrowsSpellAnimation.Play(posToSpell.name);
    }


    public void ArrangeCards()
    {
        Debug.Log("Запустился");
        int numChildren = PlayerHand.childCount;  // Количество дочерних объектов
        int numPredefined = predefinedObjects.Length;  // Количество предопределенных объектов
        if (numPredefined == 0)
        {
            Debug.LogError("No predefined objects set.");
            return;
        }

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
