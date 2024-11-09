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
    [SerializeField] Transform playerTransform;

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
        playerTransform = FindFirstObjectByType<PlayerTag>().gameObject.transform;
    }

    private void FixedUpdate()
    {
        FollowplayerTransform();
        RotateTowardsplayerTransform();
        if (attackPrepared) ShootAtplayerTransform();
    }

    void FollowplayerTransform()
    {
        float distance = Vector2.Distance(transform.position, playerTransform.position);

        if (distance > maxDistance)
        {
            MoveTowardsplayerTransform();
        }
        else
        {
            StartShootingAtplayerTransform();
            if (distance < minDistance)
            {
                MoveAwayFromplayerTransform();
            }
        }
    }

    private void StartShootingAtplayerTransform()
    {
        if (Time.time - lastShotTime >= fireRate)
        {
            animator.SetBool("isPreparingAttack", true);
        }
    }


    private void ShootAtplayerTransform()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;

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

    void RotateTowardsplayerTransform()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    void MoveTowardsplayerTransform()
    {
        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.fixedDeltaTime);
    }

    void MoveAwayFromplayerTransform()
    {
        //Vector2 direction = (transform.position - playerTransform.position).normalized;
        transform.position = -Vector2.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.fixedDeltaTime);
    }
}
