using UnityEngine;
using System.Collections;

public class Attack1 : MonoBehaviour
{
    [SerializeField] private float moveDistance = 5f;
    [SerializeField] private float moveSpeed = 10f;

    private Vector3 attack1TargetPosition;

    private void OnEnable()
    {
        attack1TargetPosition = transform.position + transform.up * moveDistance;

        StartCoroutine(MoveAttack1Forward());
    }

    private IEnumerator MoveAttack1Forward()
    {
        while (Vector3.Distance(transform.position, attack1TargetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, attack1TargetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
