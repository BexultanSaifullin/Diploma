using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]

public class Landscape : MonoBehaviour
{
    private bool cardPlaced = false;
    private bool mouseIsHovering = false;

    [SerializeField] Side whoseLandscape;
    Side whoseCard;

    GameObject currentHoverEffect;

    private void OnMouseDown()
    {
        destroyHoverEffect();

    }

    private void OnMouseEnter()
    {
        mouseIsHovering = true;
        createHoverEffect();
        // Glow the Landscape
        // depending on whether if it's an idle or attack state of the mouse show the tooltip(description of the card) of the card(if it exists)
    }

    private void OnMouseExit()
    {
        mouseIsHovering = false;
        destroyHoverEffect();
    }

    private void Update()
    {
        if (cardPlaced) 
        {
            // color it green
        }
        else
        {
            // color it default color
        }
    }

    public Vector3 getLocation()
    {
        return transform.position;
    }

    private void createHoverEffect()
    { 
        // create effect by getting the current mouse state and whether if it's enemy or player's card
    }

    private void destroyHoverEffect()
    {
        // destroy the effect
    }

    private void createHero()
    {
        // takes the information what kind of hero it shoud create on the landscape and then creates it
    }

    private void destroyHero()
    {
        // 
    }

    public enum Side
    {
        None,
        Player,
        Enemy
    }
}
