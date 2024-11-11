using System.Collections;
using UnityEngine;

public class SpearAttack2 : MonoBehaviour
{
    [SerializeField] float timeForPreparation = 1.5f;
    private Vector3 initialLocalPosition;
    public GameObject enemyParent;
    private SpriteRenderer spriteRenderer;
    private Transform playerTransform => PlayerComboAttack.instance.gameObject.transform;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialLocalPosition = enemyParent.transform.position - transform.parent.position;
        StartCoroutine(AttackPreparation());
    }

    private IEnumerator AttackPreparation()
    {
        float elapsedTime = 0f;

        while (elapsedTime < timeForPreparation)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            transform.up = direction;
            transform.parent.position = enemyParent.transform.position - initialLocalPosition;

            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / timeForPreparation);
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Color finalColor = spriteRenderer.color;
        finalColor.a = 1f;
        spriteRenderer.color = finalColor;
    }
}
