using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardManagerList
{
    public static List<Card> AllCards = new List<Card>();

}
public class CardManager : MonoBehaviour
{
    public static List<Card> playerCards = new List<Card>();
    public static List<Card> enemyCards = new List<Card>();

    Grid[,] grid;

    public void Awake()
    {
        //CardManagerList.AllCards.Add(new Card("Card ¹1", "Sprites/Pominki", 5, 5));
        //CardManagerList.AllCards.Add(new Card("Card ¹2", "Sprites/Pominki", 1, 1));
        //CardManagerList.AllCards.Add(new Card("Card ¹3", "Sprites/Pominki", 10, 1));
        //CardManagerList.AllCards.Add(new Card("Card ¹4", "Sprites/Pominki", 3, 3));
    }

    private void Start()
    {
        // import grid array from landscape manager
        grid = (Grid[,])LandscapeManager.Instance.getGridArray().Clone();
    }

    /*
        00 10 20 30
        01 11 21 31

        02 12 22 32
        03 13 23 33
    */

    public void PlayerAttackTurn()
    {
        // completes all commands that are given

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                // looks for player's hero standing in front of the column
                if (grid[i, j].isPlayer())
                {
                    if (j == 0) // - checks if its an end of the map
                    {
                        // attack the wall
                        // TODO attack the wall and take the damage from the wall
                    }
                    else if (grid[i, j - 1].isEnemy()) // - checks whether it has an enemy infront
                    {
                        grid[i, j - 1].getAttackedBy(grid[i, j]);
                    }
                    else
                    {
                        grid[i, j].moveCard(grid[i, j + 1]);
                    }
                    break;
                }
            }
        }
    }

    public void EnemyAttackTurn()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 3; j >= 0; j--)
            {
                // looks for enemy's hero standing in front of the column
                if (grid[i, j].isEnemy())
                {
                    if (j == 3) // - checks if its an end of the map
                    {
                        // TODO AttackTheWall()
                    }
                    else if (grid[i, j + 1].isPlayer())
                    {
                        grid[i, j + 1].getAttackedBy(grid[i, j]);
                    }
                    else
                    {
                        grid[i, j].moveCard(grid[i, j - 1]);
                    }
                    break;
                }
            }
        }
    }

    // draw means take !!!!!!!!!
    // draw not paint !!!!!!!!!
    public void PlayerDrawCard()
    {
        // TODO write coroutine for the draw animation
        // maybe make a coroutine method outside of this method in order to use less code space
    }
    public void EnemyDrawCard()
    {
        // TODO write coroutine for the draw animation
    }

    private void ImportDeck()
    {
        // import the player and the enemy's decks
        // add draw animation coroutine
    }
}