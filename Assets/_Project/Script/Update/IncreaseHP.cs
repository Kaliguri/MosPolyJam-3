using UnityEngine;

public class IncreaseHP : UpdateScript
{
    [SerializeField] float healIncreaseMultiplayer = 1.25f;

    public override void Use()
    {
        PlayerComboAttack.instance.gameObject.GetComponent<HPController>().maxHP *= healIncreaseMultiplayer;
        PlayerComboAttack.instance.gameObject.GetComponent<HPController>().currentHP *= healIncreaseMultiplayer;
        PlayerHPBar.instance.SetMaxHP();
    }
}
