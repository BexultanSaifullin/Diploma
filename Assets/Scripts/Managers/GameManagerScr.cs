using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;




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
