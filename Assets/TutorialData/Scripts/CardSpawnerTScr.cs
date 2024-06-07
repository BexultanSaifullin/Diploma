using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class CardSpawnerTScr : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject objectToSpawn;
    public Transform PlayerHand;
    public GameObject[] qwd;
    Vector3[] positions;
    DragTutorial Arrange;
    
    public void Spawn()
    {
        int a = PlayerHand.childCount;
        GameObject spawnedObject = Instantiate(objectToSpawn, qwd[a].transform.position, qwd[a].transform.rotation);
        spawnedObject.GetComponent<CardInfoTutorialScr>().RandomMethod();
        spawnedObject.transform.parent = transform;
        spawnedObject.transform.localScale = new Vector3(8f, 8f, 8f);
    }

    public void NotRandomSpawn()
    {
        int a = PlayerHand.childCount;
        GameObject spawnedObject = Instantiate(objectToSpawn, qwd[a].transform.position, qwd[a].transform.rotation);
        spawnedObject.GetComponent<CardInfoTutorialScr>().NotRandom(4);
        spawnedObject.transform.parent = transform;
        spawnedObject.transform.localScale = new Vector3(8f, 8f, 8f);
    }
    public void SpawnWarrior()
    {
        int a = PlayerHand.childCount;
        GameObject spawnedObject = Instantiate(objectToSpawn, qwd[a].transform.position, qwd[a].transform.rotation);
        spawnedObject.GetComponent<CardInfoTutorialScr>().NotRandom(0);
        spawnedObject.transform.parent = transform;
        spawnedObject.transform.localScale = new Vector3(8f, 8f, 8f);
    }
    public void SpawnArcher()
    {
        int a = PlayerHand.childCount;
        GameObject spawnedObject = Instantiate(objectToSpawn, qwd[a].transform.position, qwd[a].transform.rotation);
        spawnedObject.GetComponent<CardInfoTutorialScr>().NotRandom(5);
        spawnedObject.transform.parent = transform;
        spawnedObject.transform.localScale = new Vector3(8f, 8f, 8f);
    }
}
