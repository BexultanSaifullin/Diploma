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
        GameObject spawnedObject = Instantiate(objectToSpawn, new Vector3(10, 10, 10), Quaternion.Euler(45, 0, 0));
        spawnedObject.transform.parent = transform;
        //if(spawnedObject )
        //{

        //}
        Arrange.ArrangeCards();
    }
}
