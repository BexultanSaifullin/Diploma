using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardInfoScr : MonoBehaviour
{
    public Card SelfCard;
    public Image Logo;
    public TextMeshProUGUI Name, Attack, Defense, Mana;
    Drag Arrange;
    public GameObject selectedObject;

    public void ShowCardInfo(Card card)
    {
        SelfCard = card;
        Logo.sprite = card.Logo;
        Logo.preserveAspect = true;
        Name.text = card.Name;
        Mana.text = SelfCard.Mana.ToString();
        RefreshData();
    }
    public void RefreshData()
    {
        Attack.text = SelfCard.Attack.ToString();
        if (SelfCard.Type != "Spell")
        {
            Defense.text = SelfCard.Defense.ToString();
        }

    }
    private void Start()
    {
        int randomNumber = Random.Range(0, 2);

        ShowCardInfo(CardManagerList.AllCards[randomNumber]);
    }
}
