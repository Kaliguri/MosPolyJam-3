using System.Collections;
using UnityEngine;

public class SpearAttack2 : MonoBehaviour
{
    private Vector3 initialLocalPosition = new Vector3(0, 0, 0 );
    public GameObject enemyParent;
    private Transform playerTransform => PlayerComboAttack.instance.gameObject.transform;

    private void Start()
    {
        initialLocalPosition = enemyParent.transform.position - transform.parent.position;
    }

    private void FixedUpdate()
    {
        if (initialLocalPosition != new Vector3(0, 0, 0))
        {
            if (enemyParent.GetComponentInChildren<EnemyAttack2>() != null) 
            {
                Vector2 direction = (playerTransform.position - transform.position).normalized;
                transform.up = direction;
                transform.parent.position = enemyParent.transform.position - initialLocalPosition;
            }
            else
            {
                transform.parent.rotation = enemyParent.transform.rotation;
                Vector3 forwardOffset = enemyParent.transform.up * initialLocalPosition.magnitude;
                transform.position = enemyParent.transform.position + forwardOffset;
            }
        }
    }
}
