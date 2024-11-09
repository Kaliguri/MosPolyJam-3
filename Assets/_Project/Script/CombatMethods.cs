using Sirenix.OdinInspector;
using System;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Cinemachine.CinemachineFreeLookModifier;

public class CombatMethods : MonoBehaviour
{
    public static CombatMethods instance = null;

    private void Awake()
    {
        if (instance == null) { instance = this; }
    }

    public void ApplayDamage(float value, GameObject targetType)
    {
        targetType.GetComponent<HPController>().RecieveDamage(value);

        if (targetType.GetComponent<PlayerMovement>() != null)
        {
            /*FeelFeedbacksManager.instance.TakeDamage.PlayFeedbacks();

            if (targetType.GetComponent<HPController>().GetMaxHP() * FeelFeedbacksManager.instance.HPPercenForLowHP / 100 > targetType.GetComponent<HPController>().GetCurrentHP())
                FeelFeedbacksManager.instance.ActiveLowHPImage();*/
        }
        else if (targetType.GetComponent<EnemyTag>() != null)
        {
            //реакция на то что враг получил урон
        }
    }
}
