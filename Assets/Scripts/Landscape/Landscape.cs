using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]

public class Landscape : MonoBehaviour
{
    private bool cardPlaced = false;
    private bool mouseIsHovering = false;
    
    GameObject currentHoverEffect;

    private void OnMouseDown()
    {
        // Works with MouseStateManager
    }

    private void OnMouseEnter()
    {
        mouseIsHovering = true;
        createHoverEffect();
        // Glow the Landscape
        // Add icon of attack depending on the mouseState
        // depending on whether if it's an idle or attack state of the mouse show the tooltip(description of the card) of the card(if it exists)
    }

    private void OnMouseExit()
    {
        mouseIsHovering = false;
        destroyHoverEffect();
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
}
