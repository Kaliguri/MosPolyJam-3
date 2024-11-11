using System.Collections;
using UnityEngine;

public class SpearAttack2 : MonoBehaviour
{
    private Vector3 initialLocalPosition;
    public GameObject enemyParent;
    private Transform playerTransform => PlayerComboAttack.instance.gameObject.transform;

    private void Start()
    {
        initialLocalPosition = enemyParent.transform.position - transform.parent.position;
    }

    private void FixedUpdate()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        transform.up = direction;
        transform.parent.position = enemyParent.transform.position - initialLocalPosition;
    }
}
