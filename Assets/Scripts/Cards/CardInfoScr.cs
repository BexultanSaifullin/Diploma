using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardInfoScr : MonoBehaviour
{
    public Card SelfCard;
    public Image Logo;
    public TextMeshProUGUI Name, Attack, Defense;
    public GameObject selectedObject;

    public void HideCardInfo(Card card)
    {
        SelfCard = card;
        Logo.sprite = null;
        Name.text = "";
    }
    public void ShowCardInfo(Card card)
    {
        SelfCard = card;
        Logo.sprite = card.Logo;
        Logo.preserveAspect = true;
        Name.text = card.Name;
        Attack.text = SelfCard.Attack.ToString();
        Defense.text = SelfCard.Defense.ToString();
    }
    private void Start()
    {
        int randomNumber = Random.Range(0, 4);
        if(selectedObject.layer == LayerMask.NameToLayer("Robot"))
            ShowCardInfo(CardManagerList.AllCards[randomNumber]);
        else
            ShowCardInfo(CardManagerList.AllCards[randomNumber]);
    }
    
}
