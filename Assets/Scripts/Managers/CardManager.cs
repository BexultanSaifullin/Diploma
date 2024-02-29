using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Card
{
    public string Name;
    public Sprite Logo;
    public int Attack, Defense;

    public Card(string name,  string logoPath, int attack, int defense)
    {
        Name = name;
        Logo = Resources.Load<Sprite>(logoPath);
        Attack = attack;
        Defense = defense;
    }
}
public static class CardManagerList
{
    public static List<Card> AllCards = new List<Card>();

}
public class CardManager : MonoBehaviour
{
    public void Awake()
    {
        CardManagerList.AllCards.Add(new Card("Card ¹1", "Sprites/Pominki", 5, 5));
        CardManagerList.AllCards.Add(new Card("Card ¹2", "Sprites/Pominki", 1, 1));
        CardManagerList.AllCards.Add(new Card("Card ¹3", "Sprites/Pominki", 10, 1));
        CardManagerList.AllCards.Add(new Card("Card ¹4", "Sprites/Pominki", 3, 3));
    }
    

}
