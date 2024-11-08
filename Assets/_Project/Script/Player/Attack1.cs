using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class Attack1 : MonoBehaviour
{
    [Title("AttackParametrs")]
    [SerializeField] private float moveDistance = 5f; 
    [SerializeField] private float moveSpeed = 10f;

    private Vector3 targetPosition;

    private void OnEnable()
    {
        Debug.Log("Attack1!");
        targetPosition = transform.position + transform.up * moveDistance;

        StartCoroutine(MoveForward());
    }

    private IEnumerator MoveForward()
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
