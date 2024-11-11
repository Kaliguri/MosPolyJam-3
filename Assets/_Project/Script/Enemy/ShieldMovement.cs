using System.Collections;
using UnityEngine;

public class ShieldMovement : MonoBehaviour
{
    [SerializeField] float shieldSpeed = 10f; 
    [SerializeField] float resetTime = 1f;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Coroutine coroutine;
    private bool hasTarget = false;

    private void Awake()
    {
        startPosition = transform.localPosition;
        targetPosition = transform.localPosition;
    }

    public void SetTargetPoint(Vector3 targetPoint, bool isBlocked)
    {
        hasTarget = true;
        targetPosition = targetPoint;
        if (coroutine != null) { StopCoroutine(coroutine); }
        coroutine = StartCoroutine(ResetTargetPosition(isBlocked));
    }

    private void FixedUpdate()
    {
        if (hasTarget) MoveTowards(targetPosition);
    }

    private void MoveTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        transform.position += shieldSpeed * Time.fixedDeltaTime * direction;
    }

    private IEnumerator ResetTargetPosition(bool isBlocked)
    {
        if (!isBlocked) 
        {
            BreakShield();
        }
        yield return new WaitForSeconds(resetTime);
        hasTarget = false;
        transform.localPosition = startPosition;
    }

    private void BreakShield()
    {
        StopCoroutine(coroutine);
        Destroy(gameObject);
    }
}
