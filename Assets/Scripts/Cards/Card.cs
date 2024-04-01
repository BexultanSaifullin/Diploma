using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]

public class Card : MonoBehaviour
{
    private static int idCounter = 0;
    private bool mouseIsHovering = false;

    [SerializeField] public string heroName;
    [SerializeField] public string description;
    [SerializeField] public Sprite logo;

    [SerializeField] public int defaultHealth;
    [SerializeField] public int defaultAttack;
    [SerializeField] public int manaCost;

    public int id;

    public int currentHealth { set; get; }
    public int currentAttack { set; get; }

    private void Start()
    {
        id = idCounter++;

        // TODO write utility to transform id from 1 into 0001
        gameObject.name = $"[{id}] Card: {heroName}"; 

        currentHealth = defaultHealth;
        currentAttack = defaultAttack;
    }
    
    private void OnMouseEnter()
    {
        mouseIsHovering = true;
        createHoverEffect();
    }

    private void OnMouseExit()
    {
        mouseIsHovering = false;
        destroyHoverEffect();
    }

    private void createHoverEffect()
    {

    }

    private void destroyHoverEffect()
    {

    }
}
