using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawnerEnemyScr : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject objectToSpawnEnemy;
    public Vector3 positionToSpawn;
    Drag Arrange;
    void Start()
    {
        Arrange = FindObjectOfType<Drag>();
    }
    public void SpawnEnemy()
    {
        GameObject spawnedObject = Instantiate(objectToSpawnEnemy, positionToSpawn, Quaternion.Euler(135, 0, -180));
        spawnedObject.transform.parent = transform;
        Arrange.ArrangeCardsToEnemy();
    }
}
