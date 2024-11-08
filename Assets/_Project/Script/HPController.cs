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
                RecieveDamage(collision.gameObject.GetComponent<EnemyBullet>().GetDamage());
                Destroy(collision.gameObject);
            }
        }
        else if (gameObject.GetComponent<EnemyFollow>() != null && collision.gameObject.GetComponent<PlayerAttack>() != null)
        {
            RecieveDamage(collision.gameObject.GetComponent<PlayerAttack>().GetDamage());
        }
    }

    private void RecieveDamage(float damage)
    {
        currentHP -= damage;
        if (currentHP <= 0) { Death(); }
    }

    private void Death()
    {
        if (gameObject.GetComponent<PlayerMovement>() == null) Destroy(gameObject);
        else { Debug.Log("You Lose!"); }
    }
}
