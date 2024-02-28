using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawnerEnemyScr : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject objectToSpawnEnemy;
    Drag Arrange;
    void Start()
    {
        Arrange = FindObjectOfType<Drag>();
    }
    public void SpawnEnemy()
    {
        GameObject spawnedObject = Instantiate(objectToSpawnEnemy, new Vector3(10, 10, 10), Quaternion.Euler(45, 0, 0));
        spawnedObject.transform.parent = transform;
        //Arrange.ArrangeCardsToEnemy();
    }
}
