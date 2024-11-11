using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public class SwordRewardAnimation : MonoBehaviour
{
    [Title("Settings")]
    public float waitTime = 1.5f;
    [SerializeField] float speed = 5f;  // Скорость движения к цели
    [SerializeField] float fadeDuration = 2f; // Время, за которое объект исчезнет

    [SerializeField] ParticleSystem particle1;
    //[SerializeField] ParticleSystem particle2;



    public void StartAnimation()
    {
        StartCoroutine(MoveAndFadeCoroutine());     
    }

    private IEnumerator MoveAndFadeCoroutine()
    {
        yield return new WaitForSeconds(waitTime);


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

        particle1.gameObject.SetActive(true);
        //particle2.gameObject.SetActive(true);


        while (fadeTimer < fadeDuration)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, fadeTimer / fadeDuration);
            spriteRenderer.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            yield return null;
        }
    }

}
