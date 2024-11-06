using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Title("Settings")]
    [SerializeField] float moveSpeed = 5f;

    [Title("Inputs")]

    [SerializeField] InputActionReference moveInput;
    [SerializeField] InputActionReference dashInput;


    private Rigidbody2D rb2D => GetComponent<Rigidbody2D>();

    private Vector2 moveVector2;

    void FixedUpdate()
    {
        ReadInputActions();

        Move();
    }

    void Move()
    {
        rb2D.linearVelocity = moveVector2 * moveSpeed;
    }

    void ReadInputActions()
    {
        moveVector2 = moveInput.action.ReadValue<Vector2>();
    }
}
