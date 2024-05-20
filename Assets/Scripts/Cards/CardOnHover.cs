using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardOnHover : MonoBehaviour
{
    private GameObject card;
    private Vector3 initialPos;
    private Vector3 endPos;
    private Quaternion initialRot;
    private Quaternion endRot;
    private float duration = 1f;
    private float startTime;
    private bool isHovering = false;
    private Camera mainCamera;
    private bool wasAbove12 = false; // ќтслеживание, была ли камера выше 12

    void Start()
    {
        mainCamera = Camera.main;
        card = this.gameObject;
        UpdateInitialTransform();
    }

    void UpdateInitialTransform()
    {
        //UnityEditor.TransformWorldPlacementJSON:{ "position":{ "x":0.41832149028778078,"y":9.8119535446167,"z":10.030875205993653},"rotation":{ "x":0.3332860767841339,"y":-0.08283728361129761,"z":0.037537459284067157,"w":0.9384291768074036},"scale":{ "x":8.0,"y":8.0,"z":8.0} }        //UnityEditor.TransformWorldPlacementJSON:{ "position":{ "x":0.715182900428772,"y":9.772233009338379,"z":10.078919410705567},"rotation":{ "x":0.3256419003009796,"y":-0.1577540636062622,"z":0.010006037540733815,"w":0.9321861267089844},"scale":{ "x":8.0,"y":8.0,"z":8.0} }
        //UnityEditor.TransformWorldPlacementJSON:{ "position":{ "x":0.1319352388381958,"y":9.820001602172852,"z":10.018628120422364},"rotation":{ "x":0.3383420407772064,"y":-0.014642869122326374,"z":0.06222004443407059,"w":0.9388498067855835},"scale":{ "x":8.0,"y":8.0,"z":8.0} }
        initialPos = card.transform.position;
        //UnityEditor.TransformWorldPlacementJSON:{ "position":{ "x":-1.2352185249328614,"y":9.752971649169922,"z":10.327946662902832},"rotation":{ "x":0.34202009439468386,"y":0.0,"z":0.0,"w":0.9396926760673523},"scale":{ "x":7.999999523162842,"y":7.999999523162842,"z":7.999999523162842} }
        if (initialPos.x < -1.18)
            endPos = new Vector3(-1.2352185249328614f, 9.752971649169922f, 10.327946662902832f);
        else
            endPos = new Vector3(initialPos.x, initialPos.y+0.2f, initialPos.z+0.0125f);
        

        initialRot = card.transform.rotation;
        //if (initialPos.y > 10.13)
            endRot = Quaternion.Euler(40, 0, 0);
        //else
        //    endRot = Quaternion.Euler(initialRot.eulerAngles.x, 0, -7);
        //if (initialPos.x > 1.7)
        //    endRot = Quaternion.Euler(initialRot.eulerAngles.x, 0, 10);
    }

    void OnMouseEnter()
    {
        if (mainCamera.transform.position.y <= 12)
        {
            isHovering = true;
            startTime = Time.time;
        }
    }

    void OnMouseExit()
    {
        if (mainCamera.transform.position.y <= 12)
        {
            isHovering = false;
            startTime = Time.time;
        }
    }

    void Update()
    {
        
        if (mainCamera.transform.position.y > 12)
        {
            wasAbove12 = true;
            return; // ѕрекратить выполнение скрипта, если камера выше 12
        }

        if (wasAbove12)
        {
            UpdateInitialTransform(); // ѕерезаписать начальные позиции и повороты
            wasAbove12 = false;
        }

        Vector3 targetPos = isHovering ? endPos : initialPos;
        Quaternion targetRot = isHovering ? endRot : initialRot;

        float t = (Time.time - startTime) / duration;
        t = Mathf.Clamp01(t);
        float easedT = EaseInOut(t);

        card.transform.position = Vector3.Lerp(card.transform.position, targetPos, easedT);
        card.transform.rotation = Quaternion.Slerp(card.transform.rotation, targetRot, easedT);

        // ѕроверка на нажатие пробела и возврат к начальным позици€м
        if (Input.GetKeyDown(KeyCode.Space))
        {
            card.transform.position = initialPos;
            card.transform.rotation = initialRot;
            isHovering = false;
            startTime = Time.time;
        }
    }

    float EaseInOut(float t)
    {
        return t < 0.5f ? 2.0f * t * t : -1.0f + (4.0f - 2.0f * t) * t;
    }
}
