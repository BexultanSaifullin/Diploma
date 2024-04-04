using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

//Debug.Log(count);


public class GameManagerScr : MonoBehaviour
{
    int Turn, TurnTime = 30;
    public TextMeshProUGUI TurnTimeTxt;
    public Button EndTurnBtn;
    CardSpawnerScr Spawner;
    CardSpawnerEnemyScr SpawnerEnemy;
    CardInfoScr CardInfo;
    public GameObject[] ABoxes;
    public GameObject[] BBoxes;
    public GameObject[] CBoxes;
    public GameObject[] DBoxes;
    public int WallHP = 10, EnemyWallHP = 10;  
   
    public int PlayerMana = 1, EnemyMana = 1;
    int increase = 1;
    public TextMeshProUGUI PlayerManaTxt, EnemyManaTxt;
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
        }
        else
        {
            while (TurnTime-- > 27)
            {
                TurnTimeTxt.text = TurnTime.ToString();
                yield return new WaitForSeconds(1);
            }

            List<GameObject> EnemyPlaces;

            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("free");


            int EnemyLayer = LayerMask.NameToLayer("EnemyPlace");
            GameObject[] objectsOnLayer = objectsWithTag.Where(card => card.layer == EnemyLayer).ToArray();


            EnemyPlaces = objectsOnLayer.ToList();
            List<GameObject> EnemyCard;

            GameObject[] objectsWithTagE = GameObject.FindGameObjectsWithTag("EnemyCard");


            int EnemyLayerE = LayerMask.NameToLayer("Enemy");
            GameObject[] objectsOnLayerE = objectsWithTagE.Where(card => card.layer == EnemyLayerE).ToArray();


            EnemyCard = objectsOnLayerE.ToList();
            //List<GameObject> EnemyCard = GameObject.FindGameObjectsWithTag("EnemyCard").ToList();

            if (EnemyCard.Count > 0 && EnemyPlaces.Count > 0)
            {
                EnemyTurn(EnemyCard, EnemyPlaces);
            }
        }
        TurnTime = 5;
        while (TurnTime-- > 0)
        {
            TurnTimeTxt.text = TurnTime.ToString();
            yield return new WaitForSeconds(1);
        }
        ChangeTurn();
    }

    void EnemyTurn(List<GameObject> EnemyCard, List<GameObject> EnemyPlaces)
    {
        if (EnemyPlaces.Count > EnemyCard.Count)
        {
            int count = Random.Range(-1, EnemyCard.Count);
            if (count == -1)
                return;

            for (int i = count; i >= 0; i--)
            {
                int place = Random.Range(0, EnemyPlaces.Count - 1);
                Vector3 newPosition = EnemyPlaces[place].transform.position;
                newPosition.y += 0.2f;
                if (EnemyCard[i].GetComponent<CardInfoScr>().SelfCard.Mana > EnemyMana)
                {
                    continue;
                }
                EnemyCard[i].transform.position = newPosition;
                Vector3 rotationAngles = new Vector3(-90f, 0f, 0f);
                EnemyCard[i].transform.rotation = Quaternion.Euler(rotationAngles);
                EnemyCard[i].layer = LayerMask.NameToLayer("EnemyPlaying");
                EnemyPlaces[place].gameObject.tag = "busy";
                EnemyCard[i].transform.parent = EnemyPlaces[place].transform;
                EnemyMana -= EnemyCard[i].GetComponent<CardInfoScr>().SelfCard.Mana;
                ShowMana();
                EnemyPlaces.RemoveAt(place);
                EnemyCard.RemoveAt(i);
            }
        }
        else
        {
            int count = Random.Range(-1, EnemyPlaces.Count);
            if (count == -1)
                return;
            for (int i = count; i >= 0; i--)
            {
                if (EnemyCard[i].GetComponent<CardInfoScr>().SelfCard.Mana > EnemyMana)
                {
                    continue;
                }
                int place = Random.Range(0, EnemyPlaces.Count - 1);
                Vector3 newPosition = EnemyPlaces[place].transform.position;
                newPosition.y += 0.2f;
                EnemyCard[i].transform.position = newPosition;
                Vector3 rotationAngles = new Vector3(-90f, 0f, 0f);
                EnemyCard[i].transform.rotation = Quaternion.Euler(rotationAngles);
                EnemyCard[i].layer = LayerMask.NameToLayer("EnemyPlaying");
                //CardInfo.ChangeInfo(EnemyCard[i]);
                EnemyPlaces[place].gameObject.tag = "busy";
                EnemyMana -= EnemyCard[i].GetComponent<CardInfoScr>().SelfCard.Mana;
                ShowMana();
                EnemyPlaces.RemoveAt(place);
                EnemyCard.RemoveAt(i);
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
            AttackCards();
            DestroyCards();
            MoveCards();
            GivenNewCards();
            if (increase < 10)
                increase += 1;
            if (PlayerMana < 10)
                PlayerMana = 10;
            if (EnemyMana < 10)
                EnemyMana = 10;
            ShowMana();
        }
        else if (Turn != 1)
        {
            GivenNewCardsToEnemy();
        }
        StartCoroutine(TurnFunc());
    }

    public void ShowMana()
    {
        PlayerManaTxt.text = PlayerMana.ToString();
        EnemyManaTxt.text = EnemyMana.ToString();
    }
    void GivenNewCards()
    {
        Spawner.Spawn();
    }
    void GivenNewCardsToEnemy()
    {
        SpawnerEnemy.SpawnEnemy();
    }

    void MoveCards()
    {
            if (ABoxes[2].tag == "busy")
            {
                if (ABoxes[3].tag == "free")
                {
                    Transform childGameObject = ABoxes[2].transform.GetChild(0);
                    GameObject childTransform = childGameObject.gameObject;

                    if (childTransform.layer == LayerMask.NameToLayer("Playing"))
                    {
                        Vector3 newPositiona = ABoxes[3].transform.position;
                        newPositiona.y += 0.2f;
                        childTransform.transform.position = newPositiona;
                        childTransform.transform.parent = ABoxes[3].transform;
                        ABoxes[3].tag = "busy";
                        ABoxes[2].tag = "free";
                    }
                }
            }

            if (BBoxes[2].tag == "busy")
            {
                if (BBoxes[3].tag == "free")
                {
                    Transform childGameObject = BBoxes[2].transform.GetChild(0);
                    GameObject childTransform = childGameObject.gameObject;

                    if (childTransform.layer == LayerMask.NameToLayer("Playing"))
                    {
                        Vector3 newPositiona = BBoxes[3].transform.position;
                        newPositiona.y += 0.2f;
                        childTransform.transform.position = newPositiona;
                        childTransform.transform.parent = BBoxes[3].transform;
                        BBoxes[3].tag = "busy";
                        BBoxes[2].tag = "free";
                    }
                }
            }

            if (CBoxes[2].tag == "busy")
            {
                if (CBoxes[3].tag == "free")
                {
                    Transform childGameObject = CBoxes[2].transform.GetChild(0);
                    GameObject childTransform = childGameObject.gameObject;

                    if (childTransform.layer == LayerMask.NameToLayer("Playing"))
                    {
                        Vector3 newPositiona = CBoxes[3].transform.position;
                        newPositiona.y += 0.2f;
                        childTransform.transform.position = newPositiona;
                        childTransform.transform.parent = CBoxes[3].transform;
                        CBoxes[3].tag = "busy";
                        CBoxes[2].tag = "free";
                    }
                }
            }

            if (DBoxes[2].tag == "busy")
            {
                if (DBoxes[3].tag == "free")
                {
                    Transform childGameObject = DBoxes[2].transform.GetChild(0);
                    GameObject childTransform = childGameObject.gameObject;

                    if (childTransform.layer == LayerMask.NameToLayer("Playing"))
                    {
                        Vector3 newPositiona = DBoxes[3].transform.position;
                        newPositiona.y += 0.2f;
                        childTransform.transform.position = newPositiona;
                        childTransform.transform.parent = DBoxes[3].transform;
                        DBoxes[3].tag = "busy";
                        DBoxes[2].tag = "free";
                    }
                }
            }

        
            if (ABoxes[1].tag == "busy" && ABoxes[3].tag == "free")
            {
                if (ABoxes[2].tag == "free")
                {
                    Transform childGameObject = ABoxes[1].transform.GetChild(0);
                    GameObject childTransform = childGameObject.gameObject;

                    if (childTransform.layer == LayerMask.NameToLayer("Playing"))
                    {
                        Vector3 newPositiona = ABoxes[2].transform.position;
                        newPositiona.y += 0.2f;
                        childTransform.transform.position = newPositiona;
                        childTransform.transform.parent = ABoxes[2].transform;
                        ABoxes[2].tag = "busy";
                        ABoxes[1].tag = "free";
                    }
                }
            }

            if (BBoxes[1].tag == "busy" && BBoxes[3].tag == "free")
            {
                if (BBoxes[2].tag == "free")
                {
                    Transform childGameObject = BBoxes[1].transform.GetChild(0);
                    GameObject childTransform = childGameObject.gameObject;

                    if (childTransform.layer == LayerMask.NameToLayer("Playing"))
                    {
                        Vector3 newPositiona = BBoxes[2].transform.position;
                        newPositiona.y += 0.2f;
                        childTransform.transform.position = newPositiona;
                        childTransform.transform.parent = BBoxes[2].transform;
                        BBoxes[2].tag = "busy";
                        BBoxes[1].tag = "free";
                    }
                }
            }

            if (CBoxes[1].tag == "busy" && CBoxes[3].tag == "free")
            {
                if (CBoxes[2].tag == "free")
                {
                    Transform childGameObject = CBoxes[1].transform.GetChild(0);
                    GameObject childTransform = childGameObject.gameObject;

                    if (childTransform.layer == LayerMask.NameToLayer("Playing"))
                    {
                        Vector3 newPositiona = CBoxes[2].transform.position;
                        newPositiona.y += 0.2f;
                        childTransform.transform.position = newPositiona;
                        childTransform.transform.parent = CBoxes[2].transform;
                        CBoxes[2].tag = "busy";
                        CBoxes[1].tag = "free";
                    }
                }
            }

            if (DBoxes[1].tag == "busy" && DBoxes[3].tag == "free")
            {
                if (DBoxes[2].tag == "free")
                {
                    Transform childGameObject = DBoxes[1].transform.GetChild(0);
                    GameObject childTransform = childGameObject.gameObject;

                    if (childTransform.layer == LayerMask.NameToLayer("Playing"))
                    {
                        Vector3 newPositiona = DBoxes[2].transform.position;
                        newPositiona.y += 0.2f;
                        childTransform.transform.position = newPositiona;
                        childTransform.transform.parent = DBoxes[2].transform;
                        DBoxes[2].tag = "busy";
                        DBoxes[1].tag = "free";
                    }
                }
            }
        
        

            if (ABoxes[0].tag == "busy")
            {
                if (ABoxes[1].tag == "free")
                {
                    Transform childGameObject = ABoxes[0].transform.GetChild(0);
                    GameObject childTransform = childGameObject.gameObject;

                    if (childTransform.layer == LayerMask.NameToLayer("Playing"))
                    {
                        Vector3 newPositiona = ABoxes[1].transform.position;
                        newPositiona.y += 0.2f;
                        childTransform.transform.position = newPositiona;
                        childTransform.transform.parent = ABoxes[1].transform;
                        ABoxes[1].tag = "busy";
                        ABoxes[0].tag = "free";
                        
                    }
                }
            }

            if (BBoxes[0].tag == "busy")
            {
                if (BBoxes[1].tag == "free")
                {
                    Transform childGameObject = BBoxes[0].transform.GetChild(0);
                    GameObject childTransform = childGameObject.gameObject;

                    if (childTransform.layer == LayerMask.NameToLayer("Playing"))
                    {
                        Vector3 newPositiona = BBoxes[1].transform.position;
                        newPositiona.y += 0.2f;
                        childTransform.transform.position = newPositiona;
                        childTransform.transform.parent = BBoxes[1].transform;
                        BBoxes[1].tag = "busy";
                        BBoxes[0].tag = "free";
                    
                }
                }
            }

            if (CBoxes[0].tag == "busy")
            {
                if (CBoxes[1].tag == "free")
                {
                    Transform childGameObject = CBoxes[0].transform.GetChild(0);
                    GameObject childTransform = childGameObject.gameObject;

                    if (childTransform.layer == LayerMask.NameToLayer("Playing"))
                    {
                        Vector3 newPositiona = CBoxes[1].transform.position;
                        newPositiona.y += 0.2f;
                        childTransform.transform.position = newPositiona;
                        childTransform.transform.parent = CBoxes[1].transform;
                        CBoxes[1].tag = "busy";
                        CBoxes[0].tag = "free";
                        
                    }
                }
            }

            if (DBoxes[0].tag == "busy")
            {
                if (DBoxes[1].tag == "free")
                {
                    Transform childGameObject = DBoxes[0].transform.GetChild(0);
                    GameObject childTransform = childGameObject.gameObject;

                    if (childTransform.layer == LayerMask.NameToLayer("Playing"))
                    {
                        Vector3 newPositiona = DBoxes[1].transform.position;
                        newPositiona.y += 0.2f;
                        childTransform.transform.position = newPositiona;
                        childTransform.transform.parent = DBoxes[1].transform;
                        DBoxes[1].tag = "busy";
                        DBoxes[0].tag = "free";
                    
                }
                }
            }
        
        for (int i = 1; i < 4; i++)//Enemy move
        {
            if (ABoxes[i].tag == "busy")
            {
                if (ABoxes[i - 1].tag == "free")
                {
                    Transform childGameObject = ABoxes[i].transform.GetChild(0);
                    GameObject childTransform = childGameObject.gameObject;

                    if (childTransform.layer == LayerMask.NameToLayer("EnemyPlaying"))
                    {
                        Vector3 newPositiona = ABoxes[i - 1].transform.position;
                        newPositiona.y += 0.2f;
                        childTransform.transform.position = newPositiona;
                        childTransform.transform.parent = ABoxes[i - 1].transform;
                        ABoxes[i - 1].tag = "busy";
                        ABoxes[i].tag = "free";
                        
                    }
                }
            }
            if (BBoxes[i].tag == "busy")
            {
                if (BBoxes[i - 1].tag == "free")
                {
                    Transform childGameObject = BBoxes[i].transform.GetChild(0);
                    GameObject childTransform = childGameObject.gameObject;

                    if (childTransform.layer == LayerMask.NameToLayer("EnemyPlaying"))
                    {
                        Vector3 newPositiona = BBoxes[i - 1].transform.position;
                        newPositiona.y += 0.2f;
                        childTransform.transform.position = newPositiona;
                        childTransform.transform.parent = BBoxes[i - 1].transform;
                        BBoxes[i - 1].tag = "busy";
                        BBoxes[i].tag = "free";
                    }
                }
            }
            if (CBoxes[i].tag == "busy")
            {
                if (CBoxes[i - 1].tag == "free")
                {
                    Transform childGameObject = CBoxes[i].transform.GetChild(0);
                    GameObject childTransform = childGameObject.gameObject;

                    if (childTransform.layer == LayerMask.NameToLayer("EnemyPlaying"))
                    {
                        Vector3 newPositiona = CBoxes[i - 1].transform.position;
                        newPositiona.y += 0.2f;
                        childTransform.transform.position = newPositiona;
                        childTransform.transform.parent = CBoxes[i - 1].transform;
                        CBoxes[i - 1].tag = "busy";
                        CBoxes[i].tag = "free";
                    }
                }
            }
            if (DBoxes[i].tag == "busy")
            {
                if (DBoxes[i - 1].tag == "free")
                {
                    Transform childGameObject = DBoxes[i].transform.GetChild(0);
                    GameObject childTransform = childGameObject.gameObject;

                    if (childTransform.layer == LayerMask.NameToLayer("EnemyPlaying"))
                    {
                        Vector3 newPositiona = DBoxes[i - 1].transform.position;
                        newPositiona.y += 0.2f;
                        childTransform.transform.position = newPositiona;
                        childTransform.transform.parent = DBoxes[i - 1].transform;
                        DBoxes[i - 1].tag = "busy";
                        DBoxes[i].tag = "free";
                    }
                }
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
                int range = childTransform.GetComponent<CardInfoScr>().SelfCard.Range;
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
                        }

                    }
                }
            }

            if (BBoxes[i].transform.childCount > 0)
            {
                Transform childGameObject = BBoxes[i].transform.GetChild(0);
                GameObject childTransform = childGameObject.gameObject;
                int range = childTransform.GetComponent<CardInfoScr>().SelfCard.Range;
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
                        }

                    }
                }
            }

            if (CBoxes[i].transform.childCount > 0)
            {
                Transform childGameObject = CBoxes[i].transform.GetChild(0);
                GameObject childTransform = childGameObject.gameObject;
                int range = childTransform.GetComponent<CardInfoScr>().SelfCard.Range;
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
                        }

                    }
                }

            }

            if (DBoxes[i].transform.childCount > 0)
            {
                Transform childGameObject = DBoxes[i].transform.GetChild(0);
                GameObject childTransform = childGameObject.gameObject;
                int range = childTransform.GetComponent<CardInfoScr>().SelfCard.Range;
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
                        }

                    }
                }
            }
        }

        for (int i = 1; i < 4; i++)
        {
            if (ABoxes[i].transform.childCount > 0)
            {
                Transform childGameObject = ABoxes[i].transform.GetChild(0);
                GameObject childTransform = childGameObject.gameObject;
                int range = childTransform.GetComponent<CardInfoScr>().SelfCard.Range;
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
                        }
                    }
                }
            }



            if (BBoxes[i].transform.childCount > 0)
            {
                Transform childGameObject = BBoxes[i].transform.GetChild(0);
                GameObject childTransform = childGameObject.gameObject;
                int range = childTransform.GetComponent<CardInfoScr>().SelfCard.Range;
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
                        }
                    }
                }
            }




            if (CBoxes[i].transform.childCount > 0)
            {
                Transform childGameObject = CBoxes[i].transform.GetChild(0);
                GameObject childTransform = childGameObject.gameObject;
                int range = childTransform.GetComponent<CardInfoScr>().SelfCard.Range;
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
                        }
                    }
                }
            }




            if (DBoxes[i].transform.childCount > 0)
            {
                Transform childGameObject = DBoxes[i].transform.GetChild(0);
                GameObject childTransform = childGameObject.gameObject;
                int range = childTransform.GetComponent<CardInfoScr>().SelfCard.Range;
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
                        }
                    }
                }
            }
        }
    }

    void DestroyCards()
    {
        for (int i = 0; i < 4; i++)
        {
            if (ABoxes[i].transform.childCount > 0)
            {
                Transform childGameObject = ABoxes[i].transform.GetChild(0);
                GameObject childTransform = childGameObject.gameObject;
                if (childTransform.GetComponent<CardInfoScr>().SelfCard.Defense <= 0)
                {
                    DestroyImmediate(childTransform);
                    ABoxes[i].tag = "free";
                    
                }
            }
            if (BBoxes[i].transform.childCount > 0)
            {
                Transform childGameObject = BBoxes[i].transform.GetChild(0);
                GameObject childTransform = childGameObject.gameObject;
                if (childTransform.GetComponent<CardInfoScr>().SelfCard.Defense <= 0)
                {
                    DestroyImmediate(childTransform);
                    BBoxes[i].tag = "free";
                    
                }
            }
            if (CBoxes[i].transform.childCount > 0)
            {
                Transform childGameObject = CBoxes[i].transform.GetChild(0);
                GameObject childTransform = childGameObject.gameObject;
                if (childTransform.GetComponent<CardInfoScr>().SelfCard.Defense <= 0)
                {
                    DestroyImmediate(childTransform);
                    CBoxes[i].tag = "free";
                    
                }
            }
            if (DBoxes[i].transform.childCount > 0)
            {
                Transform childGameObject = DBoxes[i].transform.GetChild(0);
                GameObject childTransform = childGameObject.gameObject;
                if (childTransform.GetComponent<CardInfoScr>().SelfCard.Defense <= 0)
                {
                    DestroyImmediate(childTransform);

                    DBoxes[i].tag = "free";
                    
                }
            }
        }
    }


    void AttackWall()
    {

    }
}
