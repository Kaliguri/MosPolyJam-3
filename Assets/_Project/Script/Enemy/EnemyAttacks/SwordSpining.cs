using UnityEngine;

public class SwordSpining : MonoBehaviour
{
    private float swordRotationSpeed = 0f;
    private float swordSpeedMultiplayer = 1f;
    private int maxParryCount = 10;
    public int parryCount = 0;

    public void Inisialise(float swordRotationSpeed, float swordSpeedMultiplayer, int maxParryCount)
    {
        this.swordRotationSpeed = swordRotationSpeed;
        this.swordSpeedMultiplayer = swordSpeedMultiplayer;
        this.maxParryCount = maxParryCount;
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
