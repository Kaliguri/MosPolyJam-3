using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] public bool isPlayerAttack = true;

    private ParticleSystem hitVFX;

    private float damage = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.GetComponent<EnemyTag>() == null && collision.gameObject.GetComponent<PlayerTag>() != null && !isPlayerAttack) ||
            (collision.gameObject.GetComponent<PlayerTag>() == null && collision.gameObject.GetComponent<EnemyTag>() != null && isPlayerAttack)) CombatMethods.instance.ApplayDamage(damage, collision, gameObject);
        if (!isPlayerAttack && collision.gameObject.GetComponent<PlayerTag>() != null && collision.gameObject.GetComponent<EnemyTag>() == null) { if (transform.parent != null) Destroy(transform.parent.gameObject); else Destroy(gameObject); }
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
