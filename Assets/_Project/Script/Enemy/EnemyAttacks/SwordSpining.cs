using UnityEngine;

public class SwordSpining : MonoBehaviour
{
    private float swordRotationSpeed = 0f;
    private float swordSpeedMultiplayer = 1f;
    private float swordDamage = 10f;
    private int maxParryCount = 10;
    public int parryCount = 0;

    public void Inisialise(float swordRotationSpeed, float swordSpeedMultiplayer, int maxParryCount, float swordDamage)
    {
        this.swordRotationSpeed = swordRotationSpeed;
        this.swordSpeedMultiplayer = swordSpeedMultiplayer;
        this.maxParryCount = maxParryCount;
        this.swordDamage = swordDamage;
    }

    private void FixedUpdate()
    {
        transform.Rotate(0f, 0f, swordRotationSpeed * Time.fixedDeltaTime);
    }

    public void ParryedAttack()
    {
        if (parryCount < maxParryCount)
        {
            swordRotationSpeed = -swordRotationSpeed * swordSpeedMultiplayer;
            parryCount++;
        }
        else Destroy(gameObject);
    }
}
