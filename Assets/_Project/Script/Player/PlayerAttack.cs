using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float damage = 10f;

    private void OnEnable()
    {
        Debug.Log("Awake" + gameObject.name);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyTag>() != null)
        {
            CombatMethods.instance.ApplayDamage(damage, collision.gameObject);
        }
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
