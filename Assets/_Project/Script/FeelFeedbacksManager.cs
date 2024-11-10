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

    [Header("CameraShaking")]
    public List<MMF_Player> CameraShakingList;

    [Header("Slow-mo")]
    public MMF_Player SlowMo;

    [Header("UI")]
    public MMF_Player TooltipTextDisappear;
    public MMF_Player TooltipTextAppear;


    [Title("GameObjects Reference")]
    [SerializeField] GameObject LowHPImage;
    [SerializeField] GameObject CinematicLines;


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

    public void ActiveCinematicLines()
    {
        CinematicLines.SetActive(true);
    }

    public void DeactiveCinvematicLines()
    {
        CinematicLines.SetActive(false);

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