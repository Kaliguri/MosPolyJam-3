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

    [Title("Bullet Settings")]
    [SerializeField] private float bulletMoveSpeed = 10f;
    [SerializeField] private float bulletMaxDistance = 10f;
    [SerializeField] float damage = 5f;

    [Title("Target")]
    [SerializeField] Transform playerTransform;

    [Title("Projectile Settings")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float timeBetweenShoot = 1f;            
    [SerializeField] bool hasKickback = true;
    [EnableIf("hasKickback")] [SerializeField] float recoilForce = 0.5f;
    private Animator animator => GetComponentInChildren<Animator>();

    [HideInInspector] public float lastShotTime = 0f;
    private float lastTeleportTime = -Mathf.Infinity;
    private EnemyTeleporter currentTeleportCollider;

    private void Start()
    {
        playerTransform = FindFirstObjectByType<PlayerTag>().gameObject.transform;
        var attackComponent = bulletPrefab.GetComponent<Attack>() ?? bulletPrefab.GetComponentInChildren<Attack>();
        if (attackComponent != null)
        {
            attackComponent.SetDamage(damage);
        }
        var bulletMovement = bulletPrefab.GetComponent<BulletMovement>() ?? bulletPrefab.GetComponentInChildren<BulletMovement>();
        if (bulletMovement != null)
        {
            bulletMovement.moveSpeed = bulletMoveSpeed;
            bulletMovement.maxDistance = bulletMaxDistance;
        }
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
            StartShootingAtplayerTransform();
            if (distance < minDistanceFromPlayer)
            {
                MoveAwayFromPlayer();
            }
        }
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
        if (Time.time - lastShotTime >= timeBetweenShoot)
        {
            animator.SetBool("isPreparingAttack", true);
        }
    }


    public void ShootAtplayerTransform()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.transform.up = direction;

        if (hasKickback)
        {
            Vector2 recoilDirection = -direction.normalized * recoilForce;
            Vector2 targetPosition = (Vector2)transform.position + recoilDirection;

            if (TryGetComponent<Rigidbody2D>(out var rb))
            {
                if (!Physics2D.OverlapPoint(targetPosition))
                {
                    rb.MovePosition(targetPosition);
                }
            }
        }

        lastShotTime = Time.time;
        animator.SetBool("isPreparingAttack", false);
    }

    void RotateTowardsplayerTransform()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
         
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        
        transform.rotation = Quaternion.Euler(0, 0, angle);


        /*float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        float currentAngle = transform.rotation.eulerAngles.z;

        float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0, 0, newAngle);*/
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + direction, enemyMoveSpeed * Time.fixedDeltaTime);
    }

    void MoveAwayFromPlayer()
    {
        Vector2 direction = (transform.position - playerTransform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + direction, enemyMoveSpeed * Time.fixedDeltaTime);
    }
}
