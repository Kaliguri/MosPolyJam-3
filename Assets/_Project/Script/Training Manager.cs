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
    [SerializeField] TextMeshProUGUI tooltipHeader;
    [SerializeField] TextMeshProUGUI tooltipText;

    [ReadOnly] int currentText = -1;

    public static UnityEvent TrainingPartComplete = new();
    public static void SendTrainingPartComplete() {  TrainingPartComplete.Invoke(); }

    static public TrainingManager instance;
    void Awake()
    {
        instance = this;
        TrainingPartComplete.AddListener(NextPart);
    }

    void Start()
    {
        NextPart();
    }

    public void NextPart()
    {
        currentText++;

        //tooltipHeader.text = trainingTextList[currentText].Header;
        tooltipText.text = trainingTextList[currentText].Text;

    }

}


[Serializable]
public class TrainingText
{
    //[TextArea] public string Header;
    [TextArea] public string Text;
}