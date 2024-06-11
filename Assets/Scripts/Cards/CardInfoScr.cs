using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardInfoScr : MonoBehaviour
{
    public Material[] NewDefense, NewMana, NewAttack, NewLogo;
    public Card SelfCard;
    public GameObject Name, Attack, Defense, Mana, Logo;
    Drag Arrange;

    public void ShowCardInfo(Card card, int i)
    {
        SelfCard = card;
        Renderer mana = Mana.GetComponent<Renderer>();
        mana.material = NewMana[card.Mana];
        Renderer logo = Logo.GetComponent<Renderer>();
        logo.material = NewLogo[i];
        Renderer name = Name.GetComponent<Renderer>();
        name.material = NewLogo[i];
        RefreshData();
    }
    public void RefreshData()
    {
        if (SelfCard.Defense > 0)
        {
            Renderer defense = Defense.GetComponent<Renderer>();
            defense.material = NewDefense[SelfCard.Defense];
        }
        else
        {
            Renderer defense = Defense.GetComponent<Renderer>();
            defense.material = NewDefense[0];
        }
        if (SelfCard.Attack > 0)
        {
            Renderer attack = Attack.GetComponent<Renderer>();
            attack.material = NewAttack[SelfCard.Attack];
        }
        else
        {
            Renderer attack = Attack.GetComponent<Renderer>();
            attack.material = NewAttack[0];
        }
    }
    public void RandomMethod()
    {
        int randomNumber = Random.Range(0, 11);
        
        ShowCardInfo(CardManagerList.AllCards[randomNumber], randomNumber);
    }

    public void NotRandom(int randomNumber)
    {
        ShowCardInfo(CardManagerList.AllCards[randomNumber], randomNumber);
    }
}
