using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [Title("Follow Settings")]
    [SerializeField] float enemyMoveSpeed = 3f;
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

    private float lastShotTime = 0f;
    private Animator animator => GetComponentInChildren<Animator>();

    private void Start()
    {
        playerTransform = FindFirstObjectByType<PlayerTag>().gameObject.transform;
        bulletPrefab.GetComponent<EnemyBullet>().damage = damage;
        bulletPrefab.GetComponent<BulletMovement>().moveSpeed = bulletMoveSpeed;
        bulletPrefab.GetComponent<BulletMovement>().maxDistance = bulletMaxDistance;
    }

    private void FixedUpdate()
    {
        FollowplayerTransform();
        RotateTowardsplayerTransform();
    }

    void FollowplayerTransform()
    {
        float distance = Vector2.Distance(transform.position, playerTransform.position);

        if (distance > maxDistanceFromPlayer)
        {
            MoveTowardsplayerTransform();
        }
        else
        {
            StartShootingAtplayerTransform();
            if (distance < minDistanceFromPlayer)
            {
                MoveAwayFromplayerTransform();
            }
        }
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

    void MoveTowardsplayerTransform()
    {
        Vector2 direction = (-transform.position + playerTransform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, transform.position + (Vector3)direction, enemyMoveSpeed * Time.fixedDeltaTime);
    }

    void MoveAwayFromplayerTransform()
    {
        Vector2 direction = (transform.position - playerTransform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, transform.position + (Vector3)direction, enemyMoveSpeed * Time.fixedDeltaTime);
    }
}
