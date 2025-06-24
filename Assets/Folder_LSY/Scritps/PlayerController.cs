using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector3 targetPos;
    private bool isMoving = false;

    private void Update()
    {
        HandleInput();
        HandleMovement();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f;
            targetPos = Camera.main.ScreenToWorldPoint(mousePos);
            targetPos.z = 0f;
            isMoving = true;
        }
    }

    private void HandleMovement()
    {
        if (!isMoving) return;

        Vector3 direction = (targetPos - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, targetPos);

        if (distance > 0.1f)
        {
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
        else
        {
            isMoving = false;
        }
    }
}
