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
    [SerializeField] float attackRangeDistance = 5f;

    [Title("Enemy Settings")]
    [SerializeField] EnemyType enemyType = EnemyType.BodyRush;

    [Title("Attack Settings")]
    [SerializeField] float timeBetweenAttack = 1f;
    //[HideIfGroup("@enemyType != EnemyType.BodyRush && enemyType != EnemyType.SpiningSword")]
    [HideIf("@enemyType != EnemyType.BodyRush && enemyType != EnemyType.SpiningSword")] [SerializeField] float bodyDamage = 5f;
    [HideIf("@enemyType != EnemyType.BodyRush && enemyType != EnemyType.SpiningSword")] [SerializeField] float attackDashForce = 5f;
    [HideIf("@enemyType != EnemyType.BodyRush && enemyType != EnemyType.SpiningSword")] [SerializeField] float attackDashTime = 5f;
    [HideIf("@enemyType != EnemyType.BodyRush")] [SerializeField] float extraDashForce = 50f;
    [HideIf("@enemyType != EnemyType.BodyRush")] [SerializeField] float extraDashTime = 0.1f;
    [HideIf("@enemyType != EnemyType.BodyRush")] [SerializeField] float extraDashCooldown = 2f;
    [HideIf("@enemyType != EnemyType.RangeSpear && enemyType != EnemyType.MeleeSpear")] [SerializeField] Transform firePoint;
    [HideIf("@enemyType != EnemyType.RangeSpear && enemyType != EnemyType.MeleeSpear")] [SerializeField] float bulletDamage = 5f;
    [HideIf("@enemyType != EnemyType.RangeSpear && enemyType != EnemyType.MeleeSpear")] [SerializeField] GameObject attackPrefab;
    [HideIf("@enemyType != EnemyType.RangeSpear && enemyType != EnemyType.MeleeSpear")] [SerializeField] float bulletMoveSpeed = 10f;
    [HideIf("@enemyType != EnemyType.RangeSpear && enemyType != EnemyType.MeleeSpear")] [SerializeField] float bulletMaxDistance = 10f;
    [HideIf("@enemyType != EnemyType.RangeSpear && enemyType != EnemyType.MeleeSpear")] [SerializeField] bool hasKickback = true;
    [HideIf("@enemyType != EnemyType.RangeSpear && enemyType != EnemyType.MeleeSpear")] [EnableIf("hasKickback")] [SerializeField] float recoilForce = 0.5f;
    [HideIf("@enemyType != EnemyType.RangeSpear")] [SerializeField] float animationAttackSpeedMultiplier = 1.5f;
    [HideIf("@enemyType != EnemyType.RangeSpear")] [SerializeField] float curseTime = 2f;
    [HideIf("@enemyType != EnemyType.RangeSpear")] [SerializeField] float timeBeforeCurse = 2f;
    [HideIf("@enemyType != EnemyType.SpiningSword")] [SerializeField] float swordDamage = 5f;
    [HideIf("@enemyType != EnemyType.SpiningSword")] [SerializeField] float swordRotationSpeed = 5f;
    [HideIf("@enemyType != EnemyType.SpiningSword")] [SerializeField] float swordSpeedMultiplayer = 1.25f;
    [HideIf("@enemyType != EnemyType.SpiningSword")] [SerializeField] int maxCanBlockPlayerAttackCount = 5;
    [HideIf("@enemyType != EnemyType.SpiningSword")] [SerializeField] int maxSwordCanSurviveParryCount = 5;
    [HideIf("@enemyType != EnemyType.MeleeSpear")] [SerializeField] int spearCount = 3;
    [HideIf("@enemyType != EnemyType.MeleeSpear")] [SerializeField] float timeBetweenSpearSend = 0.5f;
    [HideIf("@enemyType != EnemyType.MeleeSpear")] [SerializeField] float damageMinimumToHurt = 100f;

    private Animator animator => GetComponentInChildren<Animator>();
    private TrailRenderer trailRenderer => GetComponent<TrailRenderer>();

    [HideInInspector] public float lastShotTime = 0f;
    private bool isStaned = false;
    private Transform playerTransform => PlayerComboAttack.instance.gameObject.transform;
    private float lastTeleportTime = -Mathf.Infinity;
    private EnemyTeleporter currentTeleportCollider;

    private enum EnemyType
    {
        BodyRush = 1,
        RangeSpear = 2,
        SpiningSword = 3,
        MeleeSpear = 4
    }

    private void Start()
    {
        InstantiateAttack();
    }

    private void FixedUpdate()
    {
        if (!isStaned) FollowplayerTransform();
        RotateTowardsplayerTransform();

        if (currentTeleportCollider != null && CanTeleport())
        {
            Vector2 targetPosition = currentTeleportCollider.linkedTeleport.GetNearestPoint(transform.position);
            trailRenderer.emitting = true;
            TeleportTo(targetPosition);
            SetTeleportCooldown(teleportCooldown);
        }
    }

    [Button("Instantiate Attack")]
    private void InstantiateAttack()
    {
        if (attackPrefab != null) 
        {
            var attackComponent = attackPrefab.GetComponent<Attack>() ?? attackPrefab.GetComponentInChildren<Attack>();
            if (attackComponent != null)
            {
                attackComponent.SetDamage(enemyType == EnemyType.BodyRush ? bodyDamage : bulletDamage);
            }
            var bulletMovement = attackPrefab.GetComponent<BulletMovement>() ?? attackPrefab.GetComponentInChildren<BulletMovement>();
            if (bulletMovement != null)
            {
                bulletMovement.moveSpeed = bulletMoveSpeed;
                bulletMovement.maxDistance = bulletMaxDistance;
            }
        }

        switch ((int)enemyType)
        {
            case 1:
                if (GetComponentInChildren<EnemyAttack1>() != null) { GetComponentInChildren<EnemyAttack1>().Inisialise(attackDashForce, attackDashTime, animator, bodyDamage, GetComponent<PolygonCollider2D>()); }
                if (GetComponentInChildren<EnemyBecomeInvinsible>() != null) { GetComponentInChildren<EnemyBecomeInvinsible>().Inisialise(playerTransform, extraDashForce, extraDashTime, extraDashCooldown, animator, GetComponent<PolygonCollider2D>()); }
                break;
            case 2:
                if (GetComponentInChildren<EnemyAttack2>() != null) { GetComponentInChildren<EnemyAttack2>().Inisialise(playerTransform, attackPrefab, firePoint, hasKickback, recoilForce, animator, curseTime, timeBeforeCurse); }
                break;
            case 3:
                if (GetComponentInChildren<EnemyAttack3>() != null) { GetComponentInChildren<EnemyAttack3>().Inisialise(playerTransform, attackDashForce, attackDashTime, animator, bodyDamage, GetComponent<PolygonCollider2D>(), maxCanBlockPlayerAttackCount); }
                if (GetComponentInChildren<SwordSpining>() != null) { GetComponentInChildren<SwordSpining>().Inisialise(swordRotationSpeed, swordSpeedMultiplayer, maxSwordCanSurviveParryCount, swordDamage, animator); }
                break;
            case 4:
                if (GetComponentInChildren<EnemyAttack4>() != null) { GetComponentInChildren<EnemyAttack4>().Inisialise(playerTransform, attackPrefab, firePoint, hasKickback, recoilForce, animator, spearCount, damageMinimumToHurt, timeBetweenSpearSend); }
                break;
        }
    }

    public void Die()
    {
        CombatMethods.instance.ApplayDamage(999f, GetComponent<Collider2D>(), gameObject);
    }

    private bool CanTeleport()
    {
        return Time.time >= lastTeleportTime + teleportCooldown && !isStaned;
    }

    public void SetTeleportCooldown(float cooldown)
    {
        lastTeleportTime = Time.time;
        teleportCooldown = cooldown;
    }

    private void TeleportTo(Vector2 position)
    {
        transform.position = position;
        Invoke(nameof(StopTrail), 0.5f);
    }

    private void StopTrail()
    {
        trailRenderer.emitting = false;
    }

    public void SetCurrentTeleport(EnemyTeleporter collider)
    {
        currentTeleportCollider = collider;
    }

    public void ClearCurrentTeleport()
    {
        currentTeleportCollider = null;
    }

    private void FollowplayerTransform()
    {
        float distance = Vector2.Distance(transform.position, playerTransform.position);

        if (distance > maxDistanceFromPlayer)
        {
            MoveTowardsPlayer();
        }
        else
        {
            if (distance < minDistanceFromPlayer)
            {
                MoveAwayFromPlayer();
            }
        }
        if (attackRangeDistance >= distance && Time.time - lastShotTime >= timeBetweenAttack)
            StartShootingAtplayerTransform();
    }

    public void StartStan()
    {
        isStaned = true;
        Invoke(nameof(StopStan), stanTime);
    }

    private void StopStan()
    {
        isStaned = false;
        animator.SetBool("isStaned", false);
    }

    private void StartShootingAtplayerTransform()
    {
        if (GetComponentInChildren<SwordSpining>() == null) animator.SetBool("isPreparingAttack", true);
        if (GetComponentInChildren<EnemyBecomeInvinsible>() != null && GetComponentInChildren<EnemyBecomeInvinsible>().isEnhancement)
        {
            animator.speed = animationAttackSpeedMultiplier;
        }
        else
        {
            animator.speed = 1f;
        }
    }

    public void Attack()
    {
        switch ((int)enemyType)
        {
            case 1:
                if (GetComponentInChildren<EnemyAttack1>() != null) { GetComponentInChildren<EnemyAttack1>().Attack1DashToPlayer(); }
                break;
            case 2:
                if (GetComponentInChildren<EnemyAttack2>() != null) { GetComponentInChildren<EnemyAttack2>().Attack2ShootAtPlayerTransform(); }
                break;
            case 3:
                if (GetComponentInChildren<EnemyAttack3>() != null) { GetComponentInChildren<EnemyAttack3>().Attack3DashToPlayer(); }
                break;
            case 4:
                if (GetComponentInChildren<EnemyAttack4>() != null) { GetComponentInChildren<EnemyAttack4>().Attack4ShootAtPlayerTransform(); }
                break;
        }
    }

    void RotateTowardsplayerTransform()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
         
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        if (Vector2.Distance(transform.position, playerTransform.position) > 0.05f) transform.rotation = Quaternion.Euler(0, 0, angle);


        /*float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        float currentAngle = transform.rotation.eulerAngles.z;

        float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0, 0, newAngle);*/
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        if (Vector2.Distance(transform.position, playerTransform.position) > 0.05f) transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + direction, enemyMoveSpeed * Time.fixedDeltaTime);
    }

    void MoveAwayFromPlayer()
    {
        Vector2 direction = (transform.position - playerTransform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + direction, enemyMoveSpeed * Time.fixedDeltaTime);
    }
}
