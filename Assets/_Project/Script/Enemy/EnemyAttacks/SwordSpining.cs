using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class SwordSpining : MonoBehaviour
{
    [Title("VFX")]
    [SerializeField] GameObject sword; 
    [SerializeField] ParticleSystem swordDestroyVFX; 
    private float currentZRotation = 0f;
    private float swordRotationSpeed = 5f;
    private float swordSpeedMultiplayer = 1f;
    public float swordDamage = 10f;
    private int maxParryCount = 10;
    private int parryCount = 0;
    private Animator animator;

    public void Inisialise(float swordRotationSpeed, float swordSpeedMultiplayer, int maxParryCount, float swordDamage, Animator animator)
    {
        this.swordRotationSpeed = swordRotationSpeed;
        this.swordSpeedMultiplayer = swordSpeedMultiplayer;
        this.maxParryCount = maxParryCount;
        this.swordDamage = swordDamage;
        this.animator = animator;
    }

    private void FixedUpdate()
    {
        currentZRotation += 10 * swordRotationSpeed * Time.fixedDeltaTime;

        float parentZRotation = transform.parent ? transform.parent.rotation.eulerAngles.z : 0f;
        float targetZRotation = currentZRotation - parentZRotation;

        transform.localRotation = Quaternion.Euler(0f, 0f, targetZRotation);
    }



    public void ParryedAttack()
    {
        if (parryCount < maxParryCount)
        {
            swordRotationSpeed *= -swordSpeedMultiplayer;
            parryCount++;
        }
        else 
        {
            GetComponentInParent<EnemyFollow>().isAttacking = false;
            Destroy(gameObject); 
        }
    }

    void OnDestroy()
    {
        if (gameObject.scene.isLoaded)
        Instantiate(swordDestroyVFX, sword.transform.position, sword.transform.rotation);
    }
}
