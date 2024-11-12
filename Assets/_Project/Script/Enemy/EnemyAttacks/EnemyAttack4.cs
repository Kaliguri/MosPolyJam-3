using DamageNumbersPro.Demo;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class EnemyAttack4 : MonoBehaviour
{
    [Title("VFX")]

    public ParticleSystem parryVFX;
    public ParticleSystem shieldDestoyVFX;
    public Animator shieldAnimator;



    private Transform playerTransform;
    private GameObject bulletPrefab;
    private Transform firePoint;
    private bool hasKickback;
    private float recoilForce;
    private Animator animator;
    private int spearCount;
    private float timeBetweenSpearSend;
    private GameObject bullet;
    private Coroutine coroutine;

    [ReadOnly] public float damageMinimumToHurt;
    [ReadOnly] public bool hasShield = true;

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
        coroutine = StartCoroutine(ShootAtPlayer());
    }

    private IEnumerator ShootAtPlayer()
    {
        if (bullet != null)
        {
            bullet.GetComponentInChildren<SpearAttack2>().enabled = false;
            bullet.GetComponentInChildren<Animator>().enabled = false;
            bullet.GetComponentInChildren<Collider2D>().enabled = true;
            bullet.GetComponentInChildren<BulletMovement>().enabled = true;
        }
        yield return new WaitForSeconds(timeBetweenSpearSend);

        GetComponentInParent<EnemyFollow>().isAttacking = true;
        for (int i = 0; i < spearCount - 1; i++)
        {
            Vector2 direction = (playerTransform.position - firePoint.position).normalized;
            GameObject _bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            _bullet.transform.up = direction;
            _bullet.GetComponentInChildren<SpearAttack2>().enabled = false;
            _bullet.GetComponentInChildren<Animator>().enabled = false;
            Color newColor = _bullet.GetComponentInChildren<SpriteRenderer>().color;
            newColor.a = 1f;
            _bullet.GetComponentInChildren<SpriteRenderer>().color = newColor;
            _bullet.GetComponentInChildren<Collider2D>().enabled = true;
            _bullet.GetComponentInChildren<BulletMovement>().enabled = true;

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

            animator.SetBool("isPreparingAttack", false);
            yield return new WaitForSeconds(timeBetweenSpearSend);
        }
        GetComponentInParent<EnemyFollow>().SetLastShotTime();
        GetComponentInParent<EnemyFollow>().isAttacking = false;
    }

    public void PrepareAttack2()
    {
        GetComponentInParent<EnemyFollow>().isAttacking = true;
        bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponentInChildren<SpearAttack2>().enemyParent = transform.parent.gameObject;
        bullet.GetComponentInChildren<Collider2D>().enabled = false;
        Color newColor = bullet.GetComponentInChildren<SpriteRenderer>().color;
        newColor.a = 0f;
        bullet.GetComponentInChildren<SpriteRenderer>().color = newColor;
    }

    public void StopAttack()
    {
        if (coroutine != null) StopCoroutine(coroutine);
        Destroy(bullet);
    }

    public void BreakShield()
    {
        if (hasShield)
        {
            hasShield = false;
            Debug.Log("Break Shield!");
            
            Instantiate(shieldDestoyVFX, gameObject.transform.position, Quaternion.identity);
            shieldAnimator.SetTrigger("ShieldDestroy");
        }
    }
}
