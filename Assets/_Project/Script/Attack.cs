using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] bool isPlayerAttack = true;

    private ParticleSystem hitVFX;

    private float damage = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.GetComponent<EnemyTag>() == null && !isPlayerAttack) || (collision.gameObject.GetComponent<PlayerTag>() == null && isPlayerAttack)) CombatMethods.instance.ApplayDamage(damage, collision);
        if (!isPlayerAttack && collision.gameObject.GetComponent<PlayerTag>() != null && collision.gameObject.GetComponent<EnemyTag>() == null) { Destroy(gameObject); }
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
