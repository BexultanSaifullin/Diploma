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

    void Start()
    {
        card = this.gameObject;
        initialPos = card.transform.position;
        if(initialPos.x < 1.7728264331817628)
            endPos = new Vector3(initialPos.x, 10.56f, 25.4f);
        else
            endPos = new Vector3(1.86f, 10.65f, 25.4f);
        initialRot = card.transform.rotation;
        if (initialPos.y > 10.13)
            endRot = Quaternion.Euler(initialRot.eulerAngles.x, 0, 0);
        else
            endRot = Quaternion.Euler(initialRot.eulerAngles.x, 0, -7);
        if (initialPos.x > 1.7)
            endRot = Quaternion.Euler(initialRot.eulerAngles.x, 0, 10);
        
       // UnityEditor.TransformWorldPlacementJSON:{ "position":{ "x":1.7728264331817628,"y":10.17051887512207,"z":25.689359664916993},"rotation":{ "x":0.30471137166023257,"y":-0.3006044626235962,"z":-0.043754637241363528,"w":0.9027034044265747},"scale":{ "x":8.0,"y":8.0,"z":8.0} }
        //UnityEditor.TransformWorldPlacementJSON:{ "position":{ "x":-0.669552206993103,"y":10.003175735473633,"z":25.891765594482423},"rotation":{ "x":0.334067165851593,"y":0.3508438467979431,"z":0.18826524913311006,"w":0.854320764541626},"scale":{ "x":8.0,"y":8.0,"z":8.0} }        // Начальные данные скрипта
        //Vector3(2.16791201, 0.0184975974, 0.0340436287)    
    }

    void OnMouseEnter()
    {
        isHovering = true;
        startTime = Time.time;
    }

    void OnMouseExit()
    {
        isHovering = false;
        startTime = Time.time;
    }

    void Update()
    {
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
