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

    public void ApplayDamage(float damage, GameObject targetType)
    {
        if (targetType.GetComponent<PlayerTag>() != null)
        {
            float _damage = damage;
            if (PlayerParry.instance.isParryState)
            {
                if (PlayerParry.instance.parryTime <= PlayerParry.instance.perfectParryTime)
                {
                    _damage = 0f;
                    PlayerParry.instance.ParryCast(true);
                }
                else
                {
                    _damage *= damageResistOnParry;
                    PlayerParry.instance.ParryCast(false);
                }
            }

            DamageNumberManager.instance.SpawnDamageText(gameObject, targetType.transform.position, _damage);

            if (_damage > 0) 
            {
                targetType.GetComponent<HPController>().RecieveDamage(_damage);

                FeelFeedbacksManager.instance.TakeDamage.PlayFeedbacks();
                if (targetType.GetComponent<HPController>().maxHP * FeelFeedbacksManager.instance.HPPercenForLowHP / 100 > targetType.GetComponent<HPController>().currentHP) FeelFeedbacksManager.instance.ActiveLowHPImage();
            }
        }
        else if (targetType.GetComponent<EnemyTag>() != null)
        {
            DamageNumberManager.instance.SpawnDamageText(gameObject, targetType.transform.position, damage);

            targetType.GetComponent<HPController>().RecieveDamage(damage);
        }
    }
}
