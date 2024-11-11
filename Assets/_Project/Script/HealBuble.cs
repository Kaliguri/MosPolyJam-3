using UnityEngine;

public class HealBuble : MonoBehaviour
{
    [SerializeField] float minRandomRadiusSpawnDistance = 1f;
    [SerializeField] float maxRandomRadiusSpawnDistance = 3f;
    [SerializeField] float speed = 5f;
    [SerializeField] float heal = 5f;
    [SerializeField] float waitTimeAtRandomTarget = 1f;

    private Transform playerTransform => PlayerComboAttack.instance.gameObject.transform;
    private Vector2 randomTargetPoint;
    private bool movingToRandomPoint = true;
    private bool movingToPlayer = false;

    void Start()
    {
        float randomDistance = Random.Range(minRandomRadiusSpawnDistance, maxRandomRadiusSpawnDistance);
        float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;

        randomTargetPoint = new Vector2(
            transform.position.x + randomDistance * Mathf.Cos(randomAngle),
            transform.position.y + randomDistance * Mathf.Sin(randomAngle)
        );
    }

    void FixedUpdate()
    {
        if (movingToRandomPoint)
        {
            MoveTowards(randomTargetPoint);

            if (Vector2.Distance(transform.position, randomTargetPoint) < 0.1f)
            {
                movingToRandomPoint = false;
                Invoke(nameof(CanMoveToPlayer), waitTimeAtRandomTarget);
            }
        }
        else if (movingToPlayer)
        {
            MoveTowards(playerTransform.position);
        }
    }

    private void CanMoveToPlayer()
    {
        movingToPlayer = true;
    }

    private void MoveTowards(Vector2 target)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        transform.position += (Vector3)(direction * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerTag>() != null)
        {
            CombatMethods.instance.ApplayHeal(heal, collision);
            Destroy(gameObject);
        }
    }
}
