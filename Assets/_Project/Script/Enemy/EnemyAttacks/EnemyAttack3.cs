using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyAttack3 : MonoBehaviour
{
    public bool isEnhancement = false;

    private float dashTime;
    private float dashForce;
    private float bodyDamage;
    private Animator animator;
    private Transform playerTransform;
    private Collider2D parentCollider;
    private GameObject sword => transform.parent.gameObject.GetComponentInChildren<SwordSpining>().gameObject;

    private Rigidbody2D rb2D => GetComponentInParent<Rigidbody2D>();
    private TrailRenderer trailRenderer => GetComponentInParent<TrailRenderer>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyTag>() == null && collision.gameObject.GetComponent<PlayerTag>() != null) CombatMethods.instance.ApplayDamage(bodyDamage, collision, gameObject);
    }

    public void Inisialise(Transform playerTransform, float dashForce, float dashTime, Animator animator, float bodyDamage, Collider2D parentCollider)
    {
        this.playerTransform = playerTransform;
        this.dashForce = dashForce;
        this.dashTime = dashTime;
        this.animator = animator;
        this.bodyDamage = bodyDamage;
        this.parentCollider = parentCollider;
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
        if (sword != null) StartCoroutine(DashToPlayer());
    }

    private IEnumerator DashToPlayer()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        parentCollider.enabled = false;
        GetComponent<Collider2D>().enabled = true;
        rb2D.linearVelocity = direction * dashForce;

        trailRenderer.emitting = true;
        //dashSound.Play2D();
        //Instantiate(dashParticle, transform.position, transform.rotation);

        yield return new WaitForSeconds(dashTime);
        rb2D.linearVelocity = new Vector3(0, 0, 0);
        parentCollider.enabled = true;
        GetComponent<Collider2D>().enabled = false;
        trailRenderer.emitting = false;

        CheckIfOutsidePlatform();
    }
}