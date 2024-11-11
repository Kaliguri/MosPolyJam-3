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

    [Title("For Sword & Attack Mission")]
    [SerializeField] GameObject islandObject;
    [SerializeField] SwordRewardAnimation Sword; 
    [SerializeField] GameObject dummyAttack; 
    [SerializeField] CinemachineCamera swordCinemachine; 

    [Title("For Parry Mission")]
    [SerializeField] GameObject dummyParry;
    [ReadOnly] [SerializeField] int parryCount = 0; 

    [Title("For Special Attack Mission")]
    [SerializeField] GameObject dummySpecial;

    [Title("For Update Card Mission")]
    [SerializeField] UpdatesUIManager updatesUI;
    [SerializeField] float timeWaitingBeforeCardSelect = 8f;
    [SerializeField] float timeWaitingBeforeEnemySpawn = 3f;
    [SerializeField] List<GameObject> enemyCardMission;
    [ReadOnly] [SerializeField] int enemyDie;

    [Title("For Training Complete")]
    [SerializeField] GameObject CompleteUI;



    [Title("VFX")]
    [SerializeField] ParticleSystem spawnVFX;
    [SerializeField] float timeBetweenVFXandSpawn = 1.5f;

    [Title("Enemy")]
    [SerializeField] GameObject bodyRush;
    [SerializeField] GameObject javelinThrower;
    [SerializeField] GameObject spiningSword;
    [SerializeField] GameObject pikeman;




    [Title("Settings")]

    [SerializeField] bool haveStartBlock = true;
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
        
        if (haveStartBlock) StartBlocking();
    }

    void TooltipStart()
    {
        tooltipHeader.gameObject.SetActive(true);
        tooltipText.gameObject.SetActive(true);
        NextPart(-1);
    }

    void StartBlocking()
    {
        var player = FindFirstObjectByType<PlayerTag>().gameObject;

        player.GetComponent<PlayerComboAttack>().canAttack = false;
        player.GetComponent<PlayerComboAttack>().canSpesialAttack = false;

        player.GetComponent<PlayerParry>().canParry = false;


    }

    public void NextPart(int missionID)
    {
        StartCoroutine(OriginalNextPart(missionID));
    }

    public IEnumerator OriginalNextPart(int missionID)
    {
        if (currentMission == missionID)
        {
            currentMission++;

            if (missionID != -1) { FeelFeedbacksManager.instance.TooltipTextDisappear.PlayFeedbacks(); }

            //Debug.Log("Before: " + currentMission);
            yield return new WaitForSecondsRealtime(timeBetweenTextAnimations);
            //Debug.Log("After: " + currentMission +"?");
            
            tooltipText.text = trainingTextList[currentMission].Text;

            {FeelFeedbacksManager.instance.TooltipTextAppear.PlayFeedbacks(); }
            

            if      (missionID == 1) IslandActive();
            else if (missionID == 2) StartCoroutine(AttackActive());
            else if (missionID == 3) ParryActive();
            else if (missionID == 4) SpecialAttackActive();
            else if (missionID == 5) StartCoroutine(SpawnEnemy(bodyRush));
            else if (missionID == 6) StartCoroutine(SpawnEnemy(javelinThrower));
            else if (missionID == 7) StartCoroutine(SpawnEnemy(spiningSword));
            else if (missionID == 8) StartCoroutine(CardSelect());
            else if (missionID == 9) StartCoroutine(SpawnEnemy(pikeman));
            else if (missionID == 10) TrainingEnd();




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

        islandObject.SetActive(false);

        yield return new WaitForSeconds(timeBetweenInSwordAnimation);

        FeelFeedbacksManager.instance.CinematicLinesDisappear.PlayFeedbacks();

        FindFirstObjectByType<PlayerComboAttack>().canAttack = true;
        GameManager.instance.InputSetActive(true);

        tooltipHeader.gameObject.SetActive(true);
        tooltipText.gameObject.SetActive(true);
        FeelFeedbacksManager.instance.TooltipTextAppear.PlayFeedbacks();

        StartCoroutine(SpawnEnemy(dummyAttack));
        swordCinemachine.Priority = 4;

        
    }

    void ParryActive()
    {
        FindFirstObjectByType<PlayerParry>().canParry = true;
        StartCoroutine(SpawnEnemy(dummyParry));
    }

    public void ParryCheck()
    {
        parryCount ++;

        if (parryCount == 3)

        {
            Destroy(dummyParry);
            NextPart(4);
        }
        
    }

    void SpecialAttackActive()
    {
        FindFirstObjectByType<PlayerComboAttack>().canSpesialAttack = true;
        //StartCoroutine(SpawnEnemy(dummySpecial));
    }

    public void SpecialCheck()
    {
        //Destroy(dummySpecial);
        NextPart(5);
    }


    IEnumerator CardSelect()
    {
        yield return new WaitForSeconds(timeWaitingBeforeCardSelect);
        Debug.Log("UI?");
        updatesUI.gameObject.SetActive(true);
    }

    public void CardSelect2()
    {
        StartCoroutine(CardSelect3());
    }
    public IEnumerator CardSelect3()
    {
        yield return new WaitForSeconds(timeWaitingBeforeEnemySpawn);

        foreach (GameObject enemy in enemyCardMission)
        {
            StartCoroutine(SpawnEnemy(enemy));
            //Debug.Log("GO?");
        }
    }

    public void EnemyCheckCardSelect()
    {
        enemyDie++;

        if (enemyDie == 6)
        {
            NextPart(9);
        }
    }

    IEnumerator SpawnEnemy(GameObject enemy)
    {
        Instantiate(spawnVFX, enemy.transform.position, Quaternion.identity);

        yield return new WaitForSeconds(timeBetweenVFXandSpawn);

        enemy.SetActive(true);
        
    }

    void TrainingEnd()
    {
        FeelFeedbacksManager.instance.DeactiveLowHPImage();
        CompleteUI.SetActive(true);

    }

}


[Serializable]
public class TrainingText
{
    //[TextArea] public string Header;
    [TextArea] public string Text;
}