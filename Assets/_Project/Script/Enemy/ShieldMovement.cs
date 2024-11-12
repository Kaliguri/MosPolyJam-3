using System.Collections;
using UnityEngine;

public class ShieldMovement : MonoBehaviour
{
    [SerializeField] float resetTime = 1f;
    private Vector3 startPosition;
    private Coroutine coroutine;
    private TrailRenderer trailRenderer => GetComponent<TrailRenderer>();

    private void Awake()
    {
        startPosition = transform.localPosition;
        //trailRenderer.emitting = false;

    }

    public void SetTargetPoint(Vector3 targetPoint, bool isBlocked)
    {
        //trailRenderer.emitting = true;
        transform.position = targetPoint;
        //trailRenderer.emitting = false;

        if (coroutine != null) { StopCoroutine(coroutine); }
        coroutine = StartCoroutine(ResetTargetPosition(isBlocked));
    }

    private IEnumerator ResetTargetPosition(bool isBlocked)
    {
        if (!isBlocked) 
        {
            BreakShield();
        }
        yield return new WaitForSeconds(resetTime);

        //trailRenderer.emitting = true;
        transform.localPosition = startPosition;
        //trailRenderer.emitting = false;

    }

    private void BreakShield()
    {
        StopCoroutine(coroutine);
        Destroy(gameObject);
    }
}
