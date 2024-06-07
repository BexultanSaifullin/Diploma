using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawnerEnemyScr : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject objectToSpawnEnemy;
    public Vector3 positionToSpawn;
    public Drag Arrange;
    void Start()
    {
        
    }
    public void SpawnEnemy()
    {
        GameObject spawnedObject = Instantiate(objectToSpawnEnemy, positionToSpawn, Quaternion.Euler(135, 0, -180));
        spawnedObject.GetComponent<CardInfoScr>().RandomMethod();
        spawnedObject.transform.parent = transform;
        Arrange.ArrangeCardsToEnemy();
    }

    public void NotRandomSpawnEnemy()
    {
        GameObject spawnedObject = Instantiate(objectToSpawnEnemy, positionToSpawn, Quaternion.Euler(135, 0, -180));
        spawnedObject.GetComponent<CardInfoScr>().NotRandom(4);
        spawnedObject.transform.parent = transform;
        Arrange.ArrangeCardsToEnemy();
    }
}
