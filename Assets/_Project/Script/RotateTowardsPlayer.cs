using UnityEngine;

public class RotateTowardsPlayer : MonoBehaviour
{
    private Transform playerTransform;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTransform = FindFirstObjectByType<PlayerMovement>().gameObject.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RotateTowardPlayer();
    }

    void RotateTowardPlayer()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
