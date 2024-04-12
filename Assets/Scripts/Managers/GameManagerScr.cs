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
    public Button EndTurnBtn;
    CardSpawnerScr Spawner; CardSpawnerEnemyScr SpawnerEnemy;

    public GameObject PlayerWall, EnemyWall,
           PlayerWarior1, PlayerWarior2, EnemyWarrior1, EnemyWarrior2,
                PlayerKhan, EnemyKhan;

    public GameObject[] ABoxes, BBoxes, CBoxes, DBoxes, PlayerBuildingsBoxes, EnemyBuildingsBoxes;

    public int PlayerWallHP = 20, EnemyWallHP = 20, WallDMG = 1;

    public int PlayerWarriorHP1 = 6, EnemyWarriorHP1 = 6, PlayerWarriorHP2 = 6, EnemyWarriorHP2 = 6,
                    PlayerKhanHP = 10, EnemyKhanHP = 10;

    public int PlayerMana = 1, EnemyMana = 1;

    public TextMeshProUGUI PlayerManaTxt, EnemyManaTxt,
        PlayerWallHPTxt, EnemyWallHPTxt,
            PlayerWarriorHP1Txt, PlayerWarriorHP2Txt, EnemyWarriorHP1Txt, EnemyWarriorHP2Txt,
                PlayerKhanHPTxt, EnemyKhanHPTxt,
                    TurnTimeTxt;

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

            //List<GameObject> EnemyCardBuildings = new List<GameObject>();

            //for(int i = EnemyCard.Count - 1; i >= 0; i--)
            //{ 
            //    if (EnemyCard[i].GetComponent<CardInfoScr>().SelfCard.Type == "Building")
            //    {
            //        EnemyCardBuildings.Add(EnemyCard[i]); // Добавляем элемент непосредственно в список зданий
            //        EnemyCard.RemoveAt(i); // Безопасно удаляем элемент из первоначального списка
            //    }
            //}


            if (EnemyCard.Count > 0 && EnemyPlaces.Count > 0)
            {
                EnemyTurn(EnemyCard, EnemyPlaces);
            }
        }
        ChangeTurn();
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
            Spawner.Spawn();
            if (increase < 10)
                increase += 1;
            if (PlayerMana < 10)
                PlayerMana = 10;
            if (EnemyMana < 10)
                EnemyMana = 100;
            ShowMana();
        }
        else if (Turn != 1)
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
            SpawnerEnemy.SpawnEnemy();
        }
        StartCoroutine(TurnFunc());
    }

    void EnemyTurn(List<GameObject> EnemyCard, List<GameObject> EnemyPlaces)
    {

        if (EnemyPlaces.Count >= EnemyCard.Count)
        {
            int count = Random.Range(-1, EnemyCard.Count);
            if (count == -1)
                return;
            for (int i = count; i >= 0; i--)
            {
                int place = Random.Range(0, EnemyPlaces.Count - 1);
                Vector3 newPosition = EnemyPlaces[place].transform.position;
                newPosition.y += 0.01f;
                if (EnemyCard[i].GetComponent<CardInfoScr>().SelfCard.Mana > EnemyMana)
                {
                    continue;
                }
                EnemyCard[i].transform.position = newPosition;
                Vector3 rotationAngles = new Vector3(90f, 0f, 180f);
                EnemyCard[i].transform.rotation = Quaternion.Euler(rotationAngles);
                EnemyCard[i].layer = LayerMask.NameToLayer("EnemyPlayed");
                EnemyPlaces[place].gameObject.tag = "busy";
                EnemyCard[i].transform.parent = EnemyPlaces[place].transform;
                EnemyCard[i].transform.localScale = new Vector3(1.36000001f, 1.64999998f, 0.925607145f);
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
                newPosition.y += 0.01f;
                EnemyCard[i].transform.position = newPosition;
                Vector3 rotationAngles = new Vector3(90f, 0f, 180f);
                EnemyCard[i].transform.rotation = Quaternion.Euler(rotationAngles);
                EnemyCard[i].layer = LayerMask.NameToLayer("EnemyPlayed");
                EnemyCard[i].transform.localScale = new Vector3(1.36f, 1.65f, 0.925f);
                //CardInfo.ChangeInfo(EnemyCard[i]);
                EnemyPlaces[place].gameObject.tag = "busy";
                EnemyMana -= EnemyCard[i].GetComponent<CardInfoScr>().SelfCard.Mana;
                ShowMana();
                EnemyPlaces.RemoveAt(place);
                EnemyCard.RemoveAt(i);
            }
        }
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
        //for(int i = 0; i < 4; i++)
        //{
        //    if (PlayerBuildingsBoxes[i].tag == "busy")
        //    {

        //        Transform childGameObject = PlayerBuildingsBoxes[i].transform.GetChild(0);
        //        GameObject childTransform = childGameObject.gameObject;
        //        childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(1);
        //        childTransform.GetComponent<CardInfoScr>().RefreshData();
        //    }
        //}
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
        //for (int i = 0; i < 4; i++)
        //{
        //    if (EnemyBuildingsBoxes[i].tag == "busy")
        //    {

        //        Transform childGameObject = PlayerBuildingsBoxes[i].transform.GetChild(0);
        //        GameObject childTransform = childGameObject.gameObject;
        //        childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(1);
        //        childTransform.GetComponent<CardInfoScr>().RefreshData();
        //    }
        //}
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
                            }
                            break;
                        }
                    }
                    else if (i == 2 && childTransform.GetComponent<CardInfoScr>().SelfCard.Range == 2 && EnemyWallHP > 0 && IsPlayerTurn)
                    {
                        EnemyWallHP -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                        if (EnemyWallHP <= 0)
                        {
                            DestroyImmediate(EnemyWall);
                            ShowHPWall();
                        }
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
                            }
                            break;
                        }

                    }
                    else if (i == 2 && childTransform.GetComponent<CardInfoScr>().SelfCard.Range == 2 && EnemyWallHP > 0 && IsPlayerTurn)
                    {
                        EnemyWallHP -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                        if (EnemyWallHP <= 0)
                        {
                            DestroyImmediate(EnemyWall);
                            ShowHPWall();
                        }
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
                            }
                            break;
                        }

                    }
                    else if (i == 2 && childTransform.GetComponent<CardInfoScr>().SelfCard.Range == 2 && EnemyWallHP > 0 && IsPlayerTurn)
                    {
                        EnemyWallHP -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                        if (EnemyWallHP <= 0)
                        {
                            DestroyImmediate(EnemyWall);
                            ShowHPWall();
                        }
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
                            }
                            break;
                        }

                    }
                    else if (i == 2 && childTransform.GetComponent<CardInfoScr>().SelfCard.Range == 2 && EnemyWallHP > 0 && IsPlayerTurn)
                    {
                        EnemyWallHP -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                        if (EnemyWallHP <= 0)
                        {
                            DestroyImmediate(EnemyWall);
                            ShowHPWall();
                        }
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
                            }
                            break;
                        }

                    }
                    else if (i == 1 && childTransform.GetComponent<CardInfoScr>().SelfCard.Range == 2 && PlayerWallHP > 0 && !IsPlayerTurn)
                    {
                        PlayerWallHP -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                        if (PlayerWallHP <= 0)
                        {
                            DestroyImmediate(PlayerWall);
                            ShowHPWall();
                        }
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
                            }
                            break;
                        }

                    }
                    else if (i == 1 && childTransform.GetComponent<CardInfoScr>().SelfCard.Range == 2 && PlayerWallHP > 0 && !IsPlayerTurn)
                    {
                        PlayerWallHP -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                        if (PlayerWallHP <= 0)
                        {
                            DestroyImmediate(PlayerWall);
                            ShowHPWall();
                        }
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
                            }
                            break;
                        }
                    }
                    else if (i == 1 && childTransform.GetComponent<CardInfoScr>().SelfCard.Range == 2 && PlayerWallHP > 0 && !IsPlayerTurn)
                    {
                        PlayerWallHP -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                        if (PlayerWallHP <= 0)
                        {
                            DestroyImmediate(PlayerWall);
                            ShowHPWall();
                        }
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
                            }
                            break;
                        }
                    }
                    else if (i == 1 && childTransform.GetComponent<CardInfoScr>().SelfCard.Range == 2 && PlayerWallHP > 0 && !IsPlayerTurn)
                    {
                        PlayerWallHP -= childTransform.GetComponent<CardInfoScr>().SelfCard.Attack;
                        if (PlayerWallHP <= 0)
                        {
                            DestroyImmediate(PlayerWall);
                            ShowHPWall();
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
                    if (PlayerWallHP <= 0)
                    {
                        DestroyImmediate(PlayerWall);
                        ShowHPWall();
                    }
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
                if (PlayerWallHP <= 0)
                {
                    DestroyImmediate(PlayerWall);
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
                if (PlayerWallHP <= 0)
                {
                    DestroyImmediate(PlayerWall);
                    ShowHPWall();
                }
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
                if (PlayerWallHP <= 0)
                {
                    DestroyImmediate(PlayerWall);
                    ShowHPWall();
                }
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
                childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(1);
                childTransform.GetComponent<CardInfoScr>().RefreshData();
            }
        }
        if (PlayerWarriorHP1 <= 0)
        {
            DestroyImmediate(EnemyWarrior1);
        }
        if (PlayerWarriorHP2 <= 0)
        {
            DestroyImmediate(EnemyWarrior2);
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
                    if (EnemyWallHP <= 0)
                    {
                        DestroyImmediate(EnemyWall);
                        ShowHPWall();
                    }
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
                if (EnemyWallHP <= 0)
                {
                    DestroyImmediate(EnemyWall);
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
                if (EnemyWallHP <= 0)
                {
                    DestroyImmediate(EnemyWall);
                    ShowHPWall();
                }
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
                if (EnemyWallHP <= 0)
                {
                    DestroyImmediate(EnemyWall);
                    ShowHPWall();
                }
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
                childTransform.GetComponent<CardInfoScr>().SelfCard.GetDamage(1);
                childTransform.GetComponent<CardInfoScr>().RefreshData();
            }
        }
        ShowHPKhan();
        ShowHPWarrior();
        ShowHPWall();
    }



    public void ShowMana()
    {
        PlayerManaTxt.text = PlayerMana.ToString();
        EnemyManaTxt.text = EnemyMana.ToString();
    }
    void ShowHPWall()
    {
        PlayerWallHPTxt.text = PlayerWallHP.ToString();
        EnemyWallHPTxt.text = EnemyWallHP.ToString();
    }
    void ShowHPWarrior()
    {
        PlayerWarriorHP1Txt.text = PlayerWarriorHP1.ToString();
        PlayerWarriorHP2Txt.text = PlayerWarriorHP2.ToString();
        EnemyWarriorHP1Txt.text = EnemyWarriorHP1.ToString();
        EnemyWarriorHP2Txt.text = EnemyWarriorHP2.ToString();
    }
    void ShowHPKhan()
    {
        PlayerKhanHPTxt.text = PlayerKhanHP.ToString();
        EnemyKhanHPTxt.text = EnemyKhanHP.ToString();
    }



}
