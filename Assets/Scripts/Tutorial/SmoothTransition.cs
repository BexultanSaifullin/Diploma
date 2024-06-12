using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothTransition : MonoBehaviour
{
    public float speed = 5f;
    private Vector3 targetPosition;
    private bool shouldMove = false;

    public void SetTargetPosition(Vector3 newPosition)
    {
        targetPosition = newPosition;
        shouldMove = true;
    }

    void Update()
    {
        if (shouldMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if (transform.position == targetPosition)
            {
                shouldMove = false;
            }
        }
    }
}
