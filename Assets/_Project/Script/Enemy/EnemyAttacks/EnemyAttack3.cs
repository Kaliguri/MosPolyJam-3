using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyAttack3 : MonoBehaviour
{
    private float dashTime;
    private float dashForce;
    private float bodyDamage;
    private Animator animator;
    private Transform playerTransform;
    private Collider2D parentCollider;

    public int maxPlayerAttackParryCount = 1;
    public int playerAttackParryCount = 0;

    private Rigidbody2D rb2D => GetComponentInParent<Rigidbody2D>();
    private TrailRenderer trailRenderer => GetComponentInParent<TrailRenderer>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyTag>() == null && collision.gameObject.GetComponent<PlayerTag>() != null) CombatMethods.instance.ApplayDamage(bodyDamage, collision, gameObject);
    }

    public void Inisialise(Transform playerTransform, float dashForce, float dashTime, Animator animator, float bodyDamage, Collider2D parentCollider, int maxPlayerAttackParryCount)
    {
        this.playerTransform = playerTransform;
        this.dashForce = dashForce;
        this.dashTime = dashTime;
        this.animator = animator;
        this.bodyDamage = bodyDamage;
        this.parentCollider = parentCollider;
        this.maxPlayerAttackParryCount = maxPlayerAttackParryCount;
        GetComponent<Collider2D>().enabled = false;
    }

    void CheckIfOutsidePlatform()
    {
        foreach (Tilemap tilemap in TileMapController.instance.tilemapList)
        {
            if (!tilemap.gameObject.transform.parent.gameObject.activeSelf) continue;

            Vector3Int tile = tilemap.WorldToCell(transform.position);
            if (tilemap.HasTile(tile)) return;
        }

        HandleFallOffPlatform();
    }

    void HandleFallOffPlatform()
    {
        animator.SetBool("isFalling", true);
    }

    public void Attack3DashToPlayer()
    {
        StartCoroutine(DashToPlayer());
    }

    private IEnumerator DashToPlayer()
    {
        GetComponentInParent<EnemyFollow>().isAttacking = true;
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        parentCollider.enabled = false;
        GetComponent<Collider2D>().enabled = true;
        rb2D.linearVelocity = direction * dashForce;

        trailRenderer.emitting = true;
        //dashSound.Play2D();
        //Instantiate(dashParticle, transform.position, transform.rotation);

        yield return new WaitForSeconds(dashTime);
        GetComponentInParent<EnemyFollow>().isAttacking = false;
        rb2D.linearVelocity = new Vector3(0, 0, 0);
        parentCollider.enabled = true;
        GetComponent<Collider2D>().enabled = false;
        trailRenderer.emitting = false;

        GetComponentInParent<EnemyFollow>().lastShotTime = Time.time;

        CheckIfOutsidePlatform();
    }
}
