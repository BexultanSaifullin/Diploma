using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropPlace : MonoBehaviour
{
    public Transform newParent;
    Material material;
    private Drag instanceOfDrag;

    public void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        instanceOfDrag = new Drag();
    }
    void OnTriggerEnter(Collider cube)
    {
        instanceOfDrag.ArrangeCards();
    }

}

