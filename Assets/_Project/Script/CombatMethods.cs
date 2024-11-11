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
            float _damage = damage;
            if (PlayerParry.instance.isParryState)
            {
                _damage = PlayerParry.instance.parryTime <= PlayerParry.instance.perfectParryTime ? 0f : _damage * damageResistOnParry;

                PlayerParry.instance.ParryCast(PlayerParry.instance.parryTime <= PlayerParry.instance.perfectParryTime, contact);
                PlayerSphereManager.instance.ActivateSphere(PlayerParry.instance.parryTime <= PlayerParry.instance.perfectParryTime);

                if (GameManager.instance.IsTraining) TrainingManager.instance.ParryCheck();

                if (attackingType.GetComponentInChildren<SwordSpining>() != null) attackingType.GetComponent<SwordSpining>().ParryedAttack();
            }

            

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
            bool isBlocked = false;
            if (targetType.GetComponentInChildren<EnemyAttack3>() != null)
            {
                if (targetType.GetComponentInChildren<EnemyAttack3>().playerAttackParryCount < targetType.GetComponentInChildren<EnemyAttack3>().maxPlayerAttackParryCount)
                {
                    isBlocked = true;
                    targetType.GetComponentInChildren<EnemyAttack3>().playerAttackParryCount++;
                }
            }
            if (!isBlocked)
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
}
