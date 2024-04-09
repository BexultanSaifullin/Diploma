using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Card
{
    public string Name, Type;
    public Sprite Logo;
    public int Attack, Defense, Range, Mana;
    public bool Abyllity;

    public Card(string name,  string logoPath, int attack, int defense, int range, int mana, string type, bool abyllity)
    {
        Name = name;
        Logo = Resources.Load<Sprite>(logoPath);
        Attack = attack;
        Defense = defense;
        Range = range;
        Mana = mana;
        Type = type;
        Abyllity = abyllity;
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
        CardManagerList.AllCards.Add(new Card("Warrior", "Sprites/Pominki", 1, 10, 1, 3, "Unit", false));
        CardManagerList.AllCards.Add(new Card("Man", "Sprites/Pominki", 1, 1, 1, 1, "Unit", false));
        CardManagerList.AllCards.Add(new Card("Archer", "Sprites/Pominki", 1, 10, 2, 4, "Unit", false));
        CardManagerList.AllCards.Add(new Card("NaN", "Sprites/Pominki", 1, 10, 1, 2, "Unit", false));
        CardManagerList.AllCards.Add(new Card("Yurt", "Sprites/Pominki", 1, 10, 0, 2, "Building", false));
    }
    
   
}
