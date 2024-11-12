using UnityEngine;

public class EnemyAttack2 : MonoBehaviour
{
    [SerializeField] ParticleSystem cursedApplayVFX;
    [SerializeField] GameObject curseTriangleObject;
    [SerializeField] GameObject curseTrianglePrefab;


    private Transform playerTransform;
    private GameObject bulletPrefab;
    private Transform firePoint;
    private float curseTime;
    private float timeBeforeCurse;
    private bool hasKickback;
    private float recoilForce;
    private Animator animator;
    private GameObject bullet;
    private bool isCursing;

    private void OnDestroy()
    {
        if (Application.isPlaying && isCursing && gameObject.scene.isLoaded) 
        {
            Instantiate(cursedApplayVFX, curseTriangleObject.transform.position, Quaternion.identity);

            var CurseTriangle = Instantiate(curseTrianglePrefab, curseTriangleObject.transform.position, Quaternion.identity);
            CurseTriangle.GetComponent<Animator>().enabled = true;
            Destroy(CurseTriangle, 1f);

            playerTransform.gameObject.GetComponent<PlayerComboAttack>().BecomeCursed(timeBeforeCurse, curseTime);
        }
    }

    public void Inisialise(Transform playerTransform, GameObject bulletPrefab, Transform firePoint, bool hasKickback, float recoilForce, Animator animator, float curseTime, float timeBeforeCurse, bool isCursing)
    {
        this.playerTransform = playerTransform;
        this.bulletPrefab = bulletPrefab;
        this.firePoint = firePoint;
        this.hasKickback = hasKickback;
        this.recoilForce = recoilForce;
        this.animator = animator;
        this.curseTime = curseTime;
        this.timeBeforeCurse = timeBeforeCurse;
        this.isCursing = isCursing;
    }

    public void Attack2ShootAtPlayerTransform()
    {
        Vector2 direction = (playerTransform.position - bullet.transform.position).normalized;
        bullet.GetComponentInChildren<SpearAttack2>().enemyParent = transform.parent.gameObject;
        bullet.GetComponentInChildren<Collider2D>().enabled = true;
        bullet.GetComponentInChildren<BulletMovement>().enabled = true;
        bullet.GetComponentInChildren<SpearAttack2>().enabled = false;
        bullet.GetComponentInChildren<Animator>().enabled = false;

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

        GetComponentInParent<EnemyFollow>().SetLastShotTime();
        animator.SetBool("isPreparingAttack", false);
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
        Destroy(bullet);
    }
}
