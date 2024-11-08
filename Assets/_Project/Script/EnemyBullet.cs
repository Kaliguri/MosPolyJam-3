using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [Title("Damage")]
    [SerializeField] float damage = 5f;

    public float GetDamage()
    {
        return damage;
    }
}
