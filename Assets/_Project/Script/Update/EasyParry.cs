using UnityEngine;

public class EasyParry : UpdateScript
{
    [SerializeField] float parryMultiplayerDecreasePercent = 0.7f;

    public override void Use()
    {
        PlayerParry.instance.perfectParryTime *= parryMultiplayerDecreasePercent;
    }
}
