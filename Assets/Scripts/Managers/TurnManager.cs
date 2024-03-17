using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{
    TurnState turnState;
    void Start()
    {
        turnState = TurnState.None;
    }

    void Update()
    {
        
    }

    public enum TurnState
    {
        None,
        PlayerTurn,
        EnemyTurn,
        Battle,
        ChooseDeck,
        Start,
        Victory,
        Defeat
    }

    // To change the state of the game and invoke the methods that should be invoken in certain states
    // ex: Beginning of the game players draw certain amounts of cards
    // ex: It's the Enemy's turn and the player can't put his cards on the table

    private void ChangeState(TurnState _turnState)
    {
        // plays animation or invokes UI calls
        switch (_turnState)
        {
            case TurnState.None:

                break;
            case TurnState.PlayerTurn:
                
                break;
            case TurnState.EnemyTurn:
                
                break;
            case TurnState.Start:
                
                break;
            case TurnState.Victory:
                
                break;
            case TurnState.Defeat:
                
                break;
            default:
                Debug.Log("[TurnManager::ChangeState]: switch went to default");
                break;
        }
        turnState = _turnState;
    }

    public void NextTurn()
    {
        if(turnState == TurnState.PlayerTurn) 
        {
            ChangeState(TurnState.EnemyTurn);
        }
        else if(turnState == TurnState.EnemyTurn)
        {
            ChangeState(TurnState.PlayerTurn);
        }
    }
}
