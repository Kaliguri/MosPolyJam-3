using UnityEngine;

public class EnemyTeleporter : MonoBehaviour
{
    [SerializeField] public EnemyTeleporter linkedTeleport;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<EnemyTag>() != null && linkedTeleport != null)
        {
            EnemyFollow enemy = other.GetComponent<EnemyFollow>();

            if (enemy != null)
            {
                enemy.SetCurrentTeleport(this);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<EnemyTag>() != null)
        {
            EnemyFollow enemy = other.GetComponent<EnemyFollow>();

            if (enemy != null)
            {
                enemy.ClearCurrentTeleport();
            }
        }
    }

    public Vector2 GetNearestPoint(Vector2 position)
    {
        Collider2D col = GetComponent<Collider2D>();
        return col.ClosestPoint(position);
    }
}
