using System.Collections;
using UnityEngine;

public class EnemyAttack1 : MonoBehaviour
{
    private float bodyDamage;
    private float damageDashSpeed;
    private GameObject bodyAura;

    public void Instantiate(float bodyDamage, float damageDashSpeed, GameObject bodyAura)
    {
        this.bodyDamage = bodyDamage;
        this.damageDashSpeed = damageDashSpeed;

        this.bodyAura = Instantiate(bodyAura);
        this.bodyAura.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (bodyAura != null)
        {
            bodyAura.transform.position = transform.parent.position;
            bodyAura.transform.rotation = transform.parent.rotation;
        }
    }

    public void Attack1DashToPlayer()
    {
        bodyAura.SetActive(true);
        StartCoroutine(DashToPlayer());
    }

    private IEnumerator DashToPlayer()
    {
        yield return new WaitForSeconds(1f);
        bodyAura.SetActive(false);
    }
}
