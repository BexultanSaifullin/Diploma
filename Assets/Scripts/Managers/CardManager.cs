using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Card
{
    public string Name, Type;
    public int Attack, Defense, Range, Mana;
    public bool Abyllity;
    public int Id;

    public Card(string name, int attack, int defense, int range, int mana, string type, bool abyllity, int id)
    {
        Name = name;
        Attack = attack;
        Defense = defense;
        Range = range;
        Mana = mana;
        Type = type;
        Abyllity = abyllity;
        Id = id;
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
        CardManagerList.AllCards.Add(new Card("Warrior", 1, 10, 1, 3, "Unit", false, 1));
        CardManagerList.AllCards.Add(new Card("Yurt", 0, 10, 0, 2, "Building", false, 2));
        CardManagerList.AllCards.Add(new Card("Barak", 0, 10, 0, 2, "Building", false, 3));
        CardManagerList.AllCards.Add(new Card("Bowrange", 0, 10, 0, 2, "Building", false, 4));
        CardManagerList.AllCards.Add(new Card("Man", 1, 1, 1, 1, "Unit", false, 5));
        CardManagerList.AllCards.Add(new Card("Archer", 1, 10, 2, 4, "Unit", false, 6));
        CardManagerList.AllCards.Add(new Card("Ensign", 1, 5, 1, 4, "Unit", true, 7));
        CardManagerList.AllCards.Add(new Card("Mystan", 1, 1, 1, 4, "Unit", true, 8));
        CardManagerList.AllCards.Add(new Card("Arrows", 1, 0, 1, 2, "Spell", false, 9));
        CardManagerList.AllCards.Add(new Card("Jut", 2, 0, 0, 1, "Spell", false, 10));



    }


}
