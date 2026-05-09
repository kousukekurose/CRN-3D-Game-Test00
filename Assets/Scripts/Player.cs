using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody rb;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // ŠO•”iPlayerControllerj‚©‚ç“ü—Í‚ğó‚¯æ‚é
    public void SetMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector3 moveDir = new Vector3(moveInput.x, 0, moveInput.y);

        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
    }
}