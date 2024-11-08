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

    private void Start()
    {
        player = FindFirstObjectByType<PlayerMovement>().gameObject.transform;
    }

    private void Update()
    {
        FollowPlayer();
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
            ShootAtPlayer();
            if (distance < minDistance)
            {
                MoveAwayFromPlayer();
            }
        }
    }

    private void ShootAtPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        bullet.transform.right = direction;
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    void MoveAwayFromPlayer()
    {
        Vector2 direction = (transform.position - player.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + direction, moveSpeed * Time.deltaTime);
    }
}
