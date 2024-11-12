using UnityEngine;

public class SpeadSpetial : UpdateScript
{
    [SerializeField] float spesialAttackMultiplayerDecreasePercent = 0.65f;

    public override void Use()
    {
        PlayerComboAttack.instance.longPressThreshold *= spesialAttackMultiplayerDecreasePercent;
    }
}
