using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeManager : Singleton<LandscapeManager>
{
    [Header("1st Row[Player]")]
    [SerializeField] GameObject Landscape11;
    [SerializeField] GameObject Landscape12;
    [SerializeField] GameObject Landscape13;
    [SerializeField] GameObject Landscape14;

    [Header("2nd Row[Player]")]
    [SerializeField] GameObject Landscape21;
    [SerializeField] GameObject Landscape22;
    [SerializeField] GameObject Landscape23;
    [SerializeField] GameObject Landscape24;

    [Header("3rd Row[Enemy]")]
    [SerializeField] GameObject Landscape31;
    [SerializeField] GameObject Landscape32;
    [SerializeField] GameObject Landscape33;
    [SerializeField] GameObject Landscape34;

    [Header("4th Row[Enemy]")]
    [SerializeField] GameObject Landscape41;
    [SerializeField] GameObject Landscape42;
    [SerializeField] GameObject Landscape43;
    [SerializeField] GameObject Landscape44;

    public Grid[,] getGridArray()
    {
        Grid[,] newGrid = new Grid[4, 4];
        newGrid[0, 0].landscapeObject = Landscape11;
        newGrid[0, 1].landscapeObject = Landscape12;
        newGrid[0, 2].landscapeObject = Landscape13;
        newGrid[0, 3].landscapeObject = Landscape14;
        
        newGrid[1, 0].landscapeObject = Landscape21;
        newGrid[1, 1].landscapeObject = Landscape22;
        newGrid[1, 2].landscapeObject = Landscape23;
        newGrid[1, 3].landscapeObject = Landscape24;

        newGrid[2, 0].landscapeObject = Landscape31;
        newGrid[2, 1].landscapeObject = Landscape32;
        newGrid[2, 2].landscapeObject = Landscape33;
        newGrid[2, 3].landscapeObject = Landscape34;

        newGrid[3, 0].landscapeObject = Landscape41;
        newGrid[3, 1].landscapeObject = Landscape42;
        newGrid[3, 2].landscapeObject = Landscape43;
        newGrid[3, 3].landscapeObject = Landscape44;

        return newGrid;
    }
}

public class Grid // one tile of the whole grid
{
    public GameObject landscapeObject { set; get; } // Landscape where the grid is linked to
    Landscape.Side side; // Landscape's side (player/enemy) 

    Card standingCard { set; get; }

    public bool isPlayer()
    {
        return side.Equals(Landscape.Side.Player);
    }

    public bool isEnemy()
    {
        return side.Equals(Landscape.Side.Enemy);
    }
    public void getAttackedBy(Grid attacker)
    {
        // TODO Add animation and sound for attack and death
        standingCard.currentHealth -= attacker.standingCard.currentAttack;
        attacker.standingCard.currentHealth -= standingCard.currentHealth;

        if (attacker.standingCard.currentHealth <= 0)
        {
            attacker.Death();
        }
        if (standingCard.currentHealth <= 0)
        {
            Death();
        }
    }

    public void moveCard(Grid newGrid)
    {
        Vector3 newLocation = newGrid.landscapeObject.GetComponent<Landscape>().getLocation();
        newLocation.z = 0f;
        standingCard.transform.position = newLocation;

        // TODO add movement animation and sound
        newGrid.standingCard = standingCard;
        standingCard = null;
    }

    private void Death()
    {
        GameObject.Destroy(standingCard.gameObject);
    }
}
