using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections;

public class UpdatesUIManager : MonoBehaviour
{
    [SerializeField] float waitingTime = 1.2f;
    [SerializeField] int cardCount = 3;
    [SerializeField] List<CardUpdate> cardUpdateList;

    [ReadOnly] [SerializeField] List<UpdateData> updateDataList = new(3);
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    static public UpdatesUIManager instance;
    void Awake()
    {
        instance = this;
    }

    void OnEnable()
    {
        //Time.timeScale = 0f;
        DataTransfer();
        StartCardAnimation();
    }

    void DataTransfer()
    {
        var db = GameManager.instance.DB;
        var availableUpdates = db.UpdatesList;

        List<int> randomNumber = new();

        if (availableUpdates.Count >= 3)
        {
        for (int i = 0; i < cardCount; i++)
        {
            int updateNumber = 0;
            bool isUnic = false;

            while (!isUnic)
            {
            updateNumber = Random.Range(0, availableUpdates.Count);
            if (!randomNumber.Contains(updateNumber)) isUnic = true; 
            }
            randomNumber.Add(updateNumber);
            
            Debug.Log("Count in List:" + availableUpdates.Count + ", random Number: " + updateNumber);
            var update = availableUpdates[updateNumber];

            //updateDataList[i] = update;
            cardUpdateList[i].DataTransfer(update);

        }
        }
        
        else Debug.LogError("Not enough updates!");
    }

    void StartCardAnimation()
    {
        foreach (var card in cardUpdateList)
        {
            card.cardFilpFeedback.PlayFeedbacks();
        }
    }

    public void SelectUpdate(int id)
    {
        //Debug.Log("Player select "+ id + " update");
        cardUpdateList[id].updateData.UpdateScript.Use();
        GameManager.instance.DB.UpdatesList.Remove(cardUpdateList[id].updateData);

        StartCoroutine(OffOtherCard(id));
    }

    IEnumerator OffOtherCard(int id)
    {
        for (int i = 0; i < cardUpdateList.Count; i++)
        {
            if (i != id) cardUpdateList[i].cardFilpFeedback.PlayFeedbacks();
        }

        yield return new WaitForSeconds(waitingTime);

        cardUpdateList[id].cardFilpFeedback.PlayFeedbacks();

        Invoke("OffUI", waitingTime);
    }
    void OffUI()
    {
        foreach (var card in cardUpdateList)
        {
            Destroy(card.Art);
        }
        gameObject.SetActive(false);
        //Time.timeScale = 1f;

        if (GameManager.instance.IsTraining) TrainingManager.instance.CardSelect2();
    }

}
