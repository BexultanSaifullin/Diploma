using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Game
{
    public List<Card> EnemyDeck, PlayerDeck,
                       EnemyHand, PlayerHand,
                       EnemyField, PlayerField;
    public Game()
    {
        EnemyDeck = GiveDeckCard();
        PlayerDeck = GiveDeckCard();

        EnemyHand = new List<Card>();
        PlayerHand = new List<Card>();

        EnemyField = new List<Card>();
        PlayerField = new List<Card>();
    }

    List<Card> GiveDeckCard()
    {
        List<Card> list = new List<Card>();
        for (int i = 0; i < 10; i++)
        {
            list.Add(CardManagerList.AllCards[Random.Range(0, CardManagerList.AllCards.Count)]);
        }
        return list;
    }


}
public class GameManagerScr : MonoBehaviour
{
    public Game CurrentGame;
    public Transform EnemyHand, PlayerHand;
    public GameObject CardPref;
    void Start()
    {
        CurrentGame = new Game();

        GiveHandCards(CurrentGame.EnemyDeck, EnemyHand);
        GiveHandCards(CurrentGame.PlayerDeck, PlayerHand);
    }

    void GiveHandCards(List<Card> deck, Transform hand)
    {
        int i = 0;
        while (i++ < 4)
        {
            GiveCardToHand(deck, hand);
        }
    }

    void GiveCardToHand(List<Card> deck, Transform hand)
    {
        if (deck.Count > 0)
        {
            return;
        }
        Card card = deck[0];

        GameObject cardGO = Instantiate(CardPref, hand, false);
        if (hand == EnemyHand)
        {
            cardGO.GetComponent<CardInfoScr>().ShowCardInfo(card);
        }
        else
            cardGO.GetComponent<CardInfoScr>().ShowCardInfo(card);
        deck.RemoveAt(0);
    }

}
