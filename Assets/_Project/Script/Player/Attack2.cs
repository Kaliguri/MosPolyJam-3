using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class Attack2 : MonoBehaviour
{
    [Title("Attack Parameters")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float attack2_Angle = 30f; 

    private Transform playerTransform;
    private float startAngleZ = 0f;
    private Vector3 initialOffset;

    private void OnEnable()
    {
        playerTransform = FindFirstObjectByType<PlayerMovement>().transform;
        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - attack2_Angle);
        startAngleZ = transform.rotation.eulerAngles.z;
        initialOffset = transform.position - playerTransform.position;

        StartCoroutine(SwingSword());
    }

    private IEnumerator SwingSword()
    {
        float startAngle = startAngleZ;
        float endAngle = startAngleZ + 2 * attack2_Angle;

        float elapsedTime = 0f;

        while (elapsedTime < Mathf.Abs(endAngle - startAngle) / (moveSpeed * 30))
        {
            float currentAngle = Mathf.Lerp(startAngle, endAngle, elapsedTime * (moveSpeed * 30) / Mathf.Abs(endAngle - startAngle));

            transform.position = playerTransform.position + initialOffset;
            transform.rotation = Quaternion.Euler(0, 0, currentAngle);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
