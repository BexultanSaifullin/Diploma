using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour
{
    [SerializeField] private Collider currentCollider;
    private Collider currentCollider2;
    private Camera mainCamera;
    private Plane dragPlane;
    private bool inputStart;
    private Vector3 offset;
    private Vector3 newPosition;
    private bool coroutineCalled = false;
    public string defaultLayerName = "Playing";
    private GameObject selectedObject;
    public string free = "free";
    public string busy = "busy";
    public GameObject parentObject;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

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
        //if (Input.GetMouseButtonUp(0))
        //{
        //    Drop();
        //}
        if (Input.GetButtonDown("Jump") && currentCollider2 != null && mainCamera.transform.position.y < 2f && currentCollider2.CompareTag("Card"))
        {
            StartCoroutine(StepFromAbove());
        }
        if (Input.GetButtonDown("Jump") && currentCollider2 != null && mainCamera.transform.position.y == 5.51f && currentCollider2.CompareTag("Card"))
        {
            BackFromAbove();
        }
        if (Input.GetMouseButtonDown(0) && mainCamera.transform.position.y == 5.51f)
        {
            Teleportation();
        }
        //Wait();




        // if (currentCollider2 != null && mainCamera.transform.position.y > 2f && !coroutineCalled)
        // {
        //     newPosition = mainCamera.transform.position + mainCamera.transform.forward * 5f;

        //     currentCollider2.transform.position = newPosition;
        // }


        //DragAndDropObject();
    }
    private void SelectPart()
    {
        RaycastHit hit;
        Ray camRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(camRay, out hit, 2000f, LayerMask.GetMask("Robot", "Playing")))
        {
            currentCollider = hit.collider;
            currentCollider2 = currentCollider;
            selectedObject = hit.collider.gameObject;
            if (mainCamera.transform.position.y == 5.51f)
            {
                newPosition = mainCamera.transform.position +
                                 mainCamera.transform.forward * 0.35f - mainCamera.transform.right * 0.5f;
                currentCollider2.transform.position = newPosition;
                Vector3 rotationAngles = new Vector3(90f, 0f, 0f);
                currentCollider2.transform.rotation = Quaternion.Euler(rotationAngles);
                GameObject a = currentCollider.transform.parent.gameObject;
                selectedObject.transform.parent = parentObject.transform;
                a.gameObject.tag = free;
            }
            dragPlane = new Plane(mainCamera.transform.forward, currentCollider.transform.position);
            float planeDist;
            dragPlane.Raycast(camRay, out planeDist);
            offset = currentCollider.transform.position - camRay.GetPoint(planeDist);
        }
        currentCollider = null;
    }

    //private void DragAndDropObject()
    //{
    //    if (currentCollider == null)
    //    {
    //        return;
    //    }
    //    Ray camRay = mainCamera.ScreenPointToRay(Input.mousePosition);
    //    float planeDist;
    //    dragPlane.Raycast(camRay, out planeDist);
    //    currentCollider.transform.position = camRay.GetPoint(planeDist) + offset;

    // if (currentCollider.transform.position.y < 0.5f)
    // {
    //     currentCollider.transform.position =
    //         new Vector3(currentCollider.transform.position.x,
    //             0.5f,
    //             currentCollider.transform.position.z);
    // }

    //}

    //private void Drop()
    //{
    //    if (currentCollider == null)
    //    {
    //        return;
    //    }
    //    // currentCollider.transform.position =
    //    //     new Vector3(currentCollider.transform.position.x,
    //    //         0.5f,
    //    //         currentCollider.transform.position.z);
    //    currentCollider = null;
    //}

    private IEnumerator StepFromAbove()
    {

        yield return new WaitForSeconds(2.5f);

        initialPosition = currentCollider2.transform.position;
        initialRotation = currentCollider2.transform.rotation;

        newPosition = mainCamera.transform.position +
                                 mainCamera.transform.forward * 0.35f - mainCamera.transform.right * 0.5f;

        currentCollider2.transform.position = newPosition;
        Vector3 rotationAngles = new Vector3(70f, 0f, 0f);
        currentCollider2.transform.rotation = Quaternion.Euler(rotationAngles);

        coroutineCalled = true;
    }

    private void BackFromAbove()
    {
        currentCollider2.transform.SetPositionAndRotation(initialPosition, initialRotation);
    }


    private void Teleportation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag == free)
        {

            currentCollider2.transform.position = hit.point;
            selectedObject.layer = LayerMask.NameToLayer("Playing");
            hit.collider.gameObject.tag = busy;
            if (selectedObject.layer == LayerMask.NameToLayer("Playing"))
            {
                selectedObject = null;
                currentCollider2 = null;
            }
        }
    }
}
