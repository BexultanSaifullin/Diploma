using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawnerEnemyTScr : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject objectToSpawnEnemy;
    public Vector3 positionToSpawn;
    public DragTutorial Arrange;
    void Start()
    {
        
    }
    public void SpawnEnemy()
    {
        GameObject spawnedObject = Instantiate(objectToSpawnEnemy, positionToSpawn, Quaternion.Euler(135, 0, -180));
        spawnedObject.GetComponent<CardInfoTutorialScr>().RandomMethod();
        spawnedObject.transform.parent = transform;
        Arrange.ArrangeCardsToEnemy();
    }

    public void NotRandomSpawnEnemy()
    {
        GameObject spawnedObject = Instantiate(objectToSpawnEnemy, positionToSpawn, Quaternion.Euler(135, 0, -180));
        spawnedObject.GetComponent<CardInfoTutorialScr>().NotRandom(4);
        spawnedObject.transform.parent = transform;
        Arrange.ArrangeCardsToEnemy();
    }
    public void SpawnWarrior()
    {
        GameObject spawnedObject = Instantiate(objectToSpawnEnemy, positionToSpawn, Quaternion.Euler(135, 0, -180));
        spawnedObject.GetComponent<CardInfoTutorialScr>().NotRandom(0);
        spawnedObject.transform.parent = transform;
        Arrange.ArrangeCardsToEnemy();
    }
}
