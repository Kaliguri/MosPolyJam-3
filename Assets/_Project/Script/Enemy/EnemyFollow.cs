using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [Title("Follow Settings")]
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float minDistance = 2f;
    [SerializeField] float maxDistance = 5f;

    [Title("Target")]
    [SerializeField] Transform player;

    [Title("Projectile Settings")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float fireRate = 1f;            
    [SerializeField] bool hasKickback = true;
    [EnableIf("hasKickback")] [SerializeField] float recoilForce = 0.5f;       

    private float lastShotTime = 0f;
    public bool attackPrepared = false;
    private Animator animator => GetComponent<Animator>();

    private void Start()
    {
        player = FindFirstObjectByType<PlayerMovement>().gameObject.transform;
    }

    private void FixedUpdate()
    {
        FollowPlayer();
        RotateTowardsPlayer();
        if (attackPrepared) ShootAtPlayer();
    }

    void FollowPlayer()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > maxDistance)
        {
            MoveTowardsPlayer();
        }
        else
        {
            StartShootingAtPlayer();
            if (distance < minDistance)
            {
                MoveAwayFromPlayer();
            }
        }
    }

    private void StartShootingAtPlayer()
    {
        if (Time.time - lastShotTime >= fireRate)
        {
            animator.SetBool("isPreparingAttack", true);
        }
    }


    private void ShootAtPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.transform.right = direction;

        if (hasKickback)
        {
            Vector2 recoilDirection = -direction * recoilForce;
            transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + recoilDirection, recoilForce);
        }

        lastShotTime = Time.time;
        animator.SetBool("isPreparingAttack", false);
        attackPrepared = false;
    }

    void RotateTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    void MoveTowardsPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    void MoveAwayFromPlayer()
    {
        Vector2 direction = (transform.position - player.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + direction, moveSpeed * Time.deltaTime);
    }
}
