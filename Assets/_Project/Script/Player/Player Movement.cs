using System.Collections;
using Sirenix.OdinInspector;
using Sonity;
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
    [SerializeField] ParticleSystem dashParticle;
    [ReadOnly] [SerializeField] int dashCurrentCount;

    [Title("Inputs")]
    [SerializeField] InputActionReference moveInput;
    [SerializeField] InputActionReference dashInput;

    [Title("GameObject Reference")]
    [SerializeField] TrailRenderer trailRenderer;

    [SerializeField] SoundEvent dashSound;
    [SerializeField] LayerMask platformLayerMask;

    private Vector3 lastPlatformPosition;

    private Rigidbody2D rb2D => GetComponent<Rigidbody2D>();
    private Animator animator => GetComponentInChildren<Animator>();

    private Vector2 moveVector2;
    private float currentSpeed = 0f;

    private bool canDash = true;
    [ReadOnly] public bool isDashing = false;

    void FixedUpdate()
    {
        //CheckIfOutsidePlatform();

        ReadInputActions();

        if (GetComponent<PlayerComboAttack>().isAttacking || PlayerParry.instance.isParryState) moveSpeed = 0f;
        else moveSpeed = currentSpeed;


        Move();

        RotateCharacter();

        if (dashInput.action.IsPressed() & canDash) StartCoroutine(Dash());
    }

    void Start()
    {
        currentSpeed = moveSpeed;
        dashCurrentCount = dashMaxCount;
    }

    void ReadInputActions()
    {
        moveVector2 = moveInput.action.ReadValue<Vector2>();
    }

    void Move()
    {
        rb2D.linearVelocity = moveVector2 * moveSpeed;

        if (Mathf.Abs(rb2D.linearVelocity.x) + Mathf.Abs(rb2D.linearVelocityY) != 0) animator.SetBool("IsMove", true);
        else                                                                         animator.SetBool("IsMove", false);
    }

    void RotateCharacter()
    {
        if (moveVector2 != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveVector2.y, moveVector2.x) * Mathf.Rad2Deg;
            angle = Mathf.Round(angle / 45f) * 45f - 90f; 

            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    void CheckIfOutsidePlatform()
    {
        bool isOnPlatform = Physics2D.OverlapPoint(transform.position, platformLayerMask);

        if (isOnPlatform)
        {
            lastPlatformPosition = transform.position;
        }
        else
        {
            HandleFallOffPlatform();
        }
    }

    void HandleFallOffPlatform()
    {
        /*rb2D.linearVelocity = Vector2.zero;
        canDash = false;
        moveVector2 = Vector2.zero;

        animator.SetTrigger("FallOff");

        //GetComponent<PlayerHealth>().TakeDamage(10);

        transform.position = lastPlatformPosition;

        canDash = true;*/
    }

    IEnumerator Dash()
    {
        //Debug.Log("Dash");

        canDash = false;
        isDashing = true;
        GetComponent<Collider2D>().enabled = false; 
        trailRenderer.emitting = true;

        dashSound.Play2D();
        Instantiate(dashParticle, transform.position, transform.rotation);
        rb2D.linearVelocity = moveVector2 * dashPower;

        yield return new WaitForSeconds(dashTime);

        isDashing = false;
        GetComponent<Collider2D>().enabled = true;
        trailRenderer.emitting = false;

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;

    }
}


