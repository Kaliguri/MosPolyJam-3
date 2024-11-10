using UnityEngine;

public class EnemyAttack2 : MonoBehaviour
{
    private Transform playerTransform;
    private GameObject bulletPrefab;
    private Transform firePoint;
    private bool hasKickback;
    private float recoilForce;
    private Animator animator;

    public void Inisialise(Transform playerTransform, GameObject bulletPrefab, Transform firePoint, bool hasKickback, float recoilForce, Animator animator)
    {
        this.playerTransform = playerTransform;
        this.bulletPrefab = bulletPrefab;
        this.firePoint = firePoint;
        this.hasKickback = hasKickback;
        this.recoilForce = recoilForce;
        this.animator = animator;
    }

    public void Attack2ShootAtPlayerTransform()
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

        GetComponentInParent<EnemyFollow>().lastShotTime = Time.time;
        animator.SetBool("isPreparingAttack", false);
    }
}
