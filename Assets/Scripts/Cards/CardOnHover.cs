using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CardOnHover : MonoBehaviour
{
    private GameObject card;
    private Vector3 initialPos;
    private Vector3 endPos;
    private float duration = 1f;
    private float startTime;
    private bool isHovering = false;

    void Start()
    {
        card = this.gameObject;
        initialPos = card.transform.position;
        endPos = new Vector3(initialPos.x, initialPos.y + 0.387128593f, initialPos.z + 0.1f);
        //Vector3(0.205125228,1.27400005,-0.954999983)
        //Vector3(0.205125228,0.386871457,-0.415932655)
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

        float t = (Time.time - startTime) / duration;
        t = Mathf.Clamp01(t);
        float easedT = EaseInOut(t);
        card.transform.position = Vector3.Lerp(card.transform.position, targetPos, easedT);
    }

    float EaseInOut(float t)
    {
        return t < 0.5f ? 2.0f * t * t : -1.0f + (4.0f - 2.0f * t) * t;
    }
}


