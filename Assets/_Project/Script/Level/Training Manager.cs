using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class TrainingManager : MonoBehaviour
{
    [Title("Text")]
    public List<TrainingText> trainingTextList;

    [Title("GameObject Reference")]
    [Title("Tooltip")]
    [SerializeField] TextMeshProUGUI tooltipHeader;
    [SerializeField] TextMeshProUGUI tooltipText;

    [Title("Islands")]
    [SerializeField] GameObject TrainingIsland1;
    [SerializeField] GameObject TrainingIsland2;

    [Title("For Sword Animation")]
    [SerializeField] SwordRewardAnimation Sword; 
    [SerializeField] GameObject dummyTriangle; 
    [SerializeField] CinemachineCamera swordCinemachine; 




    [Title("Settings")]
    [SerializeField] float timeBetweenTextAnimations = 0.5f;
    [SerializeField] float timeBeforeSwordAnimation = 1.5f;
    [SerializeField] float timeBetweenInSwordAnimation = 4f;

    [Title("Read Only")]
    [ReadOnly] public int currentMission = -1;

    static public TrainingManager instance;
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        TooltipStart();
    }

    void TooltipStart()
    {
        tooltipHeader.gameObject.SetActive(true);
        tooltipText.gameObject.SetActive(true);
        StartCoroutine(NextPart(-1));
    }

    public IEnumerator NextPart(int missionID)
    {
        if (currentMission == missionID)
        {
            currentMission++;
            //tooltipHeader.text = trainingTextList[currentMission].Header;
            if (missionID != -1) { FeelFeedbacksManager.instance.TooltipTextDisappear.PlayFeedbacks(); }

            yield return new WaitForSeconds(timeBetweenTextAnimations);
            
            tooltipText.text = trainingTextList[currentMission].Text;
            if (missionID != 3) {FeelFeedbacksManager.instance.TooltipTextAppear.PlayFeedbacks(); }

            Debug.Log("NextPart:" + missionID);

            if      (missionID == 1) IslandActive();
            else if (missionID == 2) StartCoroutine(AttackActive());
            else if (missionID == 3) ParryActive();
            else if (missionID == 4) SpecialAttackActive();
        }

    }

    void IslandActive()
    {
        TrainingIsland1.SetActive(true);
    }

    IEnumerator AttackActive()
    {
        swordCinemachine.Priority = 6;
        tooltipHeader.gameObject.SetActive(false);
        tooltipText.gameObject.SetActive(false);

        FeelFeedbacksManager.instance.CinematicLinesAppear.PlayFeedbacks();
        GameManager.instance.InputSetActive(false);

        Sword.waitTime = timeBeforeSwordAnimation;
        Sword.StartAnimation();

        yield return new WaitForSeconds(timeBetweenInSwordAnimation);

        FeelFeedbacksManager.instance.CinematicLinesDisappear.PlayFeedbacks();
        GameManager.instance.InputSetActive(true);

        tooltipHeader.gameObject.SetActive(true);
        tooltipText.gameObject.SetActive(true);
        FeelFeedbacksManager.instance.TooltipTextAppear.PlayFeedbacks();

        dummyTriangle.SetActive(true);
        swordCinemachine.Priority = 4;

        
    }

    void ParryActive()
    {

    }

    void SpecialAttackActive()
    {

    }

}


[Serializable]
public class TrainingText
{
    //[TextArea] public string Header;
    [TextArea] public string Text;
}