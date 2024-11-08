using Sirenix.OdinInspector;
using System;
using UnityEngine;
using VFolders.Libs;

public class HPController : MonoBehaviour
{
    [Title("HP")]
    [SerializeField] private float HP = 100f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.GetComponent<PlayerMovement>() != null && collision.gameObject.GetComponent<EnemyBullet>() != null)
        {
            RecieveDamage(collision.gameObject.GetComponent<EnemyBullet>().GetDamage());
        }
    }

    private void RecieveDamage(float damage)
    {
        HP -= damage;
        if (HP < 0) { Death(); }
    }

    private void Death()
    {
        if (gameObject.GetComponent<PlayerMovement>() == null) gameObject.Destroy();
        else { Debug.Log("You Lose!"); }
    }
}
