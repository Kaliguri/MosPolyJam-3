using UnityEngine;

public class Respawn : UpdateScript
{
    public override void Use()
    {
        PlayerComboAttack.instance.gameObject.GetComponent<HPController>().canRespawn = true;
    }
}
