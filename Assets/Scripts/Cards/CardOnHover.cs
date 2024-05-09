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
    private bool wasAbove12 = false; // Отслеживание, была ли камера выше 12

    void Start()
    {
        mainCamera = Camera.main;
        card = this.gameObject;
        UpdateInitialTransform();
    }

    void UpdateInitialTransform()
    {
        initialPos = card.transform.position;
        if (initialPos.x < 1.7728264331817628)
            endPos = new Vector3(initialPos.x, 10.57f, 25.4f);
        else
            endPos = new Vector3(1.86f, 10.65f, 25.4f);

        initialRot = card.transform.rotation;
        if (initialPos.y > 10.13)
            endRot = Quaternion.Euler(initialRot.eulerAngles.x, 0, 0);
        else
            endRot = Quaternion.Euler(initialRot.eulerAngles.x, 0, -7);
        if (initialPos.x > 1.7)
            endRot = Quaternion.Euler(initialRot.eulerAngles.x, 0, 10);
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
            return; // Прекратить выполнение скрипта, если камера выше 12
        }

        if (wasAbove12)
        {
            UpdateInitialTransform(); // Перезаписать начальные позиции и повороты
            wasAbove12 = false;
        }

        Vector3 targetPos = isHovering ? endPos : initialPos;
        Quaternion targetRot = isHovering ? endRot : initialRot;

        float t = (Time.time - startTime) / duration;
        t = Mathf.Clamp01(t);
        float easedT = EaseInOut(t);

        card.transform.position = Vector3.Lerp(card.transform.position, targetPos, easedT);
        card.transform.rotation = Quaternion.Slerp(card.transform.rotation, targetRot, easedT);
    }

    float EaseInOut(float t)
    {
        return t < 0.5f ? 2.0f * t * t : -1.0f + (4.0f - 2.0f * t) * t;
    }
}
