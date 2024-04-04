using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Card
{
    public string Name;
    public Sprite Logo;
    public int Attack, Defense, Range, Mana;

    public Card(string name,  string logoPath, int attack, int defense, int range, int mana)
    {
        Name = name;
        Logo = Resources.Load<Sprite>(logoPath);
        Attack = attack;
        Defense = defense;
        Range = range;
        Mana = mana;
    }
    public bool IsAlive
    {
        get
        {
            return Defense > 0;
        }
    }
    public void GetDamage(int dmg)
    {
        Defense -= dmg;
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
        CardManagerList.AllCards.Add(new Card("Warrior", "Sprites/Pominki", 5, 4, 1, 3));
        CardManagerList.AllCards.Add(new Card("Man", "Sprites/Pominki", 1, 1, 1, 1));
        CardManagerList.AllCards.Add(new Card("Archer", "Sprites/Pominki", 10, 1, 2, 4));
        CardManagerList.AllCards.Add(new Card("NaN", "Sprites/Pominki", 2, 3, 1, 2));
    }
    
   
}
