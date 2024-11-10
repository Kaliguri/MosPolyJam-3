using System.Collections;
using UnityEngine;

public class EnemyAttack1 : MonoBehaviour
{
    private float bodyDamage;
    private float dashSpeed;
    private float dashForce;
    private GameObject bodyAura;
    private GameObject attackAura;
    private Transform playerTransform;

    public void Inisialise(float bodyDamage, float dashSpeed, GameObject bodyAura, Transform playerTransform, float dashForce)
    {
        this.bodyDamage = bodyDamage;
        this.dashSpeed = dashSpeed;
        this.playerTransform = playerTransform;
        this.bodyAura = bodyAura;
        this.dashForce = dashForce;
    }

    private void FixedUpdate()
    {
        if (attackAura != null)
        {
            attackAura.transform.position = transform.parent.position;
            attackAura.transform.rotation = transform.parent.rotation;
        }
    }

    public void Attack1DashToPlayer()
    {
        if (attackAura != null) Destroy(attackAura);
        attackAura = Instantiate(bodyAura,transform.parent.position, transform.parent.rotation);
        DashToPlayer();
    }

    private void DashToPlayer()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;

        Vector2 recoilDirection = direction * dashForce;
        Vector2 targetPosition = (Vector2)transform.position + recoilDirection.normalized;

        if (TryGetComponent<Rigidbody2D>(out var rb))
        {
            if (!Physics2D.OverlapPoint(targetPosition))
            {
                rb.MovePosition(targetPosition);
            }
        }
    }
}
