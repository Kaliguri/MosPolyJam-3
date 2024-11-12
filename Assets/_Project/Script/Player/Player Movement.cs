using System.Collections;
using Sirenix.OdinInspector;
using Sonity;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [Title("Settings")]
    [SerializeField] float fallDamage = 5f;

    [Header("Move")]
    public float moveSpeed = 5f;

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
    [ReadOnly] public bool isFalling = false;
    [ReadOnly] public bool isDashing = false;
    [ReadOnly] public bool canMove = true;

    void FixedUpdate()
    {
        if (!isFalling) CheckIfOutsidePlatform();

        ReadInputActions();

        if (GetComponent<PlayerComboAttack>().isAttacking || PlayerParry.instance.isParryState || isFalling || !canMove || PlayerComboAttack.instance.isCursed) moveSpeed = 0f;
        else moveSpeed = currentSpeed;


        Move();

        if (!PlayerParry.instance.isParryState && canMove && !PlayerComboAttack.instance.isCursed) RotateCharacter();

        if (dashInput.action.IsPressed() && canDash && !PlayerComboAttack.instance.isCursed && !isFalling) 
            //if (Mathf.Abs(rb2D.linearVelocity.x) + Mathf.Abs(rb2D.linearVelocityY) != 0)
                StartCoroutine(Dash());
    }

    void Start()
    {
        lastPlatformPosition = transform.position;
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
        foreach (Tilemap tilemap in TileMapController.instance.tilemapList)
        {
            if (!tilemap.gameObject.transform.parent.gameObject.activeSelf) continue;

            Vector3Int tile = tilemap.WorldToCell(transform.position);
            if (tilemap.HasTile(tile)) return; 
        }

        HandleFallOffPlatform();
    }

    void HandleFallOffPlatform()
    {
        isFalling = true;
        animator.SetBool("isFalling", true);
        CombatMethods.instance.ApplayDamage(fallDamage, GetComponent<Collider2D>(), gameObject);
    }

    public void StopFalling()
    {
        if (GameManager.instance.IsTraining) 
       {
            if (TrainingManager.instance.currentMission == 2)
            {
                DamageNumberManager.instance.SpawnFallInTrainingText(gameObject.transform.position);
            }
            TrainingManager.instance.NextPart(1);
       }
       
        transform.position = lastPlatformPosition;
        animator.SetBool("isFalling", false);
        isFalling = false;

       
    }

    IEnumerator Dash()
    {
        //Debug.Log("Dash");

        if (GameManager.instance.IsTraining) TrainingManager.instance.NextPart(0);

        canDash = false;
        isDashing = true;
        lastPlatformPosition = transform.position;
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


