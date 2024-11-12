using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyBecomeInvinsible : MonoBehaviour
{
    public bool isEnhancement = false;

    private float dashTime;
    private float dashForce;
    private float dashCooldown;
    private Animator animator;
    private Transform playerTransform;
    private Collider2D parentCollider;

    private TrailRenderer trailRenderer => GetComponentInParent<TrailRenderer>();
    private Rigidbody2D rb2D => GetComponentInParent<Rigidbody2D>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Attack>() != null && collision.gameObject.GetComponent<Attack>().isPlayerAttack) StartCoroutine(DashRightFromPlayer());
    }

    public void Inisialise(Transform playerTransform, float dashForce, float dashTime, float dashCooldown, Animator animator, Collider2D parentCollider)
    {
        this.playerTransform = playerTransform;
        this.dashForce = dashForce;
        this.dashCooldown = dashCooldown;
        this.dashTime = dashTime;
        this.animator = animator;
        this.parentCollider = parentCollider;
        GetComponent<Collider2D>().enabled = true;
    }

    private IEnumerator DashRightFromPlayer()
    {
        isEnhancement = true;
        GetComponent<Collider2D>().enabled = false;

        var enemyFollow = parentCollider?.gameObject.GetComponentInChildren<EnemyFollow>();
        if (enemyFollow != null)
        {
            var dashRightFromPlayer = enemyFollow.gameObject.GetComponentInChildren<DashRightFromPlayer>();
            if (dashRightFromPlayer != null)
            {
                dashRightFromPlayer.SetVisability(false);
            }
        }

        Invoke(nameof(CanDashAgain), dashCooldown);
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        Vector2 directionLeft = new Vector2(-direction.y, direction.x);
        parentCollider.enabled = false;
        rb2D.linearVelocity = directionLeft * dashForce;

        trailRenderer.emitting = true;
        //dashSound.Play2D();
        //Instantiate(dashParticle, transform.position, transform.rotation);

        yield return new WaitForSeconds(dashTime);

        trailRenderer.emitting = false;
        rb2D.linearVelocity = new Vector3(0, 0, 0);
        parentCollider.enabled = true;

        CheckIfOutsidePlatform();
    }

    private void CanDashAgain()
    {
        GetComponent<Collider2D>().enabled = true;
        var enemyFollow = parentCollider?.gameObject?.GetComponentInChildren<EnemyFollow>();
        if (enemyFollow != null)
        {
            var dashRightFromPlayer = enemyFollow.gameObject?.GetComponentInChildren<DashRightFromPlayer>();
            if (dashRightFromPlayer != null)
            {
                dashRightFromPlayer.SetVisability(true);
            }
        }
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
}
