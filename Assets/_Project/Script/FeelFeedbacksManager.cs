using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;

public class FeelFeedbacksManager : MonoBehaviour
{
    [Title("Feedbacks")]
    [Header("General")]
    public MMF_Player TakeDamage;

    [Header("Fade")]
    public MMF_Player FadeIn;
    public MMF_Player FadeOut;

    [Header("CameraShaking")]
    public List<MMF_Player> CameraShakingList;

    [Header("Slow-mo")]
    public MMF_Player SlowMo;

    [Title("GameObjects Reference")]
    [SerializeField] GameObject LowHPImage;

    [Title("Settings")]
    public float HPPercenForLowHP = 30f;


    public static FeelFeedbacksManager instance = null;
    void Awake()
    {
        instance = this;
    }

    public void ActiveLowHPImage()
    {
        LowHPImage.SetActive(true);
    }

    public void DeactiveLowHPImage()
    {
        LowHPImage.SetActive(false);
    }

    public IEnumerator PlayWithDelay(MMF_Player mmf, float delay)
    {
        yield return new WaitForSeconds(delay);

        mmf.PlayFeedbacks();
        //Debug.Log(mmf.transform.name);
    }

    public void ClearFeedbacks()
    {
        DeactiveLowHPImage();
    }
}