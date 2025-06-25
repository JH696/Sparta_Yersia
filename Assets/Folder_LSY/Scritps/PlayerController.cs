using UnityEngine;

public class PlayerController : BaseCharacter
{
    [SerializeField] private float moveSpeed = 5f;

    private Vector3 targetPos;
    private bool isMoving = false;

    private void Update()
    {
        HandleInput();
        HandleMovement();

        // 테스트용: 키 입력 시 데미지 입거나 회복
        if (Input.GetKeyDown(KeyCode.H))  // H 누르면 힐 10
        {
            Heal(10f);
            Debug.Log($"힐 받음: 현재 체력 {CurrentHp}/{MaxHp}");
        }
        if (Input.GetKeyDown(KeyCode.J))  // J 누르면 데미지 20
        {
            TakeDamage(20f);
            Debug.Log($"데미지 입음: 현재 체력 {CurrentHp}/{MaxHp}");
        }
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
