using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [Title("Follow Settings")]
    [SerializeField] float moveSpeed = 3f;
    //[SerializeField] float rotationSpeed = 100f;
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
    private Animator animator => GetComponentInChildren<Animator>();

    private void Start()
    {
        playerTransform = FindFirstObjectByType<PlayerTag>().gameObject.transform;
    }

    private void FixedUpdate()
    {
        FollowplayerTransform();
        RotateTowardsplayerTransform();
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


    public void ShootAtplayerTransform()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        Debug.Log("SpawnSpear");
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.transform.up = direction;

        if (hasKickback)
        {
            Vector2 recoilDirection = -direction * recoilForce;
            transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + recoilDirection, recoilForce);
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
        transform.position = Vector2.MoveTowards(transform.position, transform.position + (Vector3)direction, moveSpeed * Time.fixedDeltaTime);
    }

    void MoveAwayFromplayerTransform()
    {
        Vector2 direction = (transform.position - playerTransform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, transform.position + (Vector3)direction, moveSpeed * Time.fixedDeltaTime);
    }
}
