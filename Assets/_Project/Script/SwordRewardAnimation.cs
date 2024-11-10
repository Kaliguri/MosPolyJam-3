using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public class SwordRewardAnimation : MonoBehaviour
{
    [Title("Settings")]
    [SerializeField] float speed = 5f;  // Скорость движения к цели
    [SerializeField] float fadeDuration = 2f; // Время, за которое объект исчезнет

    public void StartAnimation()
    {
        StartCoroutine(MoveAndFadeCoroutine());     
    }

    private IEnumerator MoveAndFadeCoroutine()
    {
        var player = FindFirstObjectByType<PlayerTag>().gameObject;  
        // Пока не достигли цели
        while (Vector3.Distance(transform.position, player.transform.position) > 0.1f)
        {
            // Перемещаемся к цели
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            yield return null;
        }

        // Начинаем процесс затухания
        var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        float fadeTimer = 0f;
        Color initialColor = spriteRenderer.color;

        while (fadeTimer < fadeDuration)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, fadeTimer / fadeDuration);
            spriteRenderer.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            yield return null;
        }

        // Удаляем объект после полного затухания
        Destroy(gameObject);
    }

}
