using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
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

    [Title("Settings")]
    [SerializeField] float timeBetweenTextAnimations = 0.5f;




    [ReadOnly] int currentMission = -1;

    static public TrainingManager instance;
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        NextPart(-1);
    }

    public IEnumerator NextPart(int missionID)
    {
        if (currentMission == missionID)
        {
            currentMission++;
            //tooltipHeader.text = trainingTextList[currentMission].Header;
            FeelFeedbacksManager.instance.TooltipTextDisappear.PlayFeedbacks();

            yield return new WaitForSeconds(timeBetweenTextAnimations);
            
            tooltipText.text = trainingTextList[currentMission].Text;
            FeelFeedbacksManager.instance.TooltipTextAppear.PlayFeedbacks();


            if      (missionID == 1) IslandActive();
            else if (missionID == 2) AttackActive();
            else if (missionID == 3) ParryActive();
            else if (missionID == 4) SpecialAttackActive();
        }

    }

    void IslandActive()
    {
        TrainingIsland1.SetActive(true);
    }

    void AttackActive()
    {
        TrainingIsland2.SetActive(false);
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