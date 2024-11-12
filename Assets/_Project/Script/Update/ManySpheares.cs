using UnityEngine;

public class ManySpheares : UpdateScript
{
    [SerializeField] int spheresIncreaseCount = 2;

    public override void Use()
    {
        PlayerSphereManager.instance.sphereCount += spheresIncreaseCount;
    }
}
