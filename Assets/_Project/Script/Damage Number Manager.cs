using System.Collections;
using System.Collections.Generic;
using DamageNumbersPro;
using Sirenix.OdinInspector;
using UnityEngine;

public class DamageNumberManager : MonoBehaviour
{

    
    [Title("Prefabs")]
    [SerializeField] GameObject prefabParentObject;
    [SerializeField] List<DamageNumber> prefabsList = new();

    [Title("Text Lists")]
    [SerializeField] List<string> parryTextList;
    [SerializeField] List<string> noDeathTextList;
    [SerializeField] List<string> FallTrainingTextList;

    [Title ("Other")]

    [ReadOnly] [SerializeField] int failTextCount = 0;



    private DamageNumber damageNumber;


    public static DamageNumberManager instance = null;
    void Awake()
    {
        if (instance == null) {instance = this;}
    }

    void Start()
    {
        //variantsListInizialize();
        prefabParentObject.SetActive(false);
    }

    IEnumerator TextSpawn(int prefabID, Vector3 position, string leftText = "", string rightText = "", float number = 0, float scale = 1f, GameObject parent = null, bool IsOnlyText = false, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);

        if (prefabsList.Count >= prefabID)
        {
        
        if (parent is not null)           {damageNumber = prefabsList[prefabID].Spawn(newPosition: position, newNumber: number, followedTransform: parent.transform);}
        else                              {damageNumber = prefabsList[prefabID].Spawn(newPosition: position, newNumber: number);}
        
        damageNumber.transform.localScale *= scale;

        if (leftText != "") 
        {
            damageNumber.enableLeftText = true;
            damageNumber.leftText = leftText;
        }

        if (rightText != "")
        {
            damageNumber.enableRightText = true;
            damageNumber.rightText = rightText;
        }

        if (IsOnlyText)
            {
                damageNumber.enableNumber = false;
            }
        }

        else Debug.LogError("Not prefab for Damage Number Manager!");
    }

    string PlusOrMinus(float value)
    {
        if (value > 0) 
        return "+";

        else if (value < 0) 
        return "";

        else
        return "";
    }


    public void SpawnDamageText(GameObject parent, Vector3 position, float textNumber, float scale = 1f, float delay = 0f)
    {
        StartCoroutine(TextSpawn(0, position, number: textNumber, scale: scale, delay: delay, parent: parent));
    }
    public void SpawnParryText(GameObject parent, Vector3 position, float scale = 1f, float delay = 0f)
    {
        string leftText = parryTextList[Random.Range(0, parryTextList.Count)];
        StartCoroutine(TextSpawn(1, position, leftText, scale: scale, delay: delay, IsOnlyText: true, parent: parent));
    }

    public void SpawnLegendNeverDieText(GameObject parent, Vector3 position, float scale = 1f, float delay = 0f)
    {
        string leftText = noDeathTextList[Random.Range(0, noDeathTextList.Count)];
        StartCoroutine(TextSpawn(2, position, leftText, scale: scale, delay: delay, IsOnlyText: true));
    }

    public void SpawnFallInTrainingText(Vector3 position, float scale = 1f, float delay = 0f)
    {
        if (failTextCount <=  FallTrainingTextList.Count - 1)
        {  
            string leftText = FallTrainingTextList[failTextCount];
            StartCoroutine(TextSpawn(3, position, leftText, scale: scale, delay: delay, IsOnlyText: true));
            failTextCount ++;
        }
    }
    
    public void SpawnHealText(GameObject parent, Vector3 position, float textNumber, float scale = 1f, float delay = 0f)
    {
        StartCoroutine(TextSpawn(4, position, number: textNumber, scale: scale, delay: delay, parent: parent));
    }
}
