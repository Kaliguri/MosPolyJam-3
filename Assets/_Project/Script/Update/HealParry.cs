using UnityEngine;

public class HealParry : UpdateScript
{
    [SerializeField] float healFromParry = 5f;

    public override void Use()
    {
        CombatMethods.instance.healFromParry = healFromParry;
    }
}
