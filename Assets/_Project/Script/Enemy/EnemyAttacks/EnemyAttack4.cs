using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class EnemyAttack4 : MonoBehaviour
{
    private Transform playerTransform;
    private GameObject bulletPrefab;
    private Transform firePoint;
    private bool hasKickback;
    private float recoilForce;
    private Animator animator;
    private int spearCount;
    private float timeBetweenSpearSend;

    public float damageMinimumToHurt;

    public void Inisialise(Transform playerTransform, GameObject bulletPrefab, Transform firePoint, bool hasKickback, float recoilForce, Animator animator, int spearCount, float damageMinimumToHurt, float timeBetweenSpearSend)
    {
        this.playerTransform = playerTransform;
        this.bulletPrefab = bulletPrefab;
        this.firePoint = firePoint;
        this.hasKickback = hasKickback;
        this.recoilForce = recoilForce;
        this.animator = animator;
        this.spearCount = spearCount;
        this.damageMinimumToHurt = damageMinimumToHurt;
        this.timeBetweenSpearSend = timeBetweenSpearSend;
    }

    public void Attack4ShootAtPlayerTransform()
    {
        StartCoroutine(ShootAtPlayer());
    }

    private IEnumerator ShootAtPlayer()
    {
        for (int i = 0; i < spearCount; i++)
        {
            Vector2 direction = (playerTransform.position - firePoint.position).normalized;
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
            yield return new WaitForSeconds(timeBetweenSpearSend);
        }
    }
}
