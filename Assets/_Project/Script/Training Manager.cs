using System;
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




    [ReadOnly] int currentMission = -1;

    public static UnityEvent<int> TrainingMissionComplete = new();
    public static void SendTrainingMissionComplete(int missionID) {  TrainingMissionComplete.Invoke(missionID); }

    static public TrainingManager instance;
    void Awake()
    {
        instance = this;
        TrainingMissionComplete.AddListener(NextPart);
    }

    void Start()
    {
        NextPart(-1);
    }

    public void NextPart(int missionID)
    {
        if (currentMission == missionID)
        {
            currentMission++;
            //tooltipHeader.text = trainingTextList[currentMission].Header;
            tooltipText.text = trainingTextList[currentMission].Text;

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