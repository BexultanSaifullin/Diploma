using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class InformationManagerScr : MonoBehaviour
{
    public static int PlayerWallHP = 20, EnemyWallHP = 20, WallDMG = 1,

                PlayerWarriorHP1 = 6, EnemyWarriorHP1 = 6, PlayerWarriorHP2 = 6, EnemyWarriorHP2 = 6,
                    PlayerKhanHP = 10, EnemyKhanHP = 10,
                        PlayerMana = 1, EnemyMana = 1, PlayerCardsCount = 10, EnemyCardsCount = 10,
                            Turn, TurnTime = 30, increase = 1;
    public GameObject[] Models;
    public TextMeshProUGUI PlayerManaTxt;
    public CardSpawnerScr Spawner;
    public bool IsPlayerTurn
    {
        get
        {
            return Turn % 2 == 0;
        }
    }

    public void ShowManaPlayer()
    {
        PlayerManaTxt.text = PlayerMana.ToString();
    }

}
