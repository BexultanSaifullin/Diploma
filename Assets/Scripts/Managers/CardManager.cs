using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Card
{
    public string Name, Type;
    public Sprite Logo;
    public int Attack, Defense, Range, Mana;
    public bool Abyllity;
    public GameObject Prefab;

    public Card(string name, string logoPath, int attack, int defense, int range, int mana, string type, bool abyllity, GameObject prefab)
    {
        Name = name;
        Logo = Resources.Load<Sprite>(logoPath);
        Attack = attack;
        Defense = defense;
        Range = range;
        Mana = mana;
        Type = type;
        Abyllity = abyllity;
        Prefab = prefab;
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
    public void SetBaff(int dmg) 
    {
        Defense += dmg;
        Attack += dmg;
    }
    public void GetBaff(int dmg)
    {
        Defense -= dmg;
        Attack -= dmg;
    }
}
public static class CardManagerList
{
    public static List<Card> AllCards = new List<Card>();
}
public class CardManager : MonoBehaviour
{
    public GameObject[] cardModels;
    public void Awake()
    {
        CardManagerList.AllCards.Add(new Card("Warrior", "Sprites/Pominki", 1, 10, 1, 3, "Unit", false, cardModels[0]));
        CardManagerList.AllCards.Add(new Card("Man", "Sprites/Pominki", 1, 1, 1, 1, "Unit", false, cardModels[1]));
        CardManagerList.AllCards.Add(new Card("Archer", "Sprites/Pominki", 1, 10, 2, 4, "Unit", false, cardModels[2]));
        CardManagerList.AllCards.Add(new Card("Arrows", "Sprites/Pominki", 1, 10, 1, 2, "Spell", false, cardModels[3]));
        CardManagerList.AllCards.Add(new Card("Jut", "Sprites/Pominki", 2, 10, 1, 2, "Spell", false, cardModels[3]));
        CardManagerList.AllCards.Add(new Card("Yurt", "Sprites/Pominki", 0, 10, 0, 2, "Building", false, cardModels[4]));
        CardManagerList.AllCards.Add(new Card("Barak", "Sprites/Pominki", 0, 10, 0, 2, "Building", false, cardModels[4]));
        CardManagerList.AllCards.Add(new Card("Bowrange", "Sprites/Pominki", 0, 10, 0, 2, "Building", false, cardModels[4]));
        
        
    }


}
