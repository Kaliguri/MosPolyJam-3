using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Title("Settings")]
    [Header("Move")]
    [SerializeField] float moveSpeed = 5f;

    [Header("Dash")]
    [SerializeField] float dashPower;
    [SerializeField] float dashTime;
    [SerializeField] float dashCooldown;
    [SerializeField] int dashMaxCount;
    [ReadOnly] [SerializeField] int dashCurrentCount;

    [Title("Inputs")]
    [SerializeField] InputActionReference moveInput;
    [SerializeField] InputActionReference dashInput;

    [Title("GameObject Reference")]
    [SerializeField] TrailRenderer trailRenderer;


    private Rigidbody2D rb2D => GetComponent<Rigidbody2D>();

    private Vector2 moveVector2;

    private bool canDash = true;
    private bool isDashing = false;

    void FixedUpdate()
    {
        ReadInputActions();

        Move();

        if (dashInput.action.IsPressed() & canDash) StartCoroutine(Dash());
    }

    void Start()
    {
        dashCurrentCount = dashMaxCount;
    }

    void ReadInputActions()
    {
        moveVector2 = moveInput.action.ReadValue<Vector2>();
    }

    void Move()
    {
        rb2D.linearVelocity = moveVector2 * moveSpeed;
    }

    IEnumerator Dash()
    {
        Debug.Log("Dash");

        canDash = false;
        isDashing = true;
        trailRenderer.emitting = true;


        rb2D.linearVelocity = moveVector2 * dashPower;

        yield return new WaitForSeconds(dashTime);

        isDashing = false;
        trailRenderer.emitting = false;

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;

    }
}


