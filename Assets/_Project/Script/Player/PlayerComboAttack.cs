using Sirenix.OdinInspector;
using Sonity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerComboAttack : MonoBehaviour
{
    [Title("Inputs")]
    [SerializeField] InputActionReference attackInput;

    [Title("Progreess Bar")]
    [SerializeField] GameObject progressBar;
    [SerializeField] float timeProgressBarApears = 0.15f;

    [Title("Combo Settings")]
    [SerializeField] float timeBetweenAttacksInCombo = 0.3f;
    [SerializeField] public float longPressThreshold = 0.5f;
    [SerializeField] public bool canSpesialAttack = true;

    [Title("AttacksGameObjects")]
    [SerializeField] List<GameObject> attacksList = new();

    [Title("Attack Parameters")]
    [SerializeField] private List<float> attacks_Damage = new List<float> { 10f, 20f, 30f };
    [Header("Attack 1")]
    [SerializeField] private float attack1_Distance = 1f;
    [SerializeField] private float attack1_MoveDistance = 5f;
    [SerializeField] private float attack1_MoveSpeed = 10f;
    [Header("Attack 1 Increases")]
    [SerializeField] private float attack1_DamageIncrease = 1f;
    [SerializeField] private float attack1_ScaleYIncrease = 1f;
    [SerializeField] private float attack1_ScaleXIncrease = 1f;

    [Header("Attack 2")]
    [SerializeField] private float attack2_MoveSpeed = 10f;
    [SerializeField] private float attack2_StartAngle = 30f;
    [Header("Attack 2 Increases")]
    [SerializeField] private float attack2_DamageIncrease = 1f;
    [SerializeField] private float attack2_ScaleXIncrease = 1f;
    [SerializeField] private float attack2_ScaleYIncrease = 1f;
    [SerializeField] private float attack2_AngleIncrease = 1f;

    [Header("Attack 3")]
    [SerializeField] private GameObject attack3_Object;
    [SerializeField] private float attack3_Distance = 1f;
    [SerializeField] private float attack3_MoveSpeed = 10f;
    [SerializeField] private float attack3_MoveDistance = 2f;
    [SerializeField] private float attack3_TimeBetweenSendSlice = 0.5f;
    [SerializeField] private int attack3_StartSliceCount = 5;
    [SerializeField] private int attack3_MaxSliceCount = 50;
    [SerializeField] private float attack3_MaxOffset = 0.5f;
    [Header("Attack 3 Increases")]
    [SerializeField] private float attack3_TimeBetweenSendSliceDesceasePercent = 1f;
    [SerializeField] private float attack3_SliceCountIncrease = 1f;

    [Title("Sounds")]
    [SerializeField] SoundEvent attack1Release;
    [SerializeField] SoundEvent attack2Release;
    [SerializeField] SoundEvent attack3Release;


    [Title("VFX")]
    [SerializeField] ParticleSystem attackReleaseVFX;
    [SerializeField] GameObject curseObject;
    [SerializeField] ParticleSystem curseVFX;



    public bool canAttack = true;

    private float attack1_StartScaleX;
    private float attack1_StartScaleY;
    private Vector3 attack1TargetPosition;
    private float attack2_StartScaleX;
    private float attack2_StartScaleY;
    private Vector3 attack2InitialOffset;
    private float attack2StartAngleZ = 0f;

    private float attackPressTime = 0f;
    private bool isLongPress = false;
    private int inputBuffered = 0;
    private bool attackPressed;
    private bool attackPreparation = false;

    private Coroutine curse;
    private float defaultValue = 1f;
    private float lastClickTime = 0f;
    private int comboStep = 0;

    private Animator animator => GetComponentInChildren<Animator>();

    [Title("ReadonlyParametrs")]
    [ReadOnly] public bool attacking = false;
    [ReadOnly] public bool isCursed = false;
    [ReadOnly] public bool isAttacking = false;
    [ReadOnly] private List<GameObject> attack3SliceList = new();

    static public PlayerComboAttack instance;
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        progressBar.SetActive(false);

        Initialise1Attack();

        Initialise2Attack();

        Initialise3Attack();

        SetDamage();
    }

    private void Update()
    {
        if (attackInput.action.WasPressedThisFrame() && !isLongPress && !PlayerParry.instance.isParryState && !isCursed && canAttack)
        {
            attackPressTime = Time.time;
            attackPreparation = true;

            if (Time.time - lastClickTime > timeBetweenAttacksInCombo && comboStep != 0) ResetCombo();
            lastClickTime = Time.time;
        }


        if (attackInput.action.WasReleasedThisFrame() && !PlayerParry.instance.isParryState && !isCursed && Time.time - attackPressTime < longPressThreshold && canAttack)
        {
            //if (comboStep > 1) Debug.Log("Releases");
            if (attackPressed) inputBuffered++;
            else attackPressed = true;
            attackPreparation = false;
            progressBar.SetActive(false);
        }

        if (attackInput.action.IsPressed())
        {
            if (PlayerParry.instance.isParryState || isCursed || !canAttack)
            {
                progressBar.SetActive(false);
                attackPreparation = false;
            }
            else if (Time.time - attackPressTime >= longPressThreshold && attackPreparation)
            {
                if (canSpesialAttack) isLongPress = true;
                attackPreparation = false;

                if (attackPressed) inputBuffered++;
                else attackPressed = true;

                progressBar.SetActive(false);
            }
            else if (canSpesialAttack && timeProgressBarApears < Time.time - attackPressTime)
            {
                if (canSpesialAttack) UpdateProgressBar();
                progressBar.GetComponentInChildren<ProgressBarTag>().gameObject.GetComponent<Image>().fillAmount = (Time.time - attackPressTime) / longPressThreshold;
            }
            else progressBar.SetActive(false);
        }
        else
        {
            attackPreparation = false;
            progressBar.SetActive(false); 
        }
    }

    private void FixedUpdate()
    {
        isAttacking = ChechIfIsAttacking();

        if (!isAttacking && attackPressed && !PlayerParry.instance.isParryState)
        {
            attackPressed = false;
            ProcessAttackInput(isLongPress);
            isLongPress = false;
        }
        else if (!isAttacking && inputBuffered > 0 && !PlayerParry.instance.isParryState)
        {
            inputBuffered--;
            ProcessAttackInput(isLongPress);
            isLongPress = false;
        }
    }

    private void UpdateProgressBar()
    {
        Vector3 mousePosition = Input.mousePosition;

        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        mousePosition.z = 0;

        progressBar.transform.position = mousePosition;

        progressBar.SetActive(true);
    }

    private void Initialise1Attack()
    {
        attack1_StartScaleX = attacksList[0].transform.localScale.x;
        attack1_StartScaleY = attacksList[0].transform.localScale.y;
    }

    private void Initialise2Attack()
    {
        attack2_StartScaleX = attacksList[1].transform.localScale.x;
        attack2_StartScaleY = attacksList[1].transform.localScale.y;
    }

    private void Initialise3Attack()
    {
        for (int i = 0; i < attack3_MaxSliceCount; i++)
        {
            GameObject newAttackObject = Instantiate(attack3_Object);
            newAttackObject.transform.SetParent(attacksList[2].transform, false);
            newAttackObject.transform.localPosition = new Vector2(0, 0);
            newAttackObject.SetActive(false);
            attack3SliceList.Add(newAttackObject);
        }
    }

    private void SetDamage()
    {
        for (int i = 0; i < attacksList.Count; i++)
        {
            if (i < 2)
            {
                var attackComponent = attacksList[i].GetComponent<Attack>() ?? attacksList[i].GetComponentInChildren<Attack>();
                if (attackComponent != null)
                {
                    attackComponent.SetDamage(attacks_Damage[i]);
                }
            }
            else if (i == 2)
            {
                for (int j = 0; j < attack3SliceList.Count; j++)
                {
                    var attackComponent = attack3SliceList[j].GetComponent<Attack>() ?? attack3SliceList[j].GetComponentInChildren<Attack>();
                    if (attackComponent != null)
                    {
                        attackComponent.SetDamage(attacks_Damage[i]);
                    }
                }
            }
        }
    }

    private void ProcessAttackInput(bool _isLongPress)
    {
        if (comboStep == 0) animator.SetInteger("attackNumber", comboStep + 1);

        if (comboStep < 4)
        {
            comboStep++;
            attacking = true;
            PerformComboAttack(comboStep, _isLongPress);
        }
        else
        {
            ResetCombo();
        }
    }

    private bool ChechIfIsAttacking()
    {
        for (int i = 0; i < attacksList.Count; i++)
        {
            if (attacksList[i].activeSelf == true) return true;
        }
        return false;
    }

    public void CheckAttack()
    {
        if (comboStep > 1) Debug.Log("CheckAttack");
        if (attackPressed || inputBuffered > 0)
        {
            animator.SetInteger("attackNumber", comboStep + 1);
        }
        else animator.SetInteger("attackNumber", 0);
    }

    private void PerformComboAttack(int _comboStep, bool _isLongPress)
    {
        switch (_comboStep)
        {
            case 1:
                Attack1(_comboStep, _isLongPress);
                if (_isLongPress) ResetCombo();
                break;
            case 2:
                Attack2(_comboStep, _isLongPress);
                if (_isLongPress) ResetCombo();
                break;
            case 3:
                Attack3(_comboStep, _isLongPress);
                if (_isLongPress) ResetCombo();
                break;
            default:
                ResetCombo();
                break;
        }
    }

    private void Attack1(int _comboStep, bool _isLongPress)
    {
        float parryValue = 0;
        if (_isLongPress) parryValue += PlayerSphereManager.instance.PullSpheresToCenter() + defaultValue;

        if (parryValue > 0) 
        if (GameManager.instance.IsTraining) TrainingManager.instance.SpecialCheck();


        GameObject attack = attacksList[_comboStep - 1];

        var attackComponent = attack.GetComponent<Attack>() ?? attack.GetComponentInChildren<Attack>();
        if (attackComponent != null)
        {
            attackComponent.SetDamage(attacks_Damage[0] + attack1_DamageIncrease * parryValue);
        }
        attack.transform.localScale = new Vector3(attack1_StartScaleX + attack1_ScaleXIncrease * parryValue, attack1_StartScaleY + attack1_ScaleYIncrease * parryValue, 1);

        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPosition.z = transform.position.z; 

        Vector3 directionToCursor = (cursorPosition - transform.position).normalized;
        float angle = Mathf.Atan2(directionToCursor.y, directionToCursor.x) * Mathf.Rad2Deg - 90f;
        
        attack.transform.SetPositionAndRotation(transform.position + directionToCursor * attack1_Distance, Quaternion.Euler(new Vector3(0, 0, angle)));

        attack.SetActive(true);
        attack1Release.Play2D();
        //Instantiate(attackReleaseVFX, gameObject.transform.position, attack.transform.rotation);
        

        attack1TargetPosition = attack.transform.position + attack.transform.up * attack1_MoveDistance;

        StartCoroutine(MoveAttack1Forward(attack, parryValue));

    }

    private IEnumerator MoveAttack1Forward(GameObject attack, float parryValue)
    {
        while (Vector3.Distance(attack.transform.position, attack1TargetPosition) > 0.1f)
        {
            if (GetComponent<PlayerMovement>().isDashing || PlayerParry.instance.isParryState) break;
            attack.transform.position = Vector3.MoveTowards(attack.transform.position, attack1TargetPosition, attack1_MoveSpeed * Time.deltaTime);
            yield return null;
        }

        lastClickTime = Time.time;

        attack.SetActive(false);
    }

    private void Attack2(int _comboStep, bool _isLongPress)
    {

        //Debug.Log("Attack2");
        float parryValue = 0;
        if (_isLongPress) parryValue += PlayerSphereManager.instance.PullSpheresToCenter() + defaultValue;

        if (parryValue > 0) 
        if (GameManager.instance.IsTraining) TrainingManager.instance.SpecialCheck();

        GameObject attack = attacksList[_comboStep - 1];

        var attackComponent = attack.GetComponent<Attack>() ?? attack.GetComponentInChildren<Attack>();
        if (attackComponent != null)
        {
            attackComponent.SetDamage(attacks_Damage[1] + attack2_DamageIncrease * parryValue);
        }
        attack.GetComponentInChildren<Attack>().gameObject.transform.localScale = new Vector3(attack2_StartScaleX + attack2_ScaleXIncrease * parryValue, attack2_StartScaleY + attack2_ScaleYIncrease * parryValue, 1);

        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPosition.z = transform.position.z;

        Vector3 directionToCursor = (cursorPosition - transform.position).normalized;
        float angle = Mathf.Atan2(directionToCursor.y, directionToCursor.x) * Mathf.Rad2Deg - 90f;

        attack.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(new Vector3(0, 0, angle)));

        attack.SetActive(true);
        attack2Release.Play2D();
        //Instantiate(attackReleaseVFX, gameObject.transform.position, attack.transform.rotation);

        attack.transform.rotation = Quaternion.Euler(0, 0, attack.transform.rotation.eulerAngles.z - (attack2_StartAngle + parryValue * attack2_AngleIncrease));
        attack2StartAngleZ = attack.transform.rotation.eulerAngles.z;
        attack2InitialOffset = attack.transform.position - transform.position;

        StartCoroutine(SwingAttack2Sword(attack, parryValue));
    }

    private IEnumerator SwingAttack2Sword(GameObject attack, float parryValue)
    {
        float startAngle = attack2StartAngleZ;
        float endAngle = attack2StartAngleZ + 2 * (attack2_StartAngle + parryValue * attack2_AngleIncrease);

        float elapsedTime = 0f;

        while (elapsedTime < Mathf.Abs(endAngle - startAngle) / (attack2_MoveSpeed * 30))
        {
            if (GetComponent<PlayerMovement>().isDashing || PlayerParry.instance.isParryState) break;
            float currentAngle = Mathf.Lerp(startAngle, endAngle, elapsedTime * (attack2_MoveSpeed * 30) / Mathf.Abs(endAngle - startAngle));

            attack.transform.position = transform.position + attack2InitialOffset;
            attack.transform.rotation = Quaternion.Euler(0, 0, currentAngle);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        lastClickTime = Time.time;

        attack.SetActive(false);
    }

    private void Attack3(int _comboStep, bool _isLongPress)
    {
        float parryValue = 0;
        if (_isLongPress) parryValue += PlayerSphereManager.instance.PullSpheresToCenter() + defaultValue;

        if (parryValue > 0) 
        if (GameManager.instance.IsTraining) TrainingManager.instance.SpecialCheck();

        GameObject attack = attacksList[_comboStep - 1];

        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPosition.z = transform.position.z;

        Vector3 directionToCursor = (cursorPosition - transform.position).normalized;
        float angle = Mathf.Atan2(directionToCursor.y, directionToCursor.x) * Mathf.Rad2Deg - 90f;

        attack.transform.SetPositionAndRotation(transform.position + directionToCursor * attack3_Distance, Quaternion.Euler(new Vector3(0, 0, angle)));

        attack.SetActive(true);

        StartCoroutine(SendAttack3Slices(_comboStep, parryValue));
    }

    private IEnumerator SendAttack3Slices(int _comboStep, float parryValue)
    {
        //Debug.Log(attack3_StartSliceCount + parryValue * attack3_SliceCountIncrease);
        for (int i = 0; i < attack3_StartSliceCount + parryValue * attack3_SliceCountIncrease; i++)
        {
            attack3SliceList[i].SetActive(true);
            float randomOffset = UnityEngine.Random.Range(-attack3_MaxOffset, attack3_MaxOffset);
            attack3SliceList[i].transform.position += attack3SliceList[i].transform.right * randomOffset;

            Vector3 attack3TargetPosition = attack3SliceList[i].transform.position + attack3SliceList[i].transform.up * attack3_MoveDistance;
            StartCoroutine(MoveAttack3Forward(attack3SliceList[i], attack3TargetPosition, i >= attack3_StartSliceCount + parryValue * attack3_SliceCountIncrease - 1 || i + 1 == attack3_MaxSliceCount || isCursed, parryValue));

            attack3Release.Play2D();
            //Instantiate(attackReleaseVFX, gameObject.transform.position, attack3SliceList[i].transform.rotation);

            float elapsed = 0f;
            float timeBetweenSendSlice = attack3_TimeBetweenSendSlice * Mathf.Pow(attack3_TimeBetweenSendSliceDesceasePercent, parryValue);
            while (elapsed < timeBetweenSendSlice)
            {
                if (GetComponent<PlayerMovement>().isDashing || PlayerParry.instance.isParryState || i + 1 >= attack3_MaxSliceCount)
                {
                    lastClickTime = Time.time;
                    for (int j = 0; j < attack3SliceList.Count; j++)
                    {
                        attack3SliceList[j].SetActive(false);
                    }
                    attacksList[2].SetActive(false);
                    yield break;
                }
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
    }

    private IEnumerator MoveAttack3Forward(GameObject attack, Vector3 attack3TargetPosition, bool isLast, float parryValue)
    {
        while (Vector3.Distance(attack.transform.position, attack3TargetPosition) > 0.1f)
        {
            if (GetComponent<PlayerMovement>().isDashing || PlayerParry.instance.isParryState) break;
            attack.transform.position = Vector3.MoveTowards(attack.transform.position, attack3TargetPosition, attack3_MoveSpeed * Time.deltaTime);
            yield return null;
        }

        attack.SetActive(false);
        attack.transform.localPosition = new Vector2(0, 0);

        if (isLast || GetComponent<PlayerMovement>().isDashing || PlayerParry.instance.isParryState) 
        {
            lastClickTime = Time.time;
            for (int i = 0; i < attack3SliceList.Count; i++)
            {
                attack3SliceList[i].SetActive(false);
            }
            attack.transform.parent.gameObject.SetActive(false); 
        }
    }

    public void BecomeCursed(float timeBeforeCurse, float curseTime)
    {
        //Debug.Log("Debug.Log");
        if (curse != null) StopCoroutine(curse);
        curse = StartCoroutine(Cursed(timeBeforeCurse, curseTime));
    }

    public IEnumerator Cursed(float timeBeforeCurse, float curseTime)
    {
        yield return new WaitForSeconds(timeBeforeCurse);
        //Debug.Log("BecameCursed");
        isCursed = true;
        curseObject.SetActive(true);
        Instantiate(curseVFX, gameObject.transform.position, Quaternion.identity);

        yield return new WaitForSeconds(curseTime);
        //Debug.Log("Cured");
        curseObject.SetActive(false);
        isCursed = false;
    }

    private void ResetCombo()
    {
        //Debug.Log("ResetCombo");
        animator.SetInteger("attackNumber", 0);
        comboStep = 0;
        inputBuffered = 0;
    }
}
