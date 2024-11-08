using Sirenix.OdinInspector;
using System;
using UnityEngine;
using VFolders.Libs;

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
                collision.gameObject.Destroy();
            }
        }
    }

    private void RecieveDamage(float damage)
    {
        currentHP -= damage;
        if (currentHP < 0) { Death(); }
    }

    private void Death()
    {
        if (gameObject.GetComponent<PlayerMovement>() == null) gameObject.Destroy();
        else { Debug.Log("You Lose!"); }
    }
}
