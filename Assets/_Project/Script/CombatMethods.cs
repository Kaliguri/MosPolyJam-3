using Unity.VisualScripting;
using UnityEngine;

public class CombatMethods : MonoBehaviour
{
    [SerializeField] float damageResistOnParry = 0.4f;
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
            float _damage = CalculateDamage(damage, contact, attackingType);

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
            if (!IsAttackBlocked(targetType, damage))
            {
                DamageNumberManager.instance.SpawnDamageText(gameObject, targetType.transform.position, damage);

                targetType.GetComponent<HPController>().RecieveDamage(damage);

                Animator animator = targetType.GetComponentInChildren<Animator>();
                if (animator != null)
                {
                    animator.SetBool("isStaned", true);
                }
                if (targetType.TryGetComponent<EnemyFollow>(out var enemyFollow))
                {
                    enemyFollow.StartStan();
                }
            }
        }
    }

    private float CalculateDamage(float damage, Vector2 contact, GameObject attackingType)
    {
        float _damage = damage;

        if (PlayerParry.instance.isParryState)
        {
            _damage = PlayerParry.instance.parryTime <= PlayerParry.instance.perfectParryTime ? 0f : _damage * damageResistOnParry;

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

    private bool IsAttackBlocked(GameObject targetType, float damage)
    {
        var enemyAttack3 = targetType.GetComponentInChildren<EnemyAttack3>();
        var enemyAttack4 = targetType.GetComponentInChildren<EnemyAttack4>();

        if (enemyAttack3 != null)
        {
            if (enemyAttack3.playerAttackParryCount < enemyAttack3.maxPlayerAttackParryCount)
            {
                enemyAttack3.playerAttackParryCount++;
                return true;
            }
        }
        else if (enemyAttack4 != null)
        {
            if (damage <= enemyAttack4.damageMinimumToHurt)
            {
                return true;
            }
        }

        return false;
    }

}
