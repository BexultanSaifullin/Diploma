using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropPlace : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform newParent; // ������ �� ������ ��������
    Material material;
    public void Start()
    {
        material = GetComponent<MeshRenderer>().material;
    }
    void OnTriggerEnter(Collider cube)
    {
       
        
            if (cube.transform.IsChildOf(transform))
            {
                material.color = Color.blue;
            }
            else
            {
                // ����� �������� �������
                cube.transform.parent = transform;
                material.color = Color.green;
            }
        
    }

}   
