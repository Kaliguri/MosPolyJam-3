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

    IEnumerator TextSpawn(int prefabID, Vector3 position, string leftText = "", string rightText = "", float number = 0, float scale = 1f, bool IsOnlyText = false, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);

        if (prefabsList.Count >= prefabID)
        {

        DamageNumber damageNumber = prefabsList[prefabID].Spawn(newPosition: position, newNumber: number);
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


    public void SpawnDamageText(Vector3 position, float textNumber, float scale = 1f, float delay = 0f)
    {
        StartCoroutine(TextSpawn(0, position, number: textNumber, scale: scale, delay: delay));
    }
    public void SpawnShieldChangeText(Vector3 position, float textNumber, float scale = 1f, float delay = 0f)
    {   
        Debug.Log(textNumber + " " + PlusOrMinus(textNumber));
        StartCoroutine(TextSpawn(1, position, leftText: PlusOrMinus(textNumber),  number: textNumber, scale: scale, delay: delay));
    }
    public void SpawnHealText(Vector3 position, float textNumber, float scale = 1f, float delay = 0f)
    {
        StartCoroutine(TextSpawn(2, position, number: textNumber, scale: scale, delay: delay));
    }
    public void SpawnInfoText(Vector3 position, string textNumber, float scale = 1f, float delay = 0f)
    {
        StartCoroutine(TextSpawn(4, position, leftText: textNumber, scale: scale, delay: delay, IsOnlyText: true));
    }
}
