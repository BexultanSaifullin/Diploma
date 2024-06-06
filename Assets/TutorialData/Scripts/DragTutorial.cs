using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using Cinemachine;


public class DragTutorial : InformationManagerTutorialScr
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
    CameraChangerTutorial Cameraman;
    public Vector3 initialPosition;
    public Quaternion initialRotation;
    public GameManagerTutorialScr GameManager;
    private Animator jutSpellAnimation, arrowsSpellAnimation;
    private GameEntryMenu gameEntryMenu;
    public Transform PlayerHand;
    public Transform[] predefinedObjects;


    private void Start()
    {
        mainCamera = Camera.main;
        Cameraman = FindObjectOfType<CameraChangerTutorial>();
        //gameEntryMenu = FindObjectOfType<GameEntryMenu>();
        //jutSpellAnimation = gameEntryMenu.jutSpellPlayer.GetComponent<Animator>();
        //arrowsSpellAnimation = gameEntryMenu.arrowsSpellPlayer.GetComponent<Animator>();
    }


    private void Update()
    {

        if (Input.GetMouseButtonDown(0) && base.IsPlayerTurn)
        {
            SelectPart();
        }
        //if (Input.GetButtonDown("Jump") && mainCamera.transform.position.y == 11.95f && currentCollider2.CompareTag("Card") && selectedObject.layer == LayerMask.NameToLayer("Robot"))
        //{
        //    StepFromAbove();
        //}
        if (Input.GetButtonDown("Jump") && mainCamera.transform.position.y != 11.95f && currentCollider2.CompareTag("Card") && selectedObject.layer == LayerMask.NameToLayer("Robot"))
        {
            BackFromAbove();
        }
        
        if (Input.GetMouseButtonDown(0))
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
            if (selectedObject.GetComponent<CardInfoTutorialScr>().SelfCard.Mana > PlayerMana)
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
            Debug.Log(selectedObject.GetComponent<CardInfoTutorialScr>().SelfCard.Name);

            if(selectedObject.GetComponent<CardInfoTutorialScr>().SelfCard.Name == "Jut")
            {
                Cameraman.VirtualCameras[0].Priority = 0;
                Cameraman.VirtualCameras[3].Priority = 1;
                Cameraman.currentCameraIndex = 3;
                StepFromAboveForJut();
            }
            else if (selectedObject.GetComponent<CardInfoTutorialScr>().SelfCard.Type == "Building" || selectedObject.GetComponent<CardInfoTutorialScr>().SelfCard.Name == "Heal")
            {
                Cameraman.VirtualCameras[0].Priority = 0;
                Cameraman.VirtualCameras[4].Priority = 1;
                Cameraman.currentCameraIndex = 4;
                StepFromAboveForBuilding();
            }
            else if(selectedObject.GetComponent<CardInfoTutorialScr>().SelfCard.Type == "Unit" || selectedObject.GetComponent<CardInfoTutorialScr>().SelfCard.Type == "Spell")
            {
                Cameraman.VirtualCameras[0].Priority = 0;
                Cameraman.VirtualCameras[2].Priority = 1;
                Cameraman.currentCameraIndex = 2;
                StepFromAboveForUnit();
            } 
            else 
            {
                Cameraman.SwitchCamera();
            }
            
        }
    }
    private void StepFromAboveForUnit()
    {

        selectedObject.GetComponent<CardOnHover>().enabled = false;
        initialPosition = currentCollider2.transform.position;
        initialRotation = currentCollider2.transform.rotation;
        //UnityEditor.TransformWorldPlacementJSON:{ "position":{ "x":1.3744081258773804,"y":10.089056015014649,"z":4.8812255859375},"rotation":{ "x":0.3289490044116974,"y":0.0,"z":0.0,"w":0.9443476796150208},"scale":{ "x":8.0,"y":8.0,"z":8.0} }
        newPosition = new Vector3(1.374f, 10.09f, 4.88f);
        currentCollider2.transform.position = newPosition;
        Vector3 rotationAngles = new Vector3(38.41f, 0f, 0f);
        currentCollider2.transform.rotation = Quaternion.Euler(rotationAngles);
    }
    private void StepFromAboveForJut()
    {
        //UnityEditor.TransformWorldPlacementJSON:{ "position":{ "x":1.4540218114852906,"y":13.819530487060547,"z":0.5195266008377075},"rotation":{ "x":0.2923717498779297,"y":0.0,"z":0.0,"w":0.9563047885894775},"scale":{ "x":8.0,"y":8.0,"z":8.0} }        //UnityEditor.TransformWorldPlacementJSON:{"position":{"x":-0.28999996185302737,"y":16.190000534057618,"z":1.6799999475479127},"rotation":{"x":0.0,"y":0.8829476237297058,"z":-0.4694715142250061,"w":0.0},"scale":{"x":1.0,"y":1.0,"z":1.0}}
        selectedObject.GetComponent<CardOnHover>().enabled = false;
        initialPosition = currentCollider2.transform.position;
        initialRotation = currentCollider2.transform.rotation;
        newPosition = new Vector3(1.454f, 13.82f, 0.52f);
        currentCollider2.transform.position = newPosition;
        Vector3 rotationAngles = new Vector3(34f, 0f, 0f);
        currentCollider2.transform.rotation = Quaternion.Euler(rotationAngles);
    }

    private void StepFromAboveForBuilding()
    {
        //UnityEditor.TransformWorldPlacementJSON:{ "position":{ "x":-2.0034916400909426,"y":11.800000190734864,"z":-0.009336352348327637},"rotation":{ "x":-1.5308051715123839e-8,"y":0.9366722106933594,"z":-0.3502073884010315,"w":-4.0943241685909018e-8},"scale":{ "x":8.0,"y":8.0,"z":8.0} }
        selectedObject.GetComponent<CardOnHover>().enabled = false;
        initialPosition = currentCollider2.transform.position;
        initialRotation = currentCollider2.transform.rotation;
        newPosition = new Vector3(-2f, 11.8f, -0.01f);
        currentCollider2.transform.position = newPosition;
        Vector3 rotationAngles = new Vector3(41f, 180f, 0f);
        currentCollider2.transform.rotation = Quaternion.Euler(rotationAngles);
    }

    private void BackFromAbove()
    {
        selectedObject.GetComponent<CardOnHover>().enabled = true;
        ArrangeCards();

        currentCollider2 = null;
    }


    private void Teleportation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;



        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag == free && hit.collider.gameObject.layer == LayerMask.NameToLayer("Default") && selectedObject.GetComponent<CardInfoTutorialScr>().SelfCard.Type == "Unit")
        {
            Vector3 rotationAngles = new Vector3(0f, 0f, 0f);
            currentCollider2.transform.rotation = Quaternion.Euler(rotationAngles);
            if (selectedObject.GetComponent<CardInfoTutorialScr>().SelfCard.Abyllity == true)
            {
                if (selectedObject.GetComponent<CardInfoTutorialScr>().SelfCard.Name == "Ensign")
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
            PlayerMana -= selectedObject.GetComponent<CardInfoTutorialScr>().SelfCard.Mana;
            base.ShowManaPlayer();

            selectedObject.transform.localScale = new Vector3(8f, 8f, 8f);
            CardModelSpawn(selPos, selectedObject);
            instantiatedPrefab.transform.parent = selectedObject.transform;
            
            selectedObject = null;
            currentCollider2 = null;


        }
        else if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag == free && hit.collider.gameObject.layer == LayerMask.NameToLayer("PlayerBuildings") && selectedObject.GetComponent<CardInfoTutorialScr>().SelfCard.Type == "Building")
        {
            Vector3 rotationAngles = new Vector3(0f, 0f, 0f);
            currentCollider2.transform.rotation = Quaternion.Euler(rotationAngles);
            Vector3 selPos = hit.collider.gameObject.transform.position;
            selPos.y += 0.01f;
            selectedObject.transform.parent = hit.collider.gameObject.transform;
            selectedObject.layer = LayerMask.NameToLayer("Played");
            currentCollider2.transform.position = selPos;
            
            hit.collider.gameObject.tag = busy;

            ArrangeCards();

            PlayerMana -= selectedObject.GetComponent<CardInfoTutorialScr>().SelfCard.Mana;
            base.ShowManaPlayer();

            if (selectedObject.GetComponent<CardInfoTutorialScr>().SelfCard.Name == "Yurt")
            {
                PlayerCardsCount++;
                Spawner.NotRandomSpawn();

            }
            else if (selectedObject.GetComponent<CardInfoTutorialScr>().SelfCard.Name == "Barak" || selectedObject.GetComponent<CardInfoTutorialScr>().SelfCard.Name == "Bowrange" || selectedObject.GetComponent<CardInfoTutorialScr>().SelfCard.Name == "Shield")
            {
                GameManager.BaffUnits(selectedObject.GetComponent<CardInfoTutorialScr>().SelfCard.Name);

            }

            selectedObject.transform.localScale = new Vector3(8f, 8f, 8f);
           
            CardModelSpawn(selPos, selectedObject);
            instantiatedPrefab.transform.parent = selectedObject.transform;

            selectedObject = null;
            currentCollider2 = null;

        }
        else if (Physics.Raycast(ray, out hit) && (hit.collider.gameObject.layer == LayerMask.NameToLayer("EnemyPlaying") || hit.collider.gameObject.layer == LayerMask.NameToLayer("EnemyPlayed")) && selectedObject.GetComponent<CardInfoTutorialScr>().SelfCard.Type == "Spell")
        {

            if (selectedObject.GetComponent<CardInfoTutorialScr>().SelfCard.Name == "Jut" && hit.collider.gameObject.GetComponent<CardInfoTutorialScr>().SelfCard.Type == "Building")
            {
                hit.collider.gameObject.GetComponent<CardInfoTutorialScr>().SelfCard.GetDamage(selectedObject.GetComponent<CardInfoTutorialScr>().SelfCard.Attack);
                hit.collider.gameObject.GetComponent<CardInfoTutorialScr>().RefreshData();
                JutSpellSpawn(hit.collider.transform.parent.gameObject);
                PlayerMana -= selectedObject.GetComponent<CardInfoTutorialScr>().SelfCard.Mana;
                base.ShowManaPlayer();
                DestroyImmediate(selectedObject);
                if (hit.collider.gameObject.GetComponent<CardInfoTutorialScr>().SelfCard.Defense <= 0)
                {
                    Transform parentTransform = hit.collider.gameObject.transform.parent;
                    GameObject childTransform = parentTransform.gameObject;
                    childTransform.tag = "free";
                    DestroyImmediate(hit.collider.gameObject);
                }
            }
            else if (selectedObject.GetComponent<CardInfoTutorialScr>().SelfCard.Name == "Arrows")
            {
                hit.collider.gameObject.GetComponent<CardInfoTutorialScr>().SelfCard.GetDamage(selectedObject.GetComponent<CardInfoTutorialScr>().SelfCard.Attack);
                hit.collider.gameObject.GetComponent<CardInfoTutorialScr>().RefreshData();
                ArrowsSpellSpawn(hit.collider.transform.parent.gameObject);
                PlayerMana -= selectedObject.GetComponent<CardInfoTutorialScr>().SelfCard.Mana;
                base.ShowManaPlayer();
                DestroyImmediate(selectedObject);
                if (hit.collider.gameObject.GetComponent<CardInfoTutorialScr>().SelfCard.Defense <= 0)
                {
                    Transform parentTransform = hit.collider.gameObject.transform.parent;
                    GameObject childTransform = parentTransform.gameObject;
                    childTransform.tag = "free";
                    DestroyImmediate(hit.collider.gameObject);
                }
            }
            ArrangeCards();
        } else if (Physics.Raycast(ray, out hit) && (hit.collider.gameObject.layer == LayerMask.NameToLayer("PlayerWallBox") ) && selectedObject.GetComponent<CardInfoTutorialScr>().SelfCard.Name == "Heal" && PlayerWallHP > 0)
        {
            PlayerWallHP += selectedObject.GetComponent<CardInfoTutorialScr>().SelfCard.Defense;
            
            PlayerMana -= selectedObject.GetComponent<CardInfoTutorialScr>().SelfCard.Mana;
            base.ShowManaPlayer();
            GameManager.ShowHPWall();
            DestroyImmediate(selectedObject);
            ArrangeCards();
        }
    }

    public void CardModelSpawn(Vector3 selPos, GameObject selectedObject)
    {
        GameObject prefab = Models[selectedObject.GetComponent<CardInfoTutorialScr>().SelfCard.Id];
        instantiatedPrefab = Instantiate(prefab, selPos, Quaternion.identity);
        if (selectedObject.GetComponent<CardInfoTutorialScr>().SelfCard.Id == 2)
        {
            instantiatedPrefab.transform.rotation = Quaternion.Euler(new Vector3(instantiatedPrefab.transform.rotation.x, instantiatedPrefab.transform.rotation.y + 180f, instantiatedPrefab.transform.rotation.z));
        }
        else if (selectedObject.GetComponent<CardInfoTutorialScr>().SelfCard.Id == 3 || selectedObject.GetComponent<CardInfoTutorialScr>().SelfCard.Id == 4)
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

        jutSpellAnimation.Play(posToSpell.name);
        // if (posToSpell.name == "A")
        //     jutSpellAnimation.Play("D");
        // if (posToSpell.name == "B")
        //     jutSpellAnimation.Play("C");
        // if (posToSpell.name == "C")
        //     jutSpellAnimation.Play("B");
        // if (posToSpell.name == "D")
        //     jutSpellAnimation.Play("A");
    }

    public void ArrowsSpellSpawn(GameObject posToSpell)
    {

        Debug.Log(posToSpell.name);
        arrowsSpellAnimation.Play(posToSpell.name);
    }

    public void ArrangeCards()
    {

        int numChildren = PlayerHand.childCount;  // ���������� �������� ��������
        int numPredefined = predefinedObjects.Length;  // ���������� ���������������� ��������

        for (int i = 0; i < numChildren; i++)
        {
            Transform child = PlayerHand.GetChild(i);

            // �������� ������ �� ������ ���������������� �������� �� ������,
            // ����� �������� ������ �� ������� �������
            Transform predefined = predefinedObjects[i % numPredefined];

            // ����������� ������� � ������� �� ����������������� �������
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
        Vector3 centerPosition = new Vector3(0.511f, 9.6f, -5.55f);

        float startX = centerPosition.x - totalWidth / 2;

        foreach (var card in cards)
        {
            float xPos = startX + Array.IndexOf(cards, card) * distanceBetweenCards;
            Vector3 cardPosition = new Vector3(xPos, centerPosition.y, centerPosition.z);
            card.transform.position = cardPosition;
        }
        //UnityEditor.TransformWorldPlacementJSON:{"position":{"x":0.5110000371932983,"y":9.59999942779541,"z":-5.550000190734863},"rotation":{"x":0.0,"y":0.0,"z":0.0,"w":1.0},"scale":{"x":1.0,"y":1.0,"z":1.0}}
    }

}
