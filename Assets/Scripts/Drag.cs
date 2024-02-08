using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    [SerializeField] private Collider currentCollider;
    private Camera mainCamera;
    private Plane dragPlane;
    private bool inputStart;
    private Vector3 offset;

    // Start is called before the first frame update
    private void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    private void Update()
    {


        if (Input.GetMouseButtonDown(0))
        {
            SelectPart();
        }
        if (Input.GetMouseButtonUp(0))
        { 
            Drop();
        }


        DragAndDropObject();
    }
    private void SelectPart()
    {
        RaycastHit hit;
        Ray camRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(camRay, out hit, 2000f, LayerMask.GetMask("Robot")))
        {
            currentCollider = hit.collider;
            dragPlane = new Plane(mainCamera.transform.forward, currentCollider.transform.position);
            float planeDist;
            dragPlane.Raycast(camRay, out planeDist);
            offset = currentCollider.transform.position - camRay.GetPoint(planeDist);
        }
    }

    private void DragAndDropObject()
    {
        if (currentCollider == null)
        {
            return;
        }
        Ray camRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        float planeDist;
        dragPlane.Raycast(camRay, out planeDist);
        currentCollider.transform.position = camRay.GetPoint(planeDist) + offset;

        if(currentCollider.transform.position.y < 0.5f)
        {
            currentCollider.transform.position =
                new Vector3(currentCollider.transform.position.x,
                    0.5f,
                    currentCollider.transform.position.z);
        }
            
    }

    private void Drop()
    {
        if (currentCollider == null)
        { 
            return;
        }
        currentCollider.transform.position =
            new Vector3(currentCollider.transform.position.x,
                0.5f,
                currentCollider.transform.position.z);
        currentCollider = null;
    }

}
