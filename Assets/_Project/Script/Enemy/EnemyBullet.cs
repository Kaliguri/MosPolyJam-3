using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [HideInInspector] public float damage = 5f;

    public float GetDamage()
    {
        return damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>() != null)
        {
            if (!collision.gameObject.GetComponent<PlayerMovement>().isDashing)
            {
                CombatMethods.instance.ApplayDamage(damage, collision, gameObject);
                Destroy(gameObject);
            }
        }
    }
}
