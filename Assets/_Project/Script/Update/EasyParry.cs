using UnityEngine;

public class EasyParry : UpdateScript
{
    private float parryMultiplayerDecreasePercent = 1.3f;

    public override void Use()
    {
        PlayerParry.instance.perfectParryTime *= parryMultiplayerDecreasePercent;
    }
}
