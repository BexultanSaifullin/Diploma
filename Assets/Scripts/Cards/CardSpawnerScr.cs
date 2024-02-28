using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawnerScr : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject objectToSpawn;
    Drag Arrange;
    void Start()
    {
        Arrange = FindObjectOfType<Drag>();
    }

    // Update is called once per frame
    public void Spawn()
    {
        Vector3 spawnPosition = new Vector3(10, 10, 10);
        Quaternion spawnRotation = Quaternion.identity;
        GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation);
        spawnedObject.transform.parent = transform;
        Arrange.ArrangeCards();
    }
}
