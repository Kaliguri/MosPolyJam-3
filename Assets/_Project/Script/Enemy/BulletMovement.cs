using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [HideInInspector] public float moveSpeed = 10f;
    [HideInInspector] public float maxDistance = 10f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.parent.position;
    }

    void Update()
    {
        MoveBullet();
    }

    void MoveBullet()
    {
        transform.Translate(moveSpeed * Time.deltaTime * Vector3.up);

        float distanceTraveled = Vector3.Distance(startPosition, transform.position);
        if (distanceTraveled >= maxDistance)
        {
            if (transform.parent != null) Destroy(transform.parent.gameObject); else Destroy(gameObject);
        }
    }
}
