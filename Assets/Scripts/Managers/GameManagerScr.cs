using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

//Debug.Log(count);


public class GameManagerScr : MonoBehaviour
{
    int Turn, TurnTime = 30, increase = 1;
    public GameObject[] Models;
    public Button EndTurnBtn;
    CardSpawnerScr Spawner; CardSpawnerEnemyScr SpawnerEnemy;

    public Transform EnemyHand, PlayerHand;

    public GameObject PlayerWall, EnemyWall, PlayerWall2st, EnemyWall2st, PlayerWall3st, EnemyWall3st,
            PlayerWarior1, PlayerWarior2, EnemyWarrior1, EnemyWarrior2,
                 PlayerKhan, EnemyKhan;

    public GameObject[] ABoxes, BBoxes, CBoxes, DBoxes, PlayerBuildingsBoxes, EnemyBuildingsBoxes;
    public GameObject[] AllBoxes;

    public int PlayerWallHP = 20, EnemyWallHP = 20, WallDMG = 1;

    public int PlayerWarriorHP1 = 6, EnemyWarriorHP1 = 6, PlayerWarriorHP2 = 6, EnemyWarriorHP2 = 6,
                    PlayerKhanHP = 10, EnemyKhanHP = 10;

    public int PlayerMana = 1, EnemyMana = 1, PlayerCardsCount = 10, EnemyCardsCount = 10;

    public TextMeshProUGUI PlayerManaTxt, EnemyManaTxt,
        PlayerWallHPTxt, EnemyWallHPTxt,
            PlayerWarriorHP1Txt, PlayerWarriorHP2Txt, EnemyWarriorHP1Txt, EnemyWarriorHP2Txt,
                PlayerKhanHPTxt, EnemyKhanHPTxt,
                    TurnTimeTxt;
    public int WarriorBaff = 0, EnemyWarriorBaff = 0, ArcheryBaff = 0, EnemyArcheryBaff = 0;

    private WonLostMenu wonLostMenu;
    private GameObject instantiatedPrefab;
    private GameEntryMenu gameEntryMenu;
    private Animator jutSpellAnimation, arrowsSpellAnimation;
    private Quaternion newRotation;


    public bool IsPlayerTurn
    {
        get
        {
            return Turn % 2 == 0;
        }
    }

    void Start()
    {
        ShowMana();
        Turn = 0;
        StartCoroutine(TurnFunc());
        Spawner = FindObjectOfType<CardSpawnerScr>();
        SpawnerEnemy = FindObjectOfType<CardSpawnerEnemyScr>();
        wonLostMenu = FindObjectOfType<WonLostMenu>();
        gameEntryMenu = FindObjectOfType<GameEntryMenu>();
        jutSpellAnimation = gameEntryMenu.jutSpellEnemy.GetComponent<Animator>();
        arrowsSpellAnimation = gameEntryMenu.arrowsSpellEnemy.GetComponent<Animator>();
    }

    IEnumerator TurnFunc()
    {
        TurnTime = 30;
        TurnTimeTxt.text = TurnTime.ToString();
        if (IsPlayerTurn)
        {
            while (TurnTime-- > 0)
            {
                TurnTimeTxt.text = TurnTime.ToString();
                yield return new WaitForSeconds(1);
            }
            ChangeTurn();
        }
        else
        {
            //while (TurnTime-- > 27)
            //{
            //    TurnTimeTxt.text = TurnTime.ToString();
            //    yield return new WaitForSeconds(1);
            //}
            List<GameObject> EnemyPlaces = new List<GameObject>();
            List<GameObject> EnemyCard = new List<GameObject>();
            List<GameObject> EnemyCardBuildings = new List<GameObject>();
            int EnemyLayer = LayerMask.NameToLayer("EnemyPlace");
            for (int i = 0; i < 4; i++)
            {
                if (ABoxes[i].layer == EnemyLayer && ABoxes[i].tag == "free")
                {
                    EnemyPlaces.Add(ABoxes[i]);
                }
                if (BBoxes[i].layer == EnemyLayer && BBoxes[i].tag == "free")
                {
                    EnemyPlaces.Add(BBoxes[i]);
                }
                if (CBoxes[i].layer == EnemyLayer && CBoxes[i].tag == "free")
                {
                    EnemyPlaces.Add(CBoxes[i]);
                }
                if (DBoxes[i].layer == EnemyLayer && DBoxes[i].tag == "free")
                {
                    EnemyPlaces.Add(DBoxes[i]);
                }
            }

            foreach (Transform child in EnemyHand)
            {
                EnemyCard.Add(child.gameObject);
            }

            for (int i = 0; i < 4; i++)
            {
                if (EnemyBuildingsBoxes[i].tag == "free")
                {
                    EnemyCardBuildings.Add(EnemyBuildingsBoxes[i]);
                }
            }



            if (EnemyCard.Count > 0)
            {
                StartCoroutine(EnemyTurn(EnemyCard, EnemyPlaces, EnemyCardBuildings));
            }


        }

    }

    public void ChangeTurn()
    {
        StopAllCoroutines();
        Turn++;
        EndTurnBtn.interactable = IsPlayerTurn;
        if (IsPlayerTurn)
        {
            GameObject[] objectsWithTagCard = GameObject.FindGameObjectsWithTag("Card");
            int PlayerLayerPlayed = LayerMask.NameToLayer("Played");
            GameObject[] objectsOnLayerPlayed = objectsWithTagCard.Where(card => card.layer == PlayerLayerPlayed).ToArray();
            foreach (GameObject obj in objectsOnLayerPlayed)
            {
                obj.layer = LayerMask.NameToLayer("Playing");
            }
            AttackCards();
            PlayerAttackWallAndWarrior();
            EnemyAttackWallAndWarrior();
            DestroyCards();
            PlayerMoveCards();
            if (PlayerHand.childCount < PlayerCardsCount)
            {
                Spawner.Spawn();
            }
            if (PlayerHand.childCount > 0)
            {
                for (int i = 0; i < PlayerHand.childCount; i++)
                {
                    Transform childGameObject = PlayerHand.transform.GetChild(i);
                    GameObject childTransform = childGameObject.gameObject;
                    if (childTransform.GetComponent<CardInfoScr>().SelfCard.Name == "Warrior" && childTransform.GetComponent<CardInfoScr>().SelfCard.Defense == 4)
                    {
                        childTransform.GetComponent<CardInfoScr>().SelfCard.SetBaff(WarriorBaff);
                        childTransform.GetComponent<CardInfoScr>().RefreshData();
                    }
                    if (childTransform.GetComponent<CardInfoScr>().SelfCard.Name == "Archer" && childTransform.GetComponent<CardInfoScr>().SelfCard.Defense == 2)
                    {
                        childTransform.GetComponent<CardInfoScr>().SelfCard.SetBaff(ArcheryBaff);
                        childTransform.GetComponent<CardInfoScr>().RefreshData();
                    }
                }
            }
            if (increase < 10)
                increase += 1;
            if (PlayerMana < 10)
                PlayerMana = increase;
            if (EnemyMana < 10)
                EnemyMana = increase;
            ShowMana();
        }
        else if (!IsPlayerTurn && Turn != 1)
        {
            GameObject[] objectsWithTagCard = GameObject.FindGameObjectsWithTag("EnemyCard");
            int PlayerLayerPlayed = LayerMask.NameToLayer("EnemyPlayed");
            GameObject[] objectsOnLayerPlayed = objectsWithTagCard.Where(card => card.layer == PlayerLayerPlayed).ToArray();
            foreach (GameObject obj in objectsOnLayerPlayed)
            {
                obj.layer = LayerMask.NameToLayer("EnemyPlaying");
            }
            PlayerAttackWallAndWarrior();
            EnemyAttackWallAndWarrior();
            AttackCards();

            DestroyCards();
            EnemyMoveCards();
            if (EnemyHand.childCount < EnemyCardsCount)
            {
                SpawnerEnemy.SpawnEnemy();
            }
            if (EnemyHand.childCount > 0)
            {
                for (int i = 0; i < EnemyHand.childCount; i++)
                {
                    Transform childGameObject = EnemyHand.transform.GetChild(i);
                    GameObject childTransform = childGameObject.gameObject;
                    if (childTransform.GetComponent<CardInfoScr>().SelfCard.Name == "Warrior" && childTransform.GetComponent<CardInfoScr>().SelfCard.Defense == 4)
                    {
                        childTransform.GetComponent<CardInfoScr>().SelfCard.SetBaff(EnemyWarriorBaff);
                        childTransform.GetComponent<CardInfoScr>().RefreshData();
                    }
                    if (childTransform.GetComponent<CardInfoScr>().SelfCard.Name == "Archer" && childTransform.GetComponent<CardInfoScr>().SelfCard.Defense == 2)
                    {
                        childTransform.GetComponent<CardInfoScr>().SelfCard.SetBaff(EnemyArcheryBaff);
                        childTransform.GetComponent<CardInfoScr>().RefreshData();
                    }
                }
            }
        }
        StartCoroutine(TurnFunc());
    }

    IEnumerator EnemyTurn(List<GameObject> EnemyCard, List<GameObject> EnemyPlaces, List<GameObject> EnemyCardBuildings)
    {
        System.Random rng = new System.Random();
        int EnemyPlacesCount = EnemyPlaces.Count - 1;
        int EnemyCardBuildingsCount = EnemyCardBuildings.Count;
        int EnemyCardCount = EnemyCard.Count - 1;
        EnemyCard = EnemyCard.OrderBy(x => Random.value).ToList();
        EnemyPlaces = EnemyPlaces.OrderBy(x => Random.value).ToList();
        EnemyCardBuildings = EnemyCardBuildings.OrderBy(x => Random.value).ToList();


        if (EnemyPlacesCount >= EnemyCardCount)
        {
            for (int i = EnemyCardCount; i >= 0; i--)
            {
                if (EnemyCard[i].GetComponent<CardInfoScr>().SelfCard.Mana > EnemyMana)
                {
                    continue;
                }
                if (EnemyCard[i].GetComponent<CardInfoScr>().SelfCard.Type == "Building")
                {
                    if (EnemyCardBuildings.Count == 0)
                    {
                        continue;
                    }
                    int random = Random.Range(0, EnemyCardBuildingsCount);

                    Vector3 newPosition = EnemyCardBuildings[random].transform.position;
                    newPosition.y += 0.01f;

                    EnemyCard[i].transform.position = newPosition;
                    Vector3 rotationAngles = new Vector3(180f, 0f, -180f);
                    EnemyCard[i].transform.rotation = Quaternion.Euler(rotationAngles);
                    EnemyCard[i].layer = LayerMask.NameToLayer("EnemyPlayed");
                    EnemyCardBuildings[random].gameObject.tag = "busy";
                    EnemyCard[i].transform.parent = EnemyCardBuildings[random].transform;
                    EnemyCard[i].transform.localScale = new Vector3(9.5f, 9.5f, 9.5f);
                    EnemyCardModelSpawn(EnemyCard[i].transform.position, EnemyCard[i]);
                    instantiatedPrefab.transform.parent = EnemyCard[i].transform;
                    EnemyMana -= EnemyCard[i].GetComponent<CardInfoScr>().SelfCard.Mana;
                    ShowMana();
                    if (EnemyCard[i].GetComponent<CardInfoScr>().SelfCard.Name == "Yurt")
                    {
                        SpawnerEnemy.SpawnEnemy();
                        EnemyCardsCount++;
                    }
                    else if (EnemyCard[i].GetComponent<CardInfoScr>().SelfCard.Name == "Barak")
                    {
                        EnemyWarriorBaff++;
                        for (int j = 0; j < 16; j++)
                        {
                            if (AllBoxes[j].tag == "busy")
                            {
                                Transform childGameObject = AllBoxes[j].transform.GetChild(0);
                                GameObject childTransform = childGameObject.gameObject;
                                if (childTransform.GetComponent<CardInfoScr>().SelfCard.Name == "Warrior" && (childTransform.layer == LayerMask.NameToLayer("EnemyPlaying") || childTransform.layer == LayerMask.NameToLayer("EnemyPlayed")))
                                {
                                    childTransform.GetComponent<CardInfoScr>().SelfCard.SetBaff(1);
                                    childTransform.GetComponent<CardInfoScr>().RefreshData();
                                }
                            }
                        }
                        if (EnemyHand.childCount > 0)
                        {
                            for (int j = 0; j < EnemyHand.childCount; j++)
                            {
                                Transform childGameObject = EnemyHand.transform.GetChild(j);
                                GameObject childTransform = childGameObject.gameObject;
                                if (childTransform.GetComponent<CardInfoScr>().SelfCard.Name == "Warrior")
                                {
                                    childTransform.GetComponent<CardInfoScr>().SelfCard.SetBaff(1);
                                    childTransform.GetComponent<CardInfoScr>().RefreshData();
                                }
                            }
                        }
                    }
                    else if (EnemyCard[i].GetComponent<CardInfoScr>().SelfCard.Name == "Bowrange")
                    {
                        EnemyArcheryBaff++;
                        for (int j = 0; j < 16; j++)
                        {
                            if (AllBoxes[j].tag == "busy")
                            {
                                Transform childGameObject = AllBoxes[j].transform.GetChild(0);
                                GameObject childTransform = childGameObject.gameObject;
                                if (childTransform.GetComponent<CardInfoScr>().SelfCard.Name == "Archer" && (childTransform.layer == LayerMask.NameToLayer("EnemyPlaying") || childTransform.layer == LayerMask.NameToLayer("EnemyPlayed")))
                                {
                                    childTransform.GetComponent<CardInfoScr>().SelfCard.SetBaff(1);
                                    childTransform.GetComponent<CardInfoScr>().RefreshData();
                                }
                            }
                        }
                        if (EnemyHand.childCount > 0)
                        {
                            for (int j = 0; j < EnemyHand.childCount; j++)
                            {
                                Transform childGameObject = EnemyHand.transform.GetChild(j);
                                GameObject childTransform = childGameObject.gameObject;
                                if (childTransform.GetComponent<CardInfoScr>().SelfCard.Name == "Archer")
                                {
                                    childTransform.GetComponent<CardInfoScr>().SelfCard.SetBaff(1);
                                    childTransform.GetComponent<CardInfoScr>().RefreshData();
                                }
                            }
                        }
                    }
                    EnemyCardBuildings.RemoveAt(random);
                    EnemyCardBuildingsCount--;
                }
                else if (EnemyCard[i].GetComponent<CardInfoScr>().SelfCard.Type == "Spell")
                {
                    if (EnemyCard[i].GetComponent<CardInfoScr>().SelfCard.Name == "Arrows")
                    {
                        List<GameObject> shallowCopy = new List<GameObject>(AllBoxes);
                        shallowCopy = shallowCopy.OrderBy(x => Random.value).ToList();
                        for (int j = 0; j < 16; j++)
                        {
                            if (shallowCopy[j].tag == "busy")
                            {
                                Transform childGameObject = shallowCopy[j].transform.GetChild(0);
                                GameObject childTransform = childGameObject.gameObject;
                                if ((childTransform.layer == LayerMask.NameToLayer("Playing") || childTransform.layer == LayerMask.NameToLayer("Played")))
                                {
                                    childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(EnemyCard[i].GetComponent<CardInfoScr>().SelfCard.Attack);
                                    childTransform.GetComponent<CardInfoScr>().RefreshData();
                                    EnemyArrowsSpawn(childTransform.transform.parent.gameObject);
                                    DestroyImmediate(EnemyCard[i]);
                                    if (childTransform.GetComponent<CardInfoScr>().SelfCard.Defense <= 0)
                                    {
                                        shallowCopy[j].tag = "free";
                                        DestroyImmediate(childTransform);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    else if (EnemyCard[i].GetComponent<CardInfoScr>().SelfCard.Name == "Jut")
                    {
                        List<GameObject> shallowCopy = new List<GameObject>(PlayerBuildingsBoxes);
                        shallowCopy = shallowCopy.OrderBy(x => Random.value).ToList();
                        for (int j = 0; j < 4; j++)
                        {
                            if (shallowCopy[j].tag == "busy")
                            {
                                Transform childGameObject = shallowCopy[j].transform.GetChild(0);
                                GameObject childTransform = childGameObject.gameObject;
                                if ((childTransform.layer == LayerMask.NameToLayer("Playing") || childTransform.layer == LayerMask.NameToLayer("Played")))
                                {
                                    childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(EnemyCard[i].GetComponent<CardInfoScr>().SelfCard.Attack);
                                    childTransform.GetComponent<CardInfoScr>().RefreshData();
                                    EnemyJutSpawn(childTransform.transform.parent.gameObject);
                                    DestroyImmediate(EnemyCard[i]);
                                    if (childTransform.GetComponent<CardInfoScr>().SelfCard.Defense <= 0)
                                    {
                                        shallowCopy[j].tag = "free";
                                        DestroyImmediate(childTransform);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
                else if (EnemyCard[i].GetComponent<CardInfoScr>().SelfCard.Type == "Unit")
                {
                    Vector3 newPosition = EnemyPlaces[i].transform.position;
                    newPosition.y += 0.01f;
                    EnemyCard[i].transform.position = newPosition;
                    Vector3 rotationAngles = new Vector3(180f, 0f, -180f);
                    EnemyCard[i].transform.rotation = Quaternion.Euler(rotationAngles);
                    EnemyCard[i].layer = LayerMask.NameToLayer("EnemyPlayed");
                    EnemyPlaces[i].gameObject.tag = "busy";
                    EnemyCard[i].transform.parent = EnemyPlaces[i].transform;
                    EnemyCard[i].transform.localScale = new Vector3(8f, 8f, 8f);
                    EnemyCardModelSpawn(EnemyCard[i].transform.position, EnemyCard[i]);
                    instantiatedPrefab.transform.parent = EnemyCard[i].transform;
                    EnemyMana -= EnemyCard[i].GetComponent<CardInfoScr>().SelfCard.Mana;
                    ShowMana();
                }
                yield return new WaitForSeconds(3);
            }
        }
        else
        {
            for (int i = EnemyPlacesCount; i >= 0; i--)
            {
                if (EnemyCard[i].GetComponent<CardInfoScr>().SelfCard.Mana > EnemyMana)
                {
                    continue;
                }
                if (EnemyCard[i].GetComponent<CardInfoScr>().SelfCard.Type == "Building")
                {
                    if (EnemyCardBuildings.Count == 0)
                    {
                        continue;
                    }
                    int random = Random.Range(0, EnemyCardBuildingsCount);
                    Vector3 newPosition = EnemyCardBuildings[random].transform.position;
                    newPosition.y += 0.01f;

                    EnemyCard[i].transform.position = newPosition;
                    Vector3 rotationAngles = new Vector3(180f, 0f, -180f);
                    EnemyCard[i].transform.rotation = Quaternion.Euler(rotationAngles);
                    EnemyCard[i].layer = LayerMask.NameToLayer("EnemyPlayed");
                    EnemyCardBuildings[random].gameObject.tag = "busy";
                    EnemyCard[i].transform.parent = EnemyCardBuildings[random].transform;
                    EnemyCard[i].transform.localScale = new Vector3(9.5f, 9.5f, 9.5f);
                    EnemyCardModelSpawn(EnemyCard[i].transform.position, EnemyCard[i]);
                    instantiatedPrefab.transform.parent = EnemyCard[i].transform;
                    if (EnemyCard[i].GetComponent<CardInfoScr>().SelfCard.Name == "Yurt")
                    {
                        SpawnerEnemy.SpawnEnemy();
                        EnemyCardsCount++;
                    }
                    else if (EnemyCard[i].GetComponent<CardInfoScr>().SelfCard.Name == "Barak")
                    {
                        EnemyWarriorBaff++;
                        for (int j = 0; j < 16; j++)
                        {
                            if (AllBoxes[j].tag == "busy")
                            {
                                Transform childGameObject = AllBoxes[j].transform.GetChild(0);
                                GameObject childTransform = childGameObject.gameObject;
                                if (childTransform.GetComponent<CardInfoScr>().SelfCard.Name == "Warrior" && (childTransform.layer == LayerMask.NameToLayer("EnemyPlaying") || childTransform.layer == LayerMask.NameToLayer("EnemyPlayed")))
                                {
                                    childTransform.GetComponent<CardInfoScr>().SelfCard.SetBaff(1);
                                    childTransform.GetComponent<CardInfoScr>().RefreshData();
                                }
                            }
                        }
                        if (EnemyHand.childCount > 0)
                        {
                            for (int j = 0; j < EnemyHand.childCount; j++)
                            {
                                Transform childGameObject = EnemyHand.transform.GetChild(j);
                                GameObject childTransform = childGameObject.gameObject;
                                if (childTransform.GetComponent<CardInfoScr>().SelfCard.Name == "Warrior")
                                {
                                    childTransform.GetComponent<CardInfoScr>().SelfCard.SetBaff(1);
                                    childTransform.GetComponent<CardInfoScr>().RefreshData();
                                }
                            }
                        }
                    }
                    else if (EnemyCard[i].GetComponent<CardInfoScr>().SelfCard.Name == "Bowrange")
                    {
                        EnemyArcheryBaff++;
                        for (int j = 0; j < 16; j++)
                        {
                            if (AllBoxes[j].tag == "busy")
                            {
                                Transform childGameObject = AllBoxes[j].transform.GetChild(0);
                                GameObject childTransform = childGameObject.gameObject;
                                if (childTransform.GetComponent<CardInfoScr>().SelfCard.Name == "Archer" && (childTransform.layer == LayerMask.NameToLayer("EnemyPlaying") || childTransform.layer == LayerMask.NameToLayer("EnemyPlayed")))
                                {
                                    childTransform.GetComponent<CardInfoScr>().SelfCard.SetBaff(1);
                                    childTransform.GetComponent<CardInfoScr>().RefreshData();
                                }
                            }
                        }
                        if (EnemyHand.childCount > 0)
                        {
                            for (int j = 0; j < EnemyHand.childCount; j++)
                            {
                                Transform childGameObject = EnemyHand.transform.GetChild(j);
                                GameObject childTransform = childGameObject.gameObject;
                                if (childTransform.GetComponent<CardInfoScr>().SelfCard.Name == "Archer")
                                {
                                    childTransform.GetComponent<CardInfoScr>().SelfCard.SetBaff(1);
                                    childTransform.GetComponent<CardInfoScr>().RefreshData();
                                }
                            }
                        }
                    }
                    EnemyMana -= EnemyCard[i].GetComponent<CardInfoScr>().SelfCard.Mana;
                    ShowMana();
                    EnemyCardBuildings.RemoveAt(random);
                    EnemyCardBuildingsCount--;
                }
                else if (EnemyCard[i].GetComponent<CardInfoScr>().SelfCard.Type == "Unit")
                {
                    Vector3 newPosition = EnemyPlaces[i].transform.position;
                    newPosition.y += 0.01f;
                    EnemyCard[i].transform.position = newPosition;
                    Vector3 rotationAngles = new Vector3(180f, 0f, -180f);
                    EnemyCard[i].transform.rotation = Quaternion.Euler(rotationAngles);
                    EnemyCard[i].transform.parent = EnemyPlaces[i].transform;
                    EnemyCard[i].layer = LayerMask.NameToLayer("EnemyPlayed");
                    EnemyCard[i].transform.localScale = new Vector3(8f, 8f, 8f);
                    EnemyCardModelSpawn(EnemyCard[i].transform.position, EnemyCard[i]);
                    instantiatedPrefab.transform.parent = EnemyCard[i].transform;
                    EnemyPlaces[i].gameObject.tag = "busy";
                    EnemyMana -= EnemyCard[i].GetComponent<CardInfoScr>().SelfCard.Mana;
                    ShowMana();
                }
                else if (EnemyCard[i].GetComponent<CardInfoScr>().SelfCard.Type == "Spell")
                {
                    if (EnemyCard[i].GetComponent<CardInfoScr>().SelfCard.Name == "Arrows")
                    {
                        List<GameObject> shallowCopy = new List<GameObject>(AllBoxes);
                        shallowCopy = shallowCopy.OrderBy(x => Random.value).ToList();
                        for (int j = 0; j < 16; j++)
                        {
                            if (shallowCopy[j].tag == "busy")
                            {
                                Transform childGameObject = shallowCopy[j].transform.GetChild(0);
                                GameObject childTransform = childGameObject.gameObject;
                                if ((childTransform.layer == LayerMask.NameToLayer("Playing") || childTransform.layer == LayerMask.NameToLayer("Played")))
                                {
                                    childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(EnemyCard[i].GetComponent<CardInfoScr>().SelfCard.Attack);
                                    childTransform.GetComponent<CardInfoScr>().RefreshData();
                                    DestroyImmediate(EnemyCard[i]);
                                    if (childTransform.GetComponent<CardInfoScr>().SelfCard.Defense <= 0)
                                    {
                                        shallowCopy[j].tag = "free";
                                        DestroyImmediate(childTransform);
                                    }
                                    EnemyCard.RemoveAt(i);
                                    i--;
                                    break;
                                }
                            }
                        }
                    }
                    else if (EnemyCard[i].GetComponent<CardInfoScr>().SelfCard.Name == "Jut")
                    {
                        List<GameObject> shallowCopy = new List<GameObject>(PlayerBuildingsBoxes);
                        shallowCopy = shallowCopy.OrderBy(x => Random.value).ToList();
                        for (int j = 0; j < 4; j++)
                        {
                            if (shallowCopy[j].tag == "busy")
                            {
                                Transform childGameObject = shallowCopy[j].transform.GetChild(0);
                                GameObject childTransform = childGameObject.gameObject;
                                if ((childTransform.layer == LayerMask.NameToLayer("Playing") || childTransform.layer == LayerMask.NameToLayer("Played")))
                                {
                                    childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(EnemyCard[i].GetComponent<CardInfoScr>().SelfCard.Attack);
                                    childTransform.GetComponent<CardInfoScr>().RefreshData();
                                    DestroyImmediate(EnemyCard[i]);
                                    if (childTransform.GetComponent<CardInfoScr>().SelfCard.Defense <= 0)
                                    {
                                        shallowCopy[j].tag = "free";
                                        DestroyImmediate(childTransform);
                                    }
                                    EnemyCard.RemoveAt(i);
                                    i--;
                                    break;
                                }
                            }
                        }
                    }
                }
                yield return new WaitForSeconds(3);
            }
        }
        ChangeTurn();
    }

    void PlayerMoveCards()
    {
        for (int i = 2; i >= 0; i--)//Player move
        {
            if (ABoxes[i].tag == "busy" && ABoxes[i + 1].tag == "free")
            {

                Transform childGameObject = ABoxes[i].transform.GetChild(0);
                GameObject childTransform = childGameObject.gameObject;

                if (childTransform.layer == LayerMask.NameToLayer("Playing") && (i != 2 || childTransform.GetComponent<CardInfoScr>().SelfCard.Range != 2 || EnemyWallHP <= 0))
                {

                    Vector3 newPositiona = ABoxes[i + 1].transform.position;
                    newPositiona.y += 0.01f;
                    childTransform.transform.position = newPositiona;
                    childTransform.transform.parent = ABoxes[i + 1].transform;
                    ABoxes[i + 1].tag = "busy";
                    ABoxes[i].tag = "free";

                }

            }
            if (BBoxes[i].tag == "busy" && BBoxes[i + 1].tag == "free")
            {

                Transform childGameObject = BBoxes[i].transform.GetChild(0);
                GameObject childTransform = childGameObject.gameObject;

                if (childTransform.layer == LayerMask.NameToLayer("Playing") && (i != 2 || childTransform.GetComponent<CardInfoScr>().SelfCard.Range != 2 || EnemyWallHP <= 0))
                {
                    Vector3 newPositiona = BBoxes[i + 1].transform.position;
                    newPositiona.y += 0.01f;
                    childTransform.transform.position = newPositiona;
                    childTransform.transform.parent = BBoxes[i + 1].transform;
                    BBoxes[i + 1].tag = "busy";
                    BBoxes[i].tag = "free";

                }
            }
            if (CBoxes[i].tag == "busy" && CBoxes[i + 1].tag == "free")
            {

                Transform childGameObject = CBoxes[i].transform.GetChild(0);
                GameObject childTransform = childGameObject.gameObject;

                if (childTransform.layer == LayerMask.NameToLayer("Playing") && (i != 2 || childTransform.GetComponent<CardInfoScr>().SelfCard.Range != 2 || EnemyWallHP <= 0))
                {
                    Vector3 newPositiona = CBoxes[i + 1].transform.position;
                    newPositiona.y += 0.01f;
                    childTransform.transform.position = newPositiona;
                    childTransform.transform.parent = CBoxes[i + 1].transform;
                    CBoxes[i + 1].tag = "busy";
                    CBoxes[i].tag = "free";

                }
            }
            if (DBoxes[i].tag == "busy" && DBoxes[i + 1].tag == "free")
            {

                Transform childGameObject = DBoxes[i].transform.GetChild(0);
                GameObject childTransform = childGameObject.gameObject;

                if (childTransform.layer == LayerMask.NameToLayer("Playing") && (i != 2 || childTransform.GetComponent<CardInfoScr>().SelfCard.Range != 2 || EnemyWallHP <= 0))
                {
                    Vector3 newPositiona = DBoxes[i + 1].transform.position;
                    newPositiona.y += 0.01f;
                    childTransform.transform.position = newPositiona;
                    childTransform.transform.parent = DBoxes[i + 1].transform;
                    DBoxes[i + 1].tag = "busy";
                    DBoxes[i].tag = "free";

                }

            }
        }
        for (int i = 0; i < 4; i++)
        {
            if (PlayerBuildingsBoxes[i].tag == "busy")
            {

                Transform childGameObject = PlayerBuildingsBoxes[i].transform.GetChild(0);
                GameObject childTransform = childGameObject.gameObject;
                childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(1);
                childTransform.GetComponent<CardInfoScr>().RefreshData();
            }
        }
    }
    void EnemyMoveCards()
    {
        for (int i = 1; i < 4; i++)//Enemy move
        {
            if (ABoxes[i].tag == "busy" && ABoxes[i - 1].tag == "free")
            {

                Transform childGameObject = ABoxes[i].transform.GetChild(0);
                GameObject childTransform = childGameObject.gameObject;

                if (childTransform.layer == LayerMask.NameToLayer("EnemyPlaying") && (i != 1 || childTransform.GetComponent<CardInfoScr>().SelfCard.Range != 2 || PlayerWallHP <= 0))
                {
                    Vector3 newPositiona = ABoxes[i - 1].transform.position;
                    newPositiona.y += 0.01f;
                    childTransform.transform.position = newPositiona;
                    childTransform.transform.parent = ABoxes[i - 1].transform;
                    ABoxes[i - 1].tag = "busy";
                    ABoxes[i].tag = "free";

                }
            }
            if (BBoxes[i].tag == "busy" && BBoxes[i - 1].tag == "free")
            {

                Transform childGameObject = BBoxes[i].transform.GetChild(0);
                GameObject childTransform = childGameObject.gameObject;

                if (childTransform.layer == LayerMask.NameToLayer("EnemyPlaying") && (i != 1 || childTransform.GetComponent<CardInfoScr>().SelfCard.Range != 2 || PlayerWallHP <= 0))
                {
                    Vector3 newPositiona = BBoxes[i - 1].transform.position;
                    newPositiona.y += 0.01f;
                    childTransform.transform.position = newPositiona;
                    childTransform.transform.parent = BBoxes[i - 1].transform;
                    BBoxes[i - 1].tag = "busy";
                    BBoxes[i].tag = "free";
                }
            }
            if (CBoxes[i].tag == "busy" && CBoxes[i - 1].tag == "free")
            {

                Transform childGameObject = CBoxes[i].transform.GetChild(0);
                GameObject childTransform = childGameObject.gameObject;

                if (childTransform.layer == LayerMask.NameToLayer("EnemyPlaying") && (i != 1 || childTransform.GetComponent<CardInfoScr>().SelfCard.Range != 2 || PlayerWallHP <= 0))
                {
                    Vector3 newPositiona = CBoxes[i - 1].transform.position;
                    newPositiona.y += 0.01f;
                    childTransform.transform.position = newPositiona;
                    childTransform.transform.parent = CBoxes[i - 1].transform;
                    CBoxes[i - 1].tag = "busy";
                    CBoxes[i].tag = "free";
                }

            }
            if (DBoxes[i].tag == "busy" && DBoxes[i - 1].tag == "free")
            {

                Transform childGameObject = DBoxes[i].transform.GetChild(0);
                GameObject childTransform = childGameObject.gameObject;

                if (childTransform.layer == LayerMask.NameToLayer("EnemyPlaying") && (i != 1 || childTransform.GetComponent<CardInfoScr>().SelfCard.Range != 2 || PlayerWallHP <= 0))
                {
                    Vector3 newPositiona = DBoxes[i - 1].transform.position;
                    newPositiona.y += 0.01f;
                    childTransform.transform.position = newPositiona;
                    childTransform.transform.parent = DBoxes[i - 1].transform;
                    DBoxes[i - 1].tag = "busy";
                    DBoxes[i].tag = "free";
                }
            }
        }
        for (int i = 0; i < 4; i++)
        {
            if (EnemyBuildingsBoxes[i].tag == "busy")
            {

                Transform childGameObject = EnemyBuildingsBoxes[i].transform.GetChild(0);
                GameObject childTransform = childGameObject.gameObject;
                childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(1);
                childTransform.GetComponent<CardInfoScr>().RefreshData();
            }
        }
    }

    void AttackCards()
    {
        for (int i = 2; i >= 0; i--)
        {
            if (ABoxes[i].transform.childCount > 0)
            {
                Transform childGameObject = ABoxes[i].transform.GetChild(0);
                GameObject childTransform = childGameObject.gameObject;
                for (int range = 1; range <= childTransform.GetComponent<CardInfoScr>().SelfCard.Range; range++)
                {
                    if (i + range < 4)
                    {
                        if (ABoxes[i + range].transform.childCount > 0)
                        {
                            Transform EnemychildGameObject = ABoxes[i + range].transform.GetChild(0);
                            GameObject EnemychildTransform = EnemychildGameObject.gameObject;
                            if (EnemychildTransform.layer == LayerMask.NameToLayer("EnemyPlaying") &&
                                        childTransform.layer == LayerMask.NameToLayer("Playing"))
                            {
                                EnemychildTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(childTransform.GetComponent<CardInfoScr>().SelfCard.Attack);
                                EnemychildTransform.GetComponent<CardInfoScr>().RefreshData();
                                break;
                            }

                        }
                    }
                    else if (i == 2 && childTransform.GetComponent<CardInfoScr>().SelfCard.Range == 2 && EnemyWallHP > 0 && IsPlayerTurn && childTransform.layer == LayerMask.NameToLayer("Playing"))
                    {
                        EnemyWallHP -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                        if (EnemyWallHP < 10)
                        {
                            EnemyWall.SetActive(false);
                            EnemyWall2st.SetActive(true);

                        }
                        if (EnemyWallHP <= 0)
                        {
                            EnemyWall2st.SetActive(false);
                            EnemyWall3st.SetActive(true);
                        }
                        ShowHPWall();
                    }
                }
            }

            if (BBoxes[i].transform.childCount > 0)
            {
                Transform childGameObject = BBoxes[i].transform.GetChild(0);
                GameObject childTransform = childGameObject.gameObject;
                for (int range = 1; range <= childTransform.GetComponent<CardInfoScr>().SelfCard.Range; range++)
                {
                    if (i + range < 4)
                    {
                        if (BBoxes[i + range].transform.childCount > 0)
                        {
                            Transform EnemychildGameObject = BBoxes[i + range].transform.GetChild(0);
                            GameObject EnemychildTransform = EnemychildGameObject.gameObject;
                            if (EnemychildTransform.layer == LayerMask.NameToLayer("EnemyPlaying") &&
                                        childTransform.layer == LayerMask.NameToLayer("Playing"))
                            {
                                EnemychildTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(childTransform.GetComponent<CardInfoScr>().SelfCard.Attack);
                                EnemychildTransform.GetComponent<CardInfoScr>().RefreshData();
                                break;
                            }

                        }

                    }
                    else if (i == 2 && childTransform.GetComponent<CardInfoScr>().SelfCard.Range == 2 && EnemyWallHP > 0 && IsPlayerTurn && childTransform.layer == LayerMask.NameToLayer("Playing"))
                    {
                        EnemyWallHP -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;

                        if (EnemyWallHP < 10)
                        {
                            EnemyWall.SetActive(false);
                            EnemyWall2st.SetActive(true);

                        }
                        if (EnemyWallHP <= 0)
                        {
                            EnemyWall2st.SetActive(false);
                            EnemyWall3st.SetActive(true);
                        }
                        ShowHPWall();
                    }
                }
            }

            if (CBoxes[i].transform.childCount > 0)
            {
                Transform childGameObject = CBoxes[i].transform.GetChild(0);
                GameObject childTransform = childGameObject.gameObject;
                for (int range = 1; range <= childTransform.GetComponent<CardInfoScr>().SelfCard.Range; range++)
                {
                    if (i + range < 4)
                    {
                        if (CBoxes[i + range].transform.childCount > 0)
                        {
                            Transform EnemychildGameObject = CBoxes[i + range].transform.GetChild(0);
                            GameObject EnemychildTransform = EnemychildGameObject.gameObject;
                            if (EnemychildTransform.layer == LayerMask.NameToLayer("EnemyPlaying") &&
                                        childTransform.layer == LayerMask.NameToLayer("Playing"))
                            {
                                EnemychildTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(childTransform.GetComponent<CardInfoScr>().SelfCard.Attack);
                                EnemychildTransform.GetComponent<CardInfoScr>().RefreshData();
                                break;
                            }

                        }

                    }
                    else if (i == 2 && childTransform.GetComponent<CardInfoScr>().SelfCard.Range == 2 && EnemyWallHP > 0 && IsPlayerTurn && childTransform.layer == LayerMask.NameToLayer("Playing"))
                    {
                        EnemyWallHP -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                        if (EnemyWallHP < 10)
                        {
                            EnemyWall.SetActive(false);
                            EnemyWall2st.SetActive(true);

                        }
                        if (EnemyWallHP <= 0)
                        {
                            EnemyWall2st.SetActive(false);
                            EnemyWall3st.SetActive(true);
                        }
                        ShowHPWall();
                    }
                }
            }

            if (DBoxes[i].transform.childCount > 0)
            {
                Transform childGameObject = DBoxes[i].transform.GetChild(0);
                GameObject childTransform = childGameObject.gameObject;
                for (int range = 1; range <= childTransform.GetComponent<CardInfoScr>().SelfCard.Range; range++)
                {
                    if (i + range < 4)
                    {
                        if (DBoxes[i + range].transform.childCount > 0)
                        {
                            Transform EnemychildGameObject = DBoxes[i + range].transform.GetChild(0);
                            GameObject EnemychildTransform = EnemychildGameObject.gameObject;
                            if (EnemychildTransform.layer == LayerMask.NameToLayer("EnemyPlaying") &&
                                        childTransform.layer == LayerMask.NameToLayer("Playing"))
                            {
                                EnemychildTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(childTransform.GetComponent<CardInfoScr>().SelfCard.Attack);
                                EnemychildTransform.GetComponent<CardInfoScr>().RefreshData();
                                break;
                            }

                        }

                    }
                    else if (i == 2 && childTransform.GetComponent<CardInfoScr>().SelfCard.Range == 2 && EnemyWallHP > 0 && IsPlayerTurn && childTransform.layer == LayerMask.NameToLayer("Playing"))
                    {
                        EnemyWallHP -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                        if (EnemyWallHP < 10)
                        {
                            EnemyWall.SetActive(false);
                            EnemyWall2st.SetActive(true);

                        }
                        if (EnemyWallHP <= 0)
                        {
                            EnemyWall2st.SetActive(false);
                            EnemyWall3st.SetActive(true);
                        }
                        ShowHPWall();
                    }
                }
            }
        }
        // Enemy Attack
        for (int i = 1; i < 4; i++)
        {
            if (ABoxes[i].transform.childCount > 0)
            {
                Transform childGameObject = ABoxes[i].transform.GetChild(0);
                GameObject childTransform = childGameObject.gameObject;
                for (int range = 1; range <= childTransform.GetComponent<CardInfoScr>().SelfCard.Range; range++)
                {
                    if (i - range >= 0)
                    {
                        if (ABoxes[i - range].transform.childCount > 0)
                        {
                            Transform EnemychildGameObject = ABoxes[i - range].transform.GetChild(0);
                            GameObject EnemychildTransform = EnemychildGameObject.gameObject;
                            if (EnemychildTransform.layer == LayerMask.NameToLayer("Playing") &&
                                        childTransform.layer == LayerMask.NameToLayer("EnemyPlaying"))
                            {
                                EnemychildTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(childTransform.GetComponent<CardInfoScr>().SelfCard.Attack);
                                EnemychildTransform.GetComponent<CardInfoScr>().RefreshData();
                                break;
                            }

                        }

                    }
                    else if (i == 1 && childTransform.GetComponent<CardInfoScr>().SelfCard.Range == 2 && PlayerWallHP > 0 && !IsPlayerTurn && childTransform.layer == LayerMask.NameToLayer("EnemyPlaying"))
                    {
                        PlayerWallHP -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                        if (PlayerWallHP < 10)
                        {
                            PlayerWall.SetActive(false);
                            PlayerWall2st.SetActive(true);

                        }
                        if (PlayerWallHP <= 0)
                        {
                            PlayerWall2st.SetActive(false);
                            PlayerWall3st.SetActive(true);
                        }
                        ShowHPWall();
                    }
                }
            }



            if (BBoxes[i].transform.childCount > 0)
            {
                Transform childGameObject = BBoxes[i].transform.GetChild(0);
                GameObject childTransform = childGameObject.gameObject;
                for (int range = 1; range <= childTransform.GetComponent<CardInfoScr>().SelfCard.Range; range++)
                {
                    if (i - range >= 0)
                    {
                        if (BBoxes[i - range].transform.childCount > 0)
                        {
                            Transform EnemychildGameObject = BBoxes[i - range].transform.GetChild(0);
                            GameObject EnemychildTransform = EnemychildGameObject.gameObject;
                            if (EnemychildTransform.layer == LayerMask.NameToLayer("Playing") &&
                                        childTransform.layer == LayerMask.NameToLayer("EnemyPlaying"))
                            {
                                EnemychildTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(childTransform.GetComponent<CardInfoScr>().SelfCard.Attack);
                                EnemychildTransform.GetComponent<CardInfoScr>().RefreshData();
                                break;
                            }

                        }

                    }
                    else if (i == 1 && childTransform.GetComponent<CardInfoScr>().SelfCard.Range == 2 && PlayerWallHP > 0 && !IsPlayerTurn && childTransform.layer == LayerMask.NameToLayer("EnemyPlaying"))
                    {
                        PlayerWallHP -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                        if (PlayerWallHP < 10)
                        {
                            PlayerWall.SetActive(false);
                            PlayerWall2st.SetActive(true);

                        }
                        if (PlayerWallHP <= 0)
                        {
                            PlayerWall2st.SetActive(false);
                            PlayerWall3st.SetActive(true);
                        }
                        ShowHPWall();
                    }
                }
            }




            if (CBoxes[i].transform.childCount > 0)
            {
                Transform childGameObject = CBoxes[i].transform.GetChild(0);
                GameObject childTransform = childGameObject.gameObject;
                for (int range = 1; range <= childTransform.GetComponent<CardInfoScr>().SelfCard.Range; range++)
                {
                    if (i - range >= 0)
                    {
                        if (CBoxes[i - range].transform.childCount > 0)
                        {
                            Transform EnemychildGameObject = CBoxes[i - range].transform.GetChild(0);
                            GameObject EnemychildTransform = EnemychildGameObject.gameObject;
                            if (EnemychildTransform.layer == LayerMask.NameToLayer("Playing") &&
                                        childTransform.layer == LayerMask.NameToLayer("EnemyPlaying"))
                            {
                                EnemychildTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(childTransform.GetComponent<CardInfoScr>().SelfCard.Attack);
                                EnemychildTransform.GetComponent<CardInfoScr>().RefreshData();
                                break;
                            }

                        }
                    }
                    else if (i == 1 && childTransform.GetComponent<CardInfoScr>().SelfCard.Range == 2 && PlayerWallHP > 0 && !IsPlayerTurn && childTransform.layer == LayerMask.NameToLayer("EnemyPlaying"))
                    {
                        PlayerWallHP -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                        if (PlayerWallHP < 10)
                        {
                            PlayerWall.SetActive(false);
                            PlayerWall2st.SetActive(true);

                        }
                        if (PlayerWallHP <= 0)
                        {
                            PlayerWall2st.SetActive(false);
                            PlayerWall3st.SetActive(true);
                        }
                        ShowHPWall();
                    }
                }
            }




            if (DBoxes[i].transform.childCount > 0)
            {
                Transform childGameObject = DBoxes[i].transform.GetChild(0);
                GameObject childTransform = childGameObject.gameObject;
                for (int range = 1; range <= childTransform.GetComponent<CardInfoScr>().SelfCard.Range; range++)
                {
                    if (i - range >= 0)
                    {
                        if (DBoxes[i - range].transform.childCount > 0)
                        {
                            Transform EnemychildGameObject = DBoxes[i - range].transform.GetChild(0);
                            GameObject EnemychildTransform = EnemychildGameObject.gameObject;
                            if (EnemychildTransform.layer == LayerMask.NameToLayer("Playing") &&
                                        childTransform.layer == LayerMask.NameToLayer("EnemyPlaying"))
                            {
                                EnemychildTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(childTransform.GetComponent<CardInfoScr>().SelfCard.Attack);
                                EnemychildTransform.GetComponent<CardInfoScr>().RefreshData();
                                break;
                            }

                        }
                    }
                    else if (i == 1 && childTransform.GetComponent<CardInfoScr>().SelfCard.Range == 2 && PlayerWallHP > 0 && !IsPlayerTurn && childTransform.layer == LayerMask.NameToLayer("EnemyPlaying"))
                    {
                        PlayerWallHP -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                        if (PlayerWallHP < 10)
                        {
                            PlayerWall.SetActive(false);
                            PlayerWall2st.SetActive(true);

                        }
                        if (PlayerWallHP <= 0)
                        {
                            PlayerWall2st.SetActive(false);
                            PlayerWall3st.SetActive(true);
                        }
                        ShowHPWall();
                    }
                }
            }
        }
    }

    void DestroyCards()
    {
        for (int i = 0; i < 4; i++)
        {
            if (ABoxes[i].tag == "busy")
            {
                Transform childGameObject = ABoxes[i].transform.GetChild(0);
                GameObject childTransform = childGameObject.gameObject;
                if (childTransform.GetComponent<CardInfoScr>().SelfCard.Defense <= 0)
                {
                    if (childTransform.GetComponent<CardInfoScr>().SelfCard.Name == "Mystan")
                    {
                        if ((childTransform.layer == LayerMask.NameToLayer("Playing") || childTransform.layer == LayerMask.NameToLayer("Played")) && i != 3)
                        {
                            if (ABoxes[i + 1].tag == "busy")
                            {
                                Transform EnemychildGameObject = ABoxes[i + 1].transform.GetChild(0);
                                GameObject EnemychildTransform = EnemychildGameObject.gameObject;
                                if (EnemychildTransform.layer == LayerMask.NameToLayer("EnemyPlaying") || EnemychildTransform.layer == LayerMask.NameToLayer("EnemyPlayed"))
                                {
                                    DestroyImmediate(EnemychildTransform);
                                    ABoxes[i + 1].tag = "free";
                                }
                            }
                        }
                        else if ((childTransform.layer == LayerMask.NameToLayer("EnemyPlaying") || childTransform.layer == LayerMask.NameToLayer("EnemyPlayed")) && i != 0)
                        {
                            if (ABoxes[i - 1].tag == "busy")
                            {
                                Transform EnemychildGameObject = ABoxes[i - 1].transform.GetChild(0);
                                GameObject EnemychildTransform = EnemychildGameObject.gameObject;
                                if (EnemychildTransform.layer == LayerMask.NameToLayer("Playing") || EnemychildTransform.layer == LayerMask.NameToLayer("Played"))
                                {
                                    DestroyImmediate(EnemychildTransform);
                                    ABoxes[i - 1].tag = "free";
                                }
                            }
                        }
                    }
                    Debug.Log(childTransform.GetComponent<CardInfoScr>().SelfCard.Name);
                    DestroyImmediate(childTransform);
                    ABoxes[i].tag = "free";
                }
            }
            if (BBoxes[i].tag == "busy")
            {
                Transform childGameObject = BBoxes[i].transform.GetChild(0);
                GameObject childTransform = childGameObject.gameObject;
                if (childTransform.GetComponent<CardInfoScr>().SelfCard.Defense <= 0)
                {
                    if (childTransform.GetComponent<CardInfoScr>().SelfCard.Name == "Mystan")
                    {
                        if ((childTransform.layer == LayerMask.NameToLayer("Playing") || childTransform.layer == LayerMask.NameToLayer("Played")) && i != 3)
                        {
                            if (BBoxes[i + 1].tag == "busy")
                            {
                                Transform EnemychildGameObject = BBoxes[i + 1].transform.GetChild(0);
                                GameObject EnemychildTransform = EnemychildGameObject.gameObject;
                                if (EnemychildTransform.layer == LayerMask.NameToLayer("EnemyPlaying") || EnemychildTransform.layer == LayerMask.NameToLayer("EnemyPlayed"))
                                {
                                    DestroyImmediate(EnemychildTransform);
                                    BBoxes[i + 1].tag = "free";
                                }
                            }
                        }
                        else if ((childTransform.layer == LayerMask.NameToLayer("EnemyPlaying") || childTransform.layer == LayerMask.NameToLayer("EnemyPlayed")) && i != 0)
                        {
                            if (BBoxes[i - 1].tag == "busy")
                            {
                                Transform EnemychildGameObject = BBoxes[i - 1].transform.GetChild(0);
                                GameObject EnemychildTransform = EnemychildGameObject.gameObject;
                                if (EnemychildTransform.layer == LayerMask.NameToLayer("Playing") || EnemychildTransform.layer == LayerMask.NameToLayer("Played"))
                                {
                                    DestroyImmediate(EnemychildTransform);
                                    BBoxes[i - 1].tag = "free";
                                }
                            }
                        }
                    }
                    Debug.Log(childTransform.GetComponent<CardInfoScr>().SelfCard.Name);
                    DestroyImmediate(childTransform);
                    BBoxes[i].tag = "free";

                }
            }
            if (CBoxes[i].tag == "busy")
            {
                Transform childGameObject = CBoxes[i].transform.GetChild(0);
                GameObject childTransform = childGameObject.gameObject;
                if (childTransform.GetComponent<CardInfoScr>().SelfCard.Defense <= 0)
                {
                    if (childTransform.GetComponent<CardInfoScr>().SelfCard.Name == "Mystan")
                    {
                        if ((childTransform.layer == LayerMask.NameToLayer("Playing") || childTransform.layer == LayerMask.NameToLayer("Played")) && i != 3)
                        {
                            if (CBoxes[i + 1].tag == "busy")
                            {
                                Transform EnemychildGameObject = CBoxes[i + 1].transform.GetChild(0);
                                GameObject EnemychildTransform = EnemychildGameObject.gameObject;
                                if (EnemychildTransform.layer == LayerMask.NameToLayer("EnemyPlaying") || EnemychildTransform.layer == LayerMask.NameToLayer("EnemyPlayed"))
                                {
                                    DestroyImmediate(EnemychildTransform);
                                    CBoxes[i + 1].tag = "free";
                                }
                            }
                        }
                        else if ((childTransform.layer == LayerMask.NameToLayer("EnemyPlaying") || childTransform.layer == LayerMask.NameToLayer("EnemyPlayed")) && i != 0)
                        {
                            if (CBoxes[i - 1].tag == "busy")
                            {
                                Transform EnemychildGameObject = CBoxes[i - 1].transform.GetChild(0);
                                GameObject EnemychildTransform = EnemychildGameObject.gameObject;
                                if (EnemychildTransform.layer == LayerMask.NameToLayer("Playing") || EnemychildTransform.layer == LayerMask.NameToLayer("Played"))
                                {
                                    DestroyImmediate(EnemychildTransform);
                                    CBoxes[i - 1].tag = "free";
                                }
                            }
                        }
                    }
                    Debug.Log(childTransform.GetComponent<CardInfoScr>().SelfCard.Name);
                    DestroyImmediate(childTransform);
                    CBoxes[i].tag = "free";

                }
            }
            if (DBoxes[i].tag == "busy")
            {
                Transform childGameObject = DBoxes[i].transform.GetChild(0);
                GameObject childTransform = childGameObject.gameObject;
                if (childTransform.GetComponent<CardInfoScr>().SelfCard.Defense <= 0)
                {
                    if (childTransform.GetComponent<CardInfoScr>().SelfCard.Name == "Mystan")
                    {
                        if ((childTransform.layer == LayerMask.NameToLayer("Playing") || childTransform.layer == LayerMask.NameToLayer("Played")) && i != 3)
                        {
                            if (DBoxes[i + 1].tag == "busy")
                            {
                                Transform EnemychildGameObject = DBoxes[i + 1].transform.GetChild(0);
                                GameObject EnemychildTransform = EnemychildGameObject.gameObject;
                                if (EnemychildTransform.layer == LayerMask.NameToLayer("EnemyPlaying") || EnemychildTransform.layer == LayerMask.NameToLayer("EnemyPlayed"))
                                {
                                    DestroyImmediate(EnemychildTransform);
                                    DBoxes[i + 1].tag = "free";
                                }
                            }
                        }
                        else if ((childTransform.layer == LayerMask.NameToLayer("EnemyPlaying") || childTransform.layer == LayerMask.NameToLayer("EnemyPlayed")) && i != 0)
                        {
                            if (DBoxes[i - 1].tag == "busy")
                            {
                                Transform EnemychildGameObject = DBoxes[i - 1].transform.GetChild(0);
                                GameObject EnemychildTransform = EnemychildGameObject.gameObject;
                                if (EnemychildTransform.layer == LayerMask.NameToLayer("Playing") || EnemychildTransform.layer == LayerMask.NameToLayer("Played"))
                                {
                                    DestroyImmediate(EnemychildTransform);
                                    DBoxes[i - 1].tag = "free";
                                }
                            }
                        }
                    }
                    Debug.Log(childTransform.GetComponent<CardInfoScr>().SelfCard.Name);
                    DestroyImmediate(childTransform);

                    DBoxes[i].tag = "free";

                }
            }
            if (PlayerBuildingsBoxes[i].tag == "busy")
            {
                Transform childGameObject = PlayerBuildingsBoxes[i].transform.GetChild(0);
                GameObject childTransform = childGameObject.gameObject;
                if (childTransform.GetComponent<CardInfoScr>().SelfCard.Defense <= 1)
                {
                    if (childTransform.GetComponent<CardInfoScr>().SelfCard.Name == "Yurt")
                    {
                        PlayerCardsCount--;
                    }
                    else if (childTransform.GetComponent<CardInfoScr>().SelfCard.Name == "Barak")
                    {
                        WarriorBaff--;
                        if (PlayerHand.childCount > 0)
                        {
                            for (int j = 0; j < PlayerHand.childCount; j++)
                            {
                                Transform a = PlayerHand.transform.GetChild(j);
                                GameObject b = a.gameObject;
                                if (b.GetComponent<CardInfoScr>().SelfCard.Name == "Warrior" && b.GetComponent<CardInfoScr>().SelfCard.Defense > 4)
                                {
                                    b.GetComponent<CardInfoScr>().SelfCard.GetBaff(1);
                                    b.GetComponent<CardInfoScr>().RefreshData();
                                }
                            }
                        }
                        for (int j = 0; j < 16; j++)
                        {

                            if (AllBoxes[j].tag == "busy")
                            {
                                Transform a = AllBoxes[j].transform.GetChild(0);
                                GameObject b = a.gameObject;
                                if (b.GetComponent<CardInfoScr>().SelfCard.Name == "Warrior" && (b.layer == LayerMask.NameToLayer("Playing") || b.layer == LayerMask.NameToLayer("Played")) && b.GetComponent<CardInfoScr>().SelfCard.Defense > 4)
                                {
                                    b.GetComponent<CardInfoScr>().SelfCard.GetBaff(1);
                                    b.GetComponent<CardInfoScr>().RefreshData();
                                }
                            }

                        }
                    }
                    else if (childTransform.GetComponent<CardInfoScr>().SelfCard.Name == "Bowrange")
                    {
                        ArcheryBaff--;
                        if (PlayerHand.childCount > 0)
                        {
                            for (int j = 0; j < PlayerHand.childCount; j++)
                            {
                                Transform a = PlayerHand.transform.GetChild(j);
                                GameObject b = a.gameObject;
                                if (b.GetComponent<CardInfoScr>().SelfCard.Name == "Archer" && b.GetComponent<CardInfoScr>().SelfCard.Defense > 4)
                                {
                                    b.GetComponent<CardInfoScr>().SelfCard.GetBaff(1);
                                    b.GetComponent<CardInfoScr>().RefreshData();
                                }
                            }
                        }
                        for (int j = 0; j < 16; j++)
                        {

                            if (AllBoxes[j].tag == "busy")
                            {
                                Transform a = AllBoxes[j].transform.GetChild(0);
                                GameObject b = a.gameObject;
                                if (b.GetComponent<CardInfoScr>().SelfCard.Name == "Archer" && (b.layer == LayerMask.NameToLayer("Playing") || b.layer == LayerMask.NameToLayer("Played")) && b.GetComponent<CardInfoScr>().SelfCard.Defense > 4)
                                {
                                    b.GetComponent<CardInfoScr>().SelfCard.GetBaff(1);
                                    b.GetComponent<CardInfoScr>().RefreshData();
                                }
                            }

                        }
                    }
                    Debug.Log(childTransform.GetComponent<CardInfoScr>().SelfCard.Name);
                    DestroyImmediate(childTransform);

                    PlayerBuildingsBoxes[i].tag = "free";

                }

            }

            if (EnemyBuildingsBoxes[i].tag == "busy")
            {
                Transform childGameObject = EnemyBuildingsBoxes[i].transform.GetChild(0);
                GameObject childTransform = childGameObject.gameObject;
                if (childTransform.GetComponent<CardInfoScr>().SelfCard.Defense <= 1)
                {
                    if (childTransform.GetComponent<CardInfoScr>().SelfCard.Name == "Yurt")
                    {
                        EnemyCardsCount--;
                    }
                    else if (childTransform.GetComponent<CardInfoScr>().SelfCard.Name == "Barak")
                    {
                        EnemyWarriorBaff--;
                        if (EnemyHand.childCount > 0)
                        {
                            for (int j = 0; j < EnemyHand.childCount; j++)
                            {
                                Transform a = EnemyHand.transform.GetChild(j);
                                GameObject b = a.gameObject;
                                if (b.GetComponent<CardInfoScr>().SelfCard.Name == "Warrior" && b.GetComponent<CardInfoScr>().SelfCard.Defense > 4)
                                {
                                    b.GetComponent<CardInfoScr>().SelfCard.GetBaff(1);
                                    b.GetComponent<CardInfoScr>().RefreshData();
                                }
                            }
                        }
                        for (int j = 0; j < 16; j++)
                        {

                            if (AllBoxes[j].tag == "busy")
                            {
                                Transform a = AllBoxes[j].transform.GetChild(0);
                                GameObject b = a.gameObject;
                                if (b.GetComponent<CardInfoScr>().SelfCard.Name == "Warrior" && (b.layer == LayerMask.NameToLayer("EnemyPlaying") || b.layer == LayerMask.NameToLayer("EnemyPlayed")) && b.GetComponent<CardInfoScr>().SelfCard.Defense > 4)
                                {
                                    b.GetComponent<CardInfoScr>().SelfCard.GetBaff(1);
                                    b.GetComponent<CardInfoScr>().RefreshData();
                                }
                            }
                        }
                    }
                    else if (childTransform.GetComponent<CardInfoScr>().SelfCard.Name == "Bowrange")
                    {
                        EnemyArcheryBaff--;
                        if (EnemyHand.childCount > 0)
                        {
                            for (int j = 0; j < EnemyHand.childCount; j++)
                            {
                                Transform a = EnemyHand.transform.GetChild(j);
                                GameObject b = a.gameObject;
                                if (b.GetComponent<CardInfoScr>().SelfCard.Name == "Archer" && b.GetComponent<CardInfoScr>().SelfCard.Defense > 2)
                                {
                                    b.GetComponent<CardInfoScr>().SelfCard.GetBaff(1);
                                    b.GetComponent<CardInfoScr>().RefreshData();
                                }
                            }
                        }
                        for (int j = 0; j < 16; j++)
                        {

                            if (AllBoxes[j].tag == "busy")
                            {
                                Transform a = AllBoxes[j].transform.GetChild(0);
                                GameObject b = a.gameObject;
                                if (b.GetComponent<CardInfoScr>().SelfCard.Name == "Archer" && (b.layer == LayerMask.NameToLayer("EnemyPlaying") || b.layer == LayerMask.NameToLayer("EnemyPlayed")) && b.GetComponent<CardInfoScr>().SelfCard.Defense > 2)
                                {
                                    b.GetComponent<CardInfoScr>().SelfCard.GetBaff(1);
                                    b.GetComponent<CardInfoScr>().RefreshData();
                                }
                            }
                        }
                    }
                    Debug.Log(childTransform.GetComponent<CardInfoScr>().SelfCard.Name);
                    DestroyImmediate(childTransform);

                    EnemyBuildingsBoxes[i].tag = "free";

                }
            }
        }

    }


    void PlayerAttackWallAndWarrior()
    {
        if (ABoxes[0].tag == "busy")
        {
            Transform childGameObject = ABoxes[0].transform.GetChild(0);
            GameObject childTransform = childGameObject.gameObject;
            if (PlayerWallHP > 0)
            {
                if (childTransform.layer == LayerMask.NameToLayer("EnemyPlaying"))
                {
                    PlayerWallHP -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                    childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(1);
                    childTransform.GetComponent<CardInfoScr>().RefreshData();
                    if (PlayerWallHP < 10)
                    {
                        PlayerWall.SetActive(false);
                        PlayerWall2st.SetActive(true);

                    }
                    if (PlayerWallHP <= 0)
                    {
                        PlayerWall2st.SetActive(false);
                        PlayerWall3st.SetActive(true);
                    }
                    ShowHPWall();
                }
            }
            else if (PlayerWarriorHP1 > 0)
            {
                if (childTransform.layer == LayerMask.NameToLayer("EnemyPlaying"))
                {
                    PlayerWarriorHP1 -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                    if (PlayerWarriorHP2 > 0)
                    {
                        childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(8);
                    }
                    else
                    {
                        childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(4);

                    }
                    childTransform.GetComponent<CardInfoScr>().RefreshData();
                }
            }
            else if (PlayerWarriorHP2 > 0)
            {
                if (childTransform.layer == LayerMask.NameToLayer("EnemyPlaying"))
                {
                    PlayerWarriorHP2 -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                    childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(4);
                    childTransform.GetComponent<CardInfoScr>().RefreshData();
                }
            }
            else if (!IsPlayerTurn)
            {
                PlayerKhanHP -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                if (PlayerKhanHP <= 0)
                {
                    wonLostMenu.LostMenu();
                }
                childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(1);
                childTransform.GetComponent<CardInfoScr>().RefreshData();
            }
        }
        if (BBoxes[0].tag == "busy")
        {
            Transform childGameObject = BBoxes[0].transform.GetChild(0);
            GameObject childTransform = childGameObject.gameObject;
            if (PlayerWallHP > 0)
            {

                if (childTransform.layer == LayerMask.NameToLayer("EnemyPlaying"))
                {
                    PlayerWallHP -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                    childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(1);
                    childTransform.GetComponent<CardInfoScr>().RefreshData();
                }
                if (PlayerWallHP < 10)
                {
                    PlayerWall.SetActive(false);
                    PlayerWall2st.SetActive(true);

                }
                if (PlayerWallHP <= 0)
                {
                    PlayerWall2st.SetActive(false);
                    PlayerWall3st.SetActive(true);
                }
                ShowHPWall();
            }
            else if (PlayerWarriorHP1 > 0)
            {
                if (childTransform.layer == LayerMask.NameToLayer("EnemyPlaying"))
                {
                    PlayerWarriorHP1 -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                    if (PlayerWarriorHP2 > 0)
                    {
                        childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(8);
                    }
                    else
                    {
                        childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(4);

                    }
                    childTransform.GetComponent<CardInfoScr>().RefreshData();
                }
            }
            else if (PlayerWarriorHP2 > 0)
            {
                if (childTransform.layer == LayerMask.NameToLayer("EnemyPlaying"))
                {
                    PlayerWarriorHP2 -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                    childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(4);
                    childTransform.GetComponent<CardInfoScr>().RefreshData();
                }
            }
            else if (!IsPlayerTurn)
            {
                PlayerKhanHP -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                if (PlayerKhanHP <= 0)
                {
                    wonLostMenu.LostMenu();
                }
                childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(1);
                childTransform.GetComponent<CardInfoScr>().RefreshData();
            }
        }

        if (CBoxes[0].tag == "busy")
        {
            Transform childGameObject = CBoxes[0].transform.GetChild(0);
            GameObject childTransform = childGameObject.gameObject;
            if (PlayerWallHP > 0)
            {
                if (childTransform.layer == LayerMask.NameToLayer("EnemyPlaying"))
                {
                    PlayerWallHP -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                    childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(1);
                    childTransform.GetComponent<CardInfoScr>().RefreshData();
                }
                if (PlayerWallHP < 10)
                {
                    PlayerWall.SetActive(false);
                    PlayerWall2st.SetActive(true);

                }
                if (PlayerWallHP <= 0)
                {
                    PlayerWall2st.SetActive(false);
                    PlayerWall3st.SetActive(true);
                }
                ShowHPWall();
            }
            else if (PlayerWarriorHP2 > 0)
            {
                if (childTransform.layer == LayerMask.NameToLayer("EnemyPlaying"))
                {
                    PlayerWarriorHP2 -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                    if (PlayerWarriorHP1 > 0)
                    {
                        childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(8);
                    }
                    else
                    {
                        childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(4);

                    }
                    childTransform.GetComponent<CardInfoScr>().RefreshData();
                }
            }
            else if (PlayerWarriorHP1 > 0)
            {
                if (childTransform.layer == LayerMask.NameToLayer("EnemyPlaying"))
                {
                    PlayerWarriorHP1 -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                    childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(4);
                    childTransform.GetComponent<CardInfoScr>().RefreshData();
                }
            }
            else if (!IsPlayerTurn)
            {
                PlayerKhanHP -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                if (PlayerKhanHP <= 0)
                {
                    wonLostMenu.LostMenu();
                }
                childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(1);
                childTransform.GetComponent<CardInfoScr>().RefreshData();
            }
        }

        if (DBoxes[0].tag == "busy")
        {
            Transform childGameObject = DBoxes[0].transform.GetChild(0);
            GameObject childTransform = childGameObject.gameObject;
            if (PlayerWallHP > 0)
            {
                if (childTransform.layer == LayerMask.NameToLayer("EnemyPlaying"))
                {
                    PlayerWallHP -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                    childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(1);
                    childTransform.GetComponent<CardInfoScr>().RefreshData();
                }
                if (PlayerWallHP < 10)
                {
                    PlayerWall.SetActive(false);
                    PlayerWall2st.SetActive(true);

                }
                if (PlayerWallHP <= 0)
                {
                    PlayerWall2st.SetActive(false);
                    PlayerWall3st.SetActive(true);
                }

                ShowHPWall();
            }
            else if (PlayerWarriorHP2 > 0)
            {
                if (childTransform.layer == LayerMask.NameToLayer("EnemyPlaying"))
                {
                    PlayerWarriorHP2 -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                    if (PlayerWarriorHP2 > 0)
                    {
                        childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(8);
                    }
                    else
                    {
                        childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(4);

                    }
                    childTransform.GetComponent<CardInfoScr>().RefreshData();
                }
            }
            else if (PlayerWarriorHP1 > 0)
            {
                if (childTransform.layer == LayerMask.NameToLayer("EnemyPlaying"))
                {
                    PlayerWarriorHP1 -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                    childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(4);
                    childTransform.GetComponent<CardInfoScr>().RefreshData();
                }
            }
            else if (IsPlayerTurn)
            {
                PlayerKhanHP -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                if (PlayerKhanHP <= 0)
                {
                    wonLostMenu.LostMenu();
                }
                childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(1);
                childTransform.GetComponent<CardInfoScr>().RefreshData();
            }
        }
        if (PlayerWarriorHP1 <= 0)
        {
            DestroyImmediate(PlayerWarior1);
        }
        if (PlayerWarriorHP2 <= 0)
        {
            DestroyImmediate(PlayerWarior2);
        }
        ShowHPWarrior();
        ShowHPWall();
        ShowHPKhan();
    }
    void EnemyAttackWallAndWarrior()
    {
        //ABoxes
        if (ABoxes[3].tag == "busy")
        {
            Transform childGameObject = ABoxes[3].transform.GetChild(0);
            GameObject childTransform = childGameObject.gameObject;
            if (EnemyWallHP > 0)
            {
                if (childTransform.layer == LayerMask.NameToLayer("Playing"))
                {
                    EnemyWallHP -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                    childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(1);
                    childTransform.GetComponent<CardInfoScr>().RefreshData();
                    if (EnemyWallHP < 10)
                    {
                        EnemyWall.SetActive(false);
                        EnemyWall2st.SetActive(true);

                    }
                    if (EnemyWallHP <= 0)
                    {
                        EnemyWall2st.SetActive(false);
                        EnemyWall3st.SetActive(true);
                    }
                    ShowHPWall();

                }
            }
            else if (EnemyWarriorHP1 > 0)
            {

                if (childTransform.layer == LayerMask.NameToLayer("Playing"))
                {
                    EnemyWarriorHP1 -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                    if (EnemyWarriorHP2 > 0)
                    {
                        childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(8);
                    }
                    else
                    {
                        childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(4);

                    }
                    childTransform.GetComponent<CardInfoScr>().RefreshData();
                }
                if (EnemyWarriorHP1 <= 0)
                {
                    DestroyImmediate(EnemyWarrior1);
                }
            }
            else if (EnemyWarriorHP2 > 0)
            {
                if (childTransform.layer == LayerMask.NameToLayer("Playing"))
                {
                    EnemyWarriorHP2 -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                    childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(4);
                    childTransform.GetComponent<CardInfoScr>().RefreshData();
                }
                if (EnemyWarriorHP2 <= 0)
                {
                    DestroyImmediate(EnemyWarrior2);
                }
            }
            else if (IsPlayerTurn)
            {
                EnemyKhanHP -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                if (EnemyKhanHP <= 0)
                {
                    wonLostMenu.WonMenu();
                }
                childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(1);
                childTransform.GetComponent<CardInfoScr>().RefreshData();
            }
        }
        //BBoxes
        if (BBoxes[3].tag == "busy")
        {
            Transform childGameObject = BBoxes[3].transform.GetChild(0);
            GameObject childTransform = childGameObject.gameObject;
            if (EnemyWallHP > 0)
            {

                if (childTransform.layer == LayerMask.NameToLayer("Playing"))
                {
                    EnemyWallHP -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                    childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(1);
                    childTransform.GetComponent<CardInfoScr>().RefreshData();
                }
                if (EnemyWallHP < 10)
                {
                    EnemyWall.SetActive(false);
                    EnemyWall2st.SetActive(true);

                }
                if (EnemyWallHP <= 0)
                {
                    EnemyWall2st.SetActive(false);
                    EnemyWall3st.SetActive(true);
                }
                ShowHPWall();
            }
            else if (EnemyWarriorHP1 > 0)
            {

                if (childTransform.layer == LayerMask.NameToLayer("Playing"))
                {
                    EnemyWarriorHP1 -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                    if (EnemyWarriorHP2 > 0)
                    {
                        childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(8);
                    }
                    else
                    {
                        childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(4);

                    }
                    childTransform.GetComponent<CardInfoScr>().RefreshData();
                }
                if (EnemyWarriorHP1 <= 0)
                {
                    DestroyImmediate(EnemyWarrior1);
                }
            }
            else if (EnemyWarriorHP2 > 0)
            {

                if (childTransform.layer == LayerMask.NameToLayer("Playing"))
                {
                    EnemyWarriorHP2 -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                    childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(4);
                    childTransform.GetComponent<CardInfoScr>().RefreshData();
                }
                if (EnemyWarriorHP2 <= 0)
                {
                    DestroyImmediate(EnemyWarrior2);
                }
            }
            else if (IsPlayerTurn)
            {
                EnemyKhanHP -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                if (EnemyKhanHP <= 0)
                {
                    wonLostMenu.WonMenu();
                }
                childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(1);
                childTransform.GetComponent<CardInfoScr>().RefreshData();
            }
        }
        //CBoxes
        if (CBoxes[3].tag == "busy")
        {
            Transform childGameObject = CBoxes[3].transform.GetChild(0);
            GameObject childTransform = childGameObject.gameObject;
            if (EnemyWallHP > 0)
            {
                if (childTransform.layer == LayerMask.NameToLayer("Playing"))
                {
                    EnemyWallHP -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                    childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(1);
                    childTransform.GetComponent<CardInfoScr>().RefreshData();
                }
                if (EnemyWallHP < 10)
                {
                    EnemyWall.SetActive(false);
                    EnemyWall2st.SetActive(true);

                }
                if (EnemyWallHP <= 0)
                {
                    EnemyWall2st.SetActive(false);
                    EnemyWall3st.SetActive(true);
                }
                ShowHPWall();
            }
            else if (EnemyWarriorHP2 > 0)
            {

                if (childTransform.layer == LayerMask.NameToLayer("Playing"))
                {
                    EnemyWarriorHP2 -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                    if (EnemyWarriorHP1 > 0)
                    {
                        childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(8);
                    }
                    else
                    {
                        childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(4);

                    }
                    childTransform.GetComponent<CardInfoScr>().RefreshData();
                }
                if (EnemyWarriorHP2 <= 0)
                {
                    DestroyImmediate(EnemyWarrior2);
                }
            }
            else if (EnemyWarriorHP1 > 0)
            {
                if (childTransform.layer == LayerMask.NameToLayer("Playing"))
                {
                    EnemyWarriorHP1 -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                    childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(4);
                    childTransform.GetComponent<CardInfoScr>().RefreshData();
                }
                if (EnemyWarriorHP1 <= 0)
                {
                    DestroyImmediate(EnemyWarrior1);
                }
            }
            else if (IsPlayerTurn)
            {
                EnemyKhanHP -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                if (EnemyKhanHP <= 0)
                {
                    wonLostMenu.WonMenu();
                }
                childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(1);
                childTransform.GetComponent<CardInfoScr>().RefreshData();
            }
        }
        //DBoxes
        if (DBoxes[3].tag == "busy")
        {
            Transform childGameObject = DBoxes[3].transform.GetChild(0);
            GameObject childTransform = childGameObject.gameObject;
            if (EnemyWallHP > 0)
            {

                if (childTransform.layer == LayerMask.NameToLayer("Playing"))
                {
                    EnemyWallHP -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                    childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(1);
                    childTransform.GetComponent<CardInfoScr>().RefreshData();
                }
                if (EnemyWallHP < 10)
                {
                    EnemyWall.SetActive(false);
                    EnemyWall2st.SetActive(true);

                }
                if (EnemyWallHP <= 0)
                {
                    EnemyWall2st.SetActive(false);
                    EnemyWall3st.SetActive(true);
                }
                ShowHPWall();
            }
            else if (EnemyWarriorHP2 > 0)
            {


                if (childTransform.layer == LayerMask.NameToLayer("Playing"))
                {
                    EnemyWarriorHP2 -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                    if (EnemyWarriorHP2 > 0)
                    {
                        childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(8);
                    }
                    else
                    {
                        childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(4);

                    }
                    childTransform.GetComponent<CardInfoScr>().RefreshData();
                }
                if (EnemyWarriorHP2 <= 0)
                {
                    DestroyImmediate(EnemyWarrior2);
                }
            }
            else if (EnemyWarriorHP1 > 0)
            {

                if (childTransform.layer == LayerMask.NameToLayer("Playing"))
                {
                    EnemyWarriorHP1 -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                    childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(4);
                    childTransform.GetComponent<CardInfoScr>().RefreshData();
                }
                if (EnemyWarriorHP1 <= 0)
                {
                    DestroyImmediate(EnemyWarrior1);
                }
            }
            else if (IsPlayerTurn)
            {
                EnemyKhanHP -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                if (EnemyKhanHP <= 0)
                {
                    wonLostMenu.WonMenu();
                }
                childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(1);
                childTransform.GetComponent<CardInfoScr>().RefreshData();
            }
        }
        ShowHPKhan();
        ShowHPWarrior();
        ShowHPWall();
    }



    public void EnemyJutSpawn(GameObject posToSpell)
    {
        Debug.Log(posToSpell.name);
        if (posToSpell.name == "A")
            jutSpellAnimation.Play("D");
        if (posToSpell.name == "B")
            jutSpellAnimation.Play("C");
        if (posToSpell.name == "C")
            jutSpellAnimation.Play("B");
        if (posToSpell.name == "D")
            jutSpellAnimation.Play("A");
    }
    public void EnemyArrowsSpawn(GameObject posToSpell)
    {
        Debug.Log(posToSpell.name);
        if (posToSpell.name == "A1")
            arrowsSpellAnimation.Play("A4");
        if (posToSpell.name == "A2")
            arrowsSpellAnimation.Play("A3");
        if (posToSpell.name == "A3")
            arrowsSpellAnimation.Play("A2");
        if (posToSpell.name == "A4")
            arrowsSpellAnimation.Play("A1");

        if (posToSpell.name == "B1")
            arrowsSpellAnimation.Play("B4");
        if (posToSpell.name == "B2")
            arrowsSpellAnimation.Play("B3");
        if (posToSpell.name == "B3")
            arrowsSpellAnimation.Play("B2");
        if (posToSpell.name == "B4")
            arrowsSpellAnimation.Play("B1");

        if (posToSpell.name == "C1")
            arrowsSpellAnimation.Play("C4");
        if (posToSpell.name == "C2")
            arrowsSpellAnimation.Play("C3");
        if (posToSpell.name == "C3")
            arrowsSpellAnimation.Play("C2");
        if (posToSpell.name == "C4")
            arrowsSpellAnimation.Play("C1");

        if (posToSpell.name == "D1")
            arrowsSpellAnimation.Play("D4");
        if (posToSpell.name == "D2")
            arrowsSpellAnimation.Play("D3");
        if (posToSpell.name == "D3")
            arrowsSpellAnimation.Play("D2");
        if (posToSpell.name == "D4")
            arrowsSpellAnimation.Play("D1");
    }
    public void EnemyCardModelSpawn(Vector3 selPos, GameObject selectedObject)
    {
        GameObject prefab = Models[selectedObject.GetComponent<CardInfoScr>().SelfCard.Id];
        //if (selectedObject.GetComponent<CardInfoScr>().SelfCard.Prefab == null)
        //{
        //    Debug.Log("Error");
        //    return;
        //}
        instantiatedPrefab = Instantiate(prefab, selPos, Quaternion.identity);
        if (selectedObject.GetComponent<CardInfoScr>().SelfCard.Id == 2)
        {
            instantiatedPrefab.transform.rotation = Quaternion.Euler(new Vector3(instantiatedPrefab.transform.rotation.x, instantiatedPrefab.transform.rotation.y + 180f, instantiatedPrefab.transform.rotation.z));
        }
        else if (selectedObject.GetComponent<CardInfoScr>().SelfCard.Id == 3 || selectedObject.GetComponent<CardInfoScr>().SelfCard.Id == 4)
        {
            instantiatedPrefab.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        }
        newRotation = Quaternion.Euler(instantiatedPrefab.transform.eulerAngles.x, instantiatedPrefab.transform.eulerAngles.y + 180f, instantiatedPrefab.transform.eulerAngles.z);
        instantiatedPrefab.transform.rotation = newRotation;
        Animator anim = instantiatedPrefab.GetComponent<Animator>();
        anim.Play("SpawnAnimationTest");
    }
    public void ShowMana()
    {
        PlayerManaTxt.text = PlayerMana.ToString();
        EnemyManaTxt.text = EnemyMana.ToString();
    }
    void ShowHPWall()
    {
        if (PlayerWallHP <= 0)
        {
            PlayerWallHPTxt.text = "0";
        }
        else
        {
            PlayerWallHPTxt.text = PlayerWallHP.ToString();
        }
        if (EnemyWallHP <= 0)
        {
            EnemyWallHPTxt.text = "0";
        }
        else
        {
            EnemyWallHPTxt.text = EnemyWallHP.ToString();
        }
    }
    void ShowHPWarrior()
    {
        if (PlayerWarriorHP1 <= 0)
        {
            PlayerWarriorHP1Txt.text = "0";
        }
        else
        {
            PlayerWarriorHP1Txt.text = PlayerWarriorHP1.ToString();
        }
        if (PlayerWarriorHP2 <= 0)
        {
            PlayerWarriorHP2Txt.text = "0";
        }
        else
        {
            PlayerWarriorHP2Txt.text = PlayerWarriorHP2.ToString();
        }
        if (EnemyWarriorHP1 <= 0)
        {
            EnemyWarriorHP1Txt.text = "0";
        }
        else
        {
            EnemyWarriorHP1Txt.text = EnemyWarriorHP1.ToString();
        }
        if (EnemyWarriorHP2 <= 0)
        {
            EnemyWarriorHP2Txt.text = "0";
        }
        else
        {
            EnemyWarriorHP2Txt.text = EnemyWarriorHP2.ToString();
        }
    }
    void ShowHPKhan()
    {
        if (PlayerKhanHP <= 0)
        {
            PlayerKhanHPTxt.text = "0";
        }
        else
        {
            PlayerKhanHPTxt.text = PlayerKhanHP.ToString();
        }
        if (EnemyKhanHP <= 0)
        {
            EnemyKhanHPTxt.text = "0";
        }
        else
        {
            EnemyKhanHPTxt.text = EnemyKhanHP.ToString();
        }
    }

    public void BaffUnits(string unit)
    {
        if (unit == "Barak")
        {
            WarriorBaff++;
            for (int i = 0; i < 16; i++)
            {
                if (AllBoxes[i].tag == "busy")
                {
                    Transform childGameObject = AllBoxes[i].transform.GetChild(0);
                    GameObject childTransform = childGameObject.gameObject;
                    if (childTransform.GetComponent<CardInfoScr>().SelfCard.Name == "Warrior" && (childTransform.layer == LayerMask.NameToLayer("Playing") || childTransform.layer == LayerMask.NameToLayer("Played")))
                    {
                        childTransform.GetComponent<CardInfoScr>().SelfCard.SetBaff(1);
                        childTransform.GetComponent<CardInfoScr>().RefreshData();
                    }
                }
            }
            if (PlayerHand.childCount > 0)
            {
                for (int i = 0; i < PlayerHand.childCount; i++)
                {
                    Transform childGameObject = PlayerHand.transform.GetChild(i);
                    GameObject childTransform = childGameObject.gameObject;
                    if (childTransform.GetComponent<CardInfoScr>().SelfCard.Name == "Warrior")
                    {
                        childTransform.GetComponent<CardInfoScr>().SelfCard.SetBaff(1);
                        childTransform.GetComponent<CardInfoScr>().RefreshData();
                    }
                }
            }
        }
        else if (unit == "Bowrange")
        {
            ArcheryBaff++;
            for (int i = 0; i < 16; i++)
            {
                if (AllBoxes[i].tag == "busy")
                {
                    Transform childGameObject = AllBoxes[i].transform.GetChild(0);
                    GameObject childTransform = childGameObject.gameObject;
                    if (childTransform.GetComponent<CardInfoScr>().SelfCard.Name == "Archer" && (childTransform.layer == LayerMask.NameToLayer("Playing") || childTransform.layer == LayerMask.NameToLayer("Played")))
                    {
                        childTransform.GetComponent<CardInfoScr>().SelfCard.SetBaff(1);
                        childTransform.GetComponent<CardInfoScr>().RefreshData();
                    }
                }
            }
            if (PlayerHand.childCount > 0)
            {
                for (int i = 0; i < PlayerHand.childCount; i++)
                {
                    Transform childGameObject = PlayerHand.transform.GetChild(i);
                    GameObject childTransform = childGameObject.gameObject;
                    if (childTransform.GetComponent<CardInfoScr>().SelfCard.Name == "Archer")
                    {
                        childTransform.GetComponent<CardInfoScr>().SelfCard.SetBaff(1);
                        childTransform.GetComponent<CardInfoScr>().RefreshData();
                    }
                }
            }
        }
    }

    public void BaffAbillity()
    {
        for (int i = 0; i < 16; i++)
        {
            if (AllBoxes[i].tag == "busy")
            {
                Transform childGameObject = AllBoxes[i].transform.GetChild(0);
                GameObject childTransform = childGameObject.gameObject;
                if (childTransform.GetComponent<CardInfoScr>().SelfCard.Name == "Ensign")
                {
                    continue;
                }
                else if (childTransform.layer == LayerMask.NameToLayer("Played") || childTransform.layer == LayerMask.NameToLayer("Playing"))
                {
                    childTransform.GetComponent<CardInfoScr>().SelfCard.SetBaff(1);
                    childTransform.GetComponent<CardInfoScr>().RefreshData();

                }
            }
        }
    }
}
