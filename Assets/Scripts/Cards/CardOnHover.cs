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
        initialPos = card.transform.position;
        if (initialPos.x < -1.18)
            endPos = new Vector3(-1.2352185249328614f, 9.752971649169922f, 10.327946662902832f);
        else
            endPos = new Vector3(initialPos.x, initialPos.y+0.2f, initialPos.z+0.0125f);
        

        initialRot = card.transform.rotation;
        endRot = Quaternion.Euler(40, 0, 0);
        
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
