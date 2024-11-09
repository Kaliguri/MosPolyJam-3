using UnityEngine;

public class CombatMethods : MonoBehaviour
{
    public static CombatMethods instance = null;

    private void Awake()
    {
        if (instance == null) { instance = this; }
    }

    public void ApplayDamage(float damage, GameObject targetType)
    {
        if (targetType.GetComponent<PlayerTag>() != null)
        {
            if (PlayerParry.instance.isParryState)
            {

            }
            else
            {

            }

            DamageNumberManager.instance.SpawnDamageText(gameObject, targetType.transform.position, damage);

            targetType.GetComponent<HPController>().RecieveDamage(damage);

            FeelFeedbacksManager.instance.TakeDamage.PlayFeedbacks();
            if (targetType.GetComponent<HPController>().maxHP * FeelFeedbacksManager.instance.HPPercenForLowHP / 100 > targetType.GetComponent<HPController>().currentHP) FeelFeedbacksManager.instance.ActiveLowHPImage();
        }
        else if (targetType.GetComponent<EnemyTag>() != null)
        {
            DamageNumberManager.instance.SpawnDamageText(gameObject, targetType.transform.position, damage);

            targetType.GetComponent<HPController>().RecieveDamage(damage);
        }
    }
}
