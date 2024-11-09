using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] bool isPlayerAttack = true;

    private ParticleSystem hitVFX;

    private float damage = 10f;

    private void OnEnable()
    {
        Debug.Log("Awake" + gameObject.name);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CombatMethods.instance.ApplayDamage(damage, collision.gameObject);
        if (!isPlayerAttack && collision.gameObject.GetComponent<PlayerTag>() != null) { Destroy(gameObject); }
    }

    public float GetDamage()
    {
        return damage;
    }

    public void SetDamage(float _damage)
    {
        damage = _damage;
    }
}
