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
    public Transform PlayerHand;
    public GameObject[] qwd;
    Vector3[] positions;
    Drag Arrange;
    
    public void Spawn()
    {
        int a = PlayerHand.childCount;
        GameObject spawnedObject = Instantiate(objectToSpawn, qwd[a].transform.position, qwd[a].transform.rotation);
        
        spawnedObject.transform.parent = transform;
        spawnedObject.transform.localScale = new Vector3(8f, 8f, 8f);
    }
    
}
