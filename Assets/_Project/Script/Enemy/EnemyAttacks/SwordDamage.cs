using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    private float swordDamage => GetComponentInParent<SwordSpining>().swordDamage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyTag>() == null && collision.gameObject.GetComponent<PlayerTag>() != null) CombatMethods.instance.ApplayDamage(swordDamage, collision, transform.parent.gameObject);
    }
}
