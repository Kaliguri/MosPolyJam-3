using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class UpdatesUIManager : MonoBehaviour
{
    [SerializeField] float waitingTime = 1.2f;
    [SerializeField] List<MMF_Player> cardFeelStartList;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCardAnimation();
    }

    void StartCardAnimation()
    {
        foreach (var card in cardFeelStartList)
        {
            card.PlayFeedbacks();
        }
    }

    public void SelectUpdate(int id)
    {
        Debug.Log("Player select "+ id + " update");

        OffOtherCard(id);

        Invoke("UIOff", waitingTime);

        
    }

    void OffOtherCard(int id)
    {
        for (int i = 0; i < cardFeelStartList.Count; i++)
        {
            if (i != id) cardFeelStartList[i].PlayFeedbacks();
        }
    }

    void UIOff()
    {
        gameObject.SetActive(false);
    }

}
