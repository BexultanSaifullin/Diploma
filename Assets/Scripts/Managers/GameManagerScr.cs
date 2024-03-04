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
    public bool IsPlayerTurn
    {
        get
        {
            return Turn % 2 == 0;
        }
    }
    void Start()
    {
        Turn = 0;
        StartCoroutine(TurnFunc());
        Spawner = FindObjectOfType<CardSpawnerScr>();
        SpawnerEnemy = FindObjectOfType<CardSpawnerEnemyScr>();
    }
    IEnumerator TurnFunc()
    {
        TurnTime = 30;
        TurnTimeTxt.text = TurnTime.ToString();
        if(IsPlayerTurn)
        {
            while(TurnTime-- > 0)
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
        ChangeTurn();
    }

    void EnemyTurn(List<GameObject> EnemyCard, List<GameObject> EnemyPlaces)
    {
        if(EnemyPlaces.Count > EnemyCard.Count)
        {
            int count = Random.Range(-1, EnemyCard.Count);
            if (count == -1)
                    return;
          
            for (int i = count; i >= 0; i--)
            {
                int place = Random.Range(0, EnemyPlaces.Count-1);
                EnemyCard[i].transform.position = EnemyPlaces[place].transform.position;
                EnemyCard[i].layer = LayerMask.NameToLayer("EnemyPlaying");
                EnemyPlaces[place].gameObject.tag = "busy";
                EnemyPlaces.RemoveAt(place);
                EnemyCard.RemoveAt(i);
            }
        } else
        {
            int count = Random.Range(-1, EnemyPlaces.Count);
            if (count == -1)
                return;
            for (int i = count; i >= 0; i--)
            {
                int place = Random.Range(0, EnemyPlaces.Count-1);
                EnemyCard[i].transform.position = EnemyPlaces[place].transform.position;
                EnemyCard[i].layer = LayerMask.NameToLayer("EnemyPlaying");
                EnemyPlaces[place].gameObject.tag = "busy";
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
            GivenNewCards();
        } else if(Turn !=1)
        {
            GivenNewCardsToEnemy();
        }
        StartCoroutine(TurnFunc());
    }
    void GivenNewCards()
    {
        Spawner.Spawn();
    }
    void GivenNewCardsToEnemy()
    {
        SpawnerEnemy.SpawnEnemy();
    }
}
