using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [Title("Follow Settings")]
    [SerializeField] float stanTime = 1f;
    [SerializeField] float enemyMoveSpeed = 3f;
    [SerializeField] float teleportCooldown = 2f;
    //[SerializeField] float rotationSpeed = 100f;
    [SerializeField] float minDistanceFromPlayer = 2f;
    [SerializeField] float maxDistanceFromPlayer = 5f;
    [SerializeField] float attackRangeDistance = 5f;

    [Title("Enemy Settings")]
    [SerializeField] EnemyType enemyType = EnemyType.BodyRush;

    [Title("Attack Settings")]
    [SerializeField] float timeBetweenAttack = 1f;
    [HideIf("@enemyType != EnemyType.BodyRush")] [SerializeField] float dashForce = 5f;
    [HideIf("@enemyType != EnemyType.BodyRush")] [SerializeField] float dashTime = 5f;
    [HideIf("@enemyType != EnemyType.BodyRush")] [SerializeField] float bodyDamage = 5f;
    [HideIf("@enemyType == EnemyType.BodyRush")] [SerializeField] GameObject attackPrefab;
    [HideIf("@enemyType == EnemyType.BodyRush")] [SerializeField] private float bulletMoveSpeed = 10f;
    [HideIf("@enemyType == EnemyType.BodyRush")] [SerializeField] private float bulletMaxDistance = 10f;
    [HideIf("@enemyType == EnemyType.BodyRush")] [SerializeField] float bulletDamage = 5f;
    [HideIf("@enemyType == EnemyType.BodyRush")] [SerializeField] Transform firePoint;
    [SerializeField] bool hasKickback = true;
    [EnableIf("hasKickback")] [SerializeField] float recoilForce = 0.5f;
    private Animator animator => GetComponentInChildren<Animator>();

    [HideInInspector] public float lastShotTime = 0f;
    private Transform playerTransform;
    private float lastTeleportTime = -Mathf.Infinity;
    private EnemyTeleporter currentTeleportCollider;

    private enum EnemyType
    {
        BodyRush = 1,
        RangeSpear = 2,
        SpiningSword = 4,
        MeleeSpear = 5
    }

    private void Start()
    {
        playerTransform = FindFirstObjectByType<PlayerTag>().gameObject.transform;
        var attackComponent = attackPrefab.GetComponent<Attack>() ?? attackPrefab.GetComponentInChildren<Attack>();
        if (attackComponent != null)
        {
            attackComponent.SetDamage(enemyType == EnemyType.BodyRush ? bodyDamage : bulletDamage);
        }
        var bulletMovement = attackPrefab.GetComponent<BulletMovement>() ?? attackPrefab.GetComponentInChildren<BulletMovement>();
        if (bulletMovement != null)
        {
            bulletMovement.moveSpeed = bulletMoveSpeed;
            bulletMovement.maxDistance = bulletMaxDistance;
        }

        InstantiateAttack();
    }

    private void FixedUpdate()
    {
        FollowplayerTransform();
        RotateTowardsplayerTransform();

        if (currentTeleportCollider != null && CanTeleport())
        {
            Vector2 targetPosition = currentTeleportCollider.linkedTeleport.GetNearestPoint(transform.position);
            TeleportTo(targetPosition);
            SetTeleportCooldown(teleportCooldown);
        }
    }

    private void InstantiateAttack()
    {
        switch ((int)enemyType)
        {
            case 1:
                if (GetComponentInChildren<EnemyAttack1>() != null) { GetComponentInChildren<EnemyAttack1>().Inisialise(playerTransform, dashForce, dashTime, animator, bodyDamage); }
                break;
            case 2:
                if (GetComponentInChildren<EnemyAttack2>() != null) { GetComponentInChildren<EnemyAttack2>().Inisialise(playerTransform, attackPrefab, firePoint, hasKickback, recoilForce, animator); }
                break;
        }
    }

    public void Die()
    {
        CombatMethods.instance.ApplayDamage(999f, GetComponent<Collider2D>(), gameObject);
    }

    public bool CanTeleport()
    {
        return Time.time >= lastTeleportTime + teleportCooldown;
    }

    public void SetTeleportCooldown(float cooldown)
    {
        lastTeleportTime = Time.time;
        teleportCooldown = cooldown;
    }

    public void TeleportTo(Vector2 position)
    {
        transform.position = position;
    }

    public void SetCurrentTeleport(EnemyTeleporter collider)
    {
        currentTeleportCollider = collider;
    }

    public void ClearCurrentTeleport()
    {
        currentTeleportCollider = null;
    }

    void FollowplayerTransform()
    {
        float distance = Vector2.Distance(transform.position, playerTransform.position);

        if (distance > maxDistanceFromPlayer)
        {
            MoveTowardsPlayer();
        }
        else
        {
            if (distance < minDistanceFromPlayer)
            {
                MoveAwayFromPlayer();
            }
        }
        if (attackRangeDistance >= distance && Time.time - lastShotTime >= timeBetweenAttack)
            StartShootingAtplayerTransform();
    }

    public void StartStan()
    {
        Invoke(nameof(StopStan), stanTime);
    }

    private void StopStan()
    {
        animator.SetBool("isStaned", false);
    }

    private void StartShootingAtplayerTransform()
    {
        animator.SetBool("isPreparingAttack", true);
    }

    public void Attack()
    {
        switch ((int)enemyType)
        {
            case 1:
                if (GetComponentInChildren<EnemyAttack1>() != null) { GetComponentInChildren<EnemyAttack1>().Attack1DashToPlayer(); }
                break;
            case 2:
                if (GetComponentInChildren<EnemyAttack2>() != null) { GetComponentInChildren<EnemyAttack2>().Attack2ShootAtPlayerTransform(); }
                break;
        }
    }

    void RotateTowardsplayerTransform()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
         
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        if (Vector2.Distance(transform.position, playerTransform.position) > 0.05f) transform.rotation = Quaternion.Euler(0, 0, angle);


        /*float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        float currentAngle = transform.rotation.eulerAngles.z;

        float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0, 0, newAngle);*/
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        if (Vector2.Distance(transform.position, playerTransform.position) > 0.05f) transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + direction, enemyMoveSpeed * Time.fixedDeltaTime);
    }

    void MoveAwayFromPlayer()
    {
        Vector2 direction = (transform.position - playerTransform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + direction, enemyMoveSpeed * Time.fixedDeltaTime);
    }
}
