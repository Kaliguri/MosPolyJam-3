using Sirenix.OdinInspector;
using UnityEngine;

public class HPController : MonoBehaviour
{
    [Title("HP")]
    [SerializeField] public float MaxHP = 100f;
    public float currentHP = 100f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.GetComponent<PlayerMovement>() != null && collision.gameObject.GetComponent<EnemyBullet>() != null)
        {
            if (!gameObject.GetComponent<PlayerMovement>().isDashing)
            { 
                ApplayDamage(collision.transform.position, collision.gameObject.GetComponent<EnemyBullet>().GetDamage());
                Destroy(collision.gameObject);
            }
        }
        else if (gameObject.GetComponent<EnemyFollow>() != null && collision.gameObject.GetComponent<PlayerAttack>() != null)
        {
            ApplayDamage(collision.transform.position, collision.gameObject.GetComponent<PlayerAttack>().GetDamage());
        }
    }

    private void ApplayDamage(Vector2 collisionTransform, float damage)
    {
        DamageNumberManager.instance.SpawnDamageText(gameObject, collisionTransform, damage);
        currentHP -= damage;
        if (currentHP <= 0) { Death(); }
    }

    private void Death()
    {
        if (gameObject.GetComponent<PlayerMovement>() == null) Destroy(gameObject);
        else { Debug.Log("You Lose!"); }
    }
}
