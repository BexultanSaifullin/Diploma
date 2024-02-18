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
        CardManagerList.AllCards.Add(new Card("Ebalo", "Sprites/Pominki", 5, 5));
        CardManagerList.AllCards.Add(new Card("Pominki", "Sprites/Pominki", 5, 5));
        CardManagerList.AllCards.Add(new Card("Papich", "Sprites/Pominki", 5, 5));
        CardManagerList.AllCards.Add(new Card("Musor", "Sprites/Pominki", 5, 5));
    }
    

}
