using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test3DButtons : MonoBehaviour
{
    private GameObject button;
    private Vector3 initialPos;
    private Vector3 endPos;
    private float duration = 1f;
    private float startTime;
    private bool isHovering = false;

    void Start()
    {
        button = this.gameObject;
        initialPos = button.transform.position;
        endPos = new Vector3(initialPos.x, initialPos.y - 0.2f, initialPos.z - 0.15f);
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
        button.transform.position = Vector3.Lerp(button.transform.position, targetPos, easedT);
    }

    float EaseInOut(float t)
    {
        return t < 0.5f ? 2.0f * t * t : -1.0f + (4.0f - 2.0f * t) * t;
    }
}


