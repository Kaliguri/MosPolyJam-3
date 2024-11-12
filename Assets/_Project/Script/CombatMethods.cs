using Unity.VisualScripting;
using UnityEngine;

public class CombatMethods : MonoBehaviour
{
    [SerializeField] float damageResistOnParry = 0.4f;
    [HideInInspector] public float healFromParry = 0f;
    public static CombatMethods instance = null;

    private void Awake()
    {
        if (instance == null) { instance = this; }
    }

    public void ApplayDamage(float damage, Collider2D collisionTargetType, GameObject attackingType)
    {
        var targetType = collisionTargetType.gameObject;
        var contact = collisionTargetType.ClosestPoint(attackingType.transform.position);

        if (targetType.GetComponent<PlayerTag>() != null)
        {
            float _damage = CalculateDamage(damage, contact, attackingType, collisionTargetType);

            if (_damage > 0) 
            {
                DamageNumberManager.instance.SpawnDamageText(gameObject, targetType.transform.position, _damage);

                targetType.GetComponent<HPController>().RecieveDamage(_damage);

                if (!PlayerParry.instance.isParryState)FeelFeedbacksManager.instance.TakeDamage.PlayFeedbacks();
                if (targetType.GetComponent<HPController>().maxHP * FeelFeedbacksManager.instance.HPPercenForLowHP / 100 > targetType.GetComponent<HPController>().currentHP) FeelFeedbacksManager.instance.ActiveLowHPImage();
            }

            else
            {
                DamageNumberManager.instance.SpawnParryText(gameObject, targetType.transform.position);
            }
        }
        else if (targetType.GetComponent<EnemyTag>() != null)
        {
            if (!IsAttackBlocked(targetType, damage, contact))
            {
                DamageNumberManager.instance.SpawnDamageText(gameObject, targetType.transform.position, damage);

                targetType.GetComponent<HPController>().RecieveDamage(damage);

                if (targetType.TryGetComponent<EnemyFollow>(out var enemyFollow))
                {
                    enemyFollow.GetHit();
                    enemyFollow.StartStan();
                }

                Animator animator = targetType.GetComponent<EnemyFollow>().animator;
                if (animator != null)
                {
                    animator.SetBool("isStaned", true);
                }
            }
            else
            {
                DamageNumberManager.instance.SpawnDamageText(gameObject, targetType.transform.position, 0);
                var enemyAttack3 = targetType.GetComponentInChildren<EnemyAttack3>();
                var enemyAttack4 = targetType.GetComponentInChildren<EnemyAttack4>();
                if (enemyAttack3 != null)
                {
                    //Debug.Log("third enemy get 0 damage");
                    var parryVFX = enemyAttack3.parryVFX;
                    Instantiate(parryVFX, contact, Quaternion.identity);
                    
                }
                else if (enemyAttack4 != null)
                {
                    //Debug.Log("fourth enemy get 0 damage");
                    var parryVFX = enemyAttack4.parryVFX;
                    Instantiate(parryVFX, contact, Quaternion.identity);

                    enemyAttack4.shieldAnimator.SetTrigger("Parry");
                    
                }
            }
        }
    }

    public void ApplayHeal(float heal, Collider2D collisionTargetType)
    {
        var targetType = collisionTargetType.gameObject;
        targetType.GetComponent<HPController>().GetHeal(heal);
        
        DamageNumberManager.instance.SpawnHealText(gameObject, targetType.transform.position, heal);

        if (targetType.GetComponent<HPController>().maxHP * FeelFeedbacksManager.instance.HPPercenForLowHP / 100 <= targetType.GetComponent<HPController>().currentHP) FeelFeedbacksManager.instance.DeactiveLowHPImage();

    }

    private float CalculateDamage(float damage, Vector2 contact, GameObject attackingType, Collider2D collisionTargetType)
    {
        float _damage = damage;

        if (PlayerParry.instance.isParryState)
        {
            _damage = PlayerParry.instance.parryTime <= PlayerParry.instance.perfectParryTime ? 0f : _damage * damageResistOnParry;

            if (_damage == 0 && healFromParry > 0) ApplayHeal(healFromParry, collisionTargetType);

            PlayerParry.instance.ParryCast(PlayerParry.instance.parryTime <= PlayerParry.instance.perfectParryTime, contact);
            PlayerSphereManager.instance.ActivateSphere(PlayerParry.instance.parryTime <= PlayerParry.instance.perfectParryTime);

            if (GameManager.instance.IsTraining)
            {
                TrainingManager.instance.ParryCheck();
            }

            if (attackingType.GetComponentInChildren<SwordSpining>() != null)
            {
                attackingType.GetComponent<SwordSpining>().ParryedAttack();
            }
        }

        return _damage;
    }

    private bool IsAttackBlocked(GameObject targetType, float damage, Vector2 contact)
    {
        var enemyAttack2 = targetType.GetComponentInChildren<EnemyBecomeInvinsible>();
        var enemyAttack3 = targetType.GetComponentInChildren<EnemyAttack3>();
        var enemyAttack4 = targetType.GetComponentInChildren<EnemyAttack4>();

        if (enemyAttack2 != null)
        {
            if (enemyAttack2.gameObject.GetComponent<Collider2D>().enabled && damage != 999f) return true;
        }
        else if (enemyAttack3 != null)
        {
            if (enemyAttack3.playerAttackParryCount < enemyAttack3.maxPlayerAttackParryCount && damage != 999f)
            {
                enemyAttack3.BlockAttack();
                return true;
            }
        }
        else if (enemyAttack4 != null)
        {
            if (damage <= enemyAttack4.damageMinimumToHurt && enemyAttack4.hasShield)
            {
                return true;
            }
            else if (enemyAttack4.hasShield)
            {
                enemyAttack4.BreakShield();
            }
        }

        return false;
    }
}
