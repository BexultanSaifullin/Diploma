using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class CardSpawnerScr : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject objectToSpawn;
    public Vector3 positionToSpawn;
    Drag Arrange;
    
    void Start()
    {
        Arrange = FindObjectOfType<Drag>();
    }

    // Update is called once per frame
    public void Spawn()
    {
        GameObject spawnedObject = Instantiate(objectToSpawn, positionToSpawn, Quaternion.Euler(45, 180, 0));

        spawnedObject.transform.parent = transform;
        Arrange.ArrangeCards();
    }
    
}
