using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerComboAttack : MonoBehaviour
{
    [Title("Inputs")]
    [SerializeField] InputActionReference attackInput;

    [Title("Combo Settings")]
    [SerializeField] float timeBetweenAttacksInCombo = 0.3f;

    [Title("AttacksGameObjects")]
    [SerializeField] List<GameObject> attacksList = new();

    [Title("Attack Parameters")]
    [SerializeField] private List<float> attacks_Damage = new List<float> { 10f, 20f, 30f };
    [SerializeField] private float attack1_Distance = 1f;
    [SerializeField] private float attack1_MoveDistance = 5f;
    [SerializeField] private float attack1_MoveSpeed = 10f;
    [SerializeField] private float attack2_MoveSpeed = 10f;
    [SerializeField] private float attack2_Angle = 30f;
    [SerializeField] private GameObject attack3_Object;
    [SerializeField] private float attack3_Distance = 1f;
    [SerializeField] private float attack3_MoveSpeed = 10f;
    [SerializeField] private float attack3_MoveDistance = 2f;
    [SerializeField] private float attack3_TimeBetweenSendSlice = 0.5f;
    [SerializeField] private int attack3_SliceCount = 4;
    [SerializeField] private float attack3_MaxOffset = 0.5f;

    private Vector3 attack1TargetPosition;
    private float attack2StartAngleZ = 0f;
    private Vector3 attack2InitialOffset;
    private float lastClickTime = 0f;
    private int comboStep = 0;

    [Title("ReadonlyParametrs")]
    [ReadOnly] public bool attacking = false;
    [ReadOnly] public bool canMove = true;
    [ReadOnly] private List<GameObject> attack3SliceList = new();

    private void Start()
    {
        Initialise3Attack();

        SetDamage();
    }

    private void Initialise3Attack()
    {
        for (int i = 0; i < attack3_SliceCount; i++)
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
            if (i == 0) attacksList[i].GetComponent<PlayerAttack>().SetDamage(attacks_Damage[i]);
            else if (i == 1) attacksList[i].GetComponentInChildren<PlayerAttack>().SetDamage(attacks_Damage[i]);
            else
            {
                for (int j = 0; j < attack3SliceList.Count; j++)
                {
                    attack3SliceList[j].GetComponent<PlayerAttack>().SetDamage(attacks_Damage[i]);
                }
            }
        }
    }

    private void Update()
    {

        if (attackInput.action.WasPressedThisFrame())
        {
            if ((Time.time - lastClickTime <= timeBetweenAttacksInCombo && comboStep != 0) || comboStep == 0)
            {
                comboStep++;

                attacking = true;

                lastClickTime = Time.time;

                PerformComboAttack(comboStep);
            }
            else if (Time.time - lastClickTime > timeBetweenAttacksInCombo && comboStep != 0)
            {
                comboStep = 1;

                attacking = true;

                lastClickTime = Time.time;

                PerformComboAttack(comboStep);
            }
        }

        canMove = ChechIfCanMove();
    }

    private bool ChechIfCanMove()
    {
        for (int i = 0; i < attacksList.Count; i++)
        {
            if (attacksList[i].activeSelf == true) return false;
        }
        return true;
    }

    private void PerformComboAttack(int _comboStep)
    {
        switch (_comboStep)
        {
            case 1:
                Attack1(_comboStep);
                break;
            case 2:
                Attack2(_comboStep);
                break;
            case 3:
                Attack3(_comboStep);
                break;
            default:
                ResetCombo();
                break;
        }
    }

    private void Attack1(int _comboStep)
    {
        GameObject attack = attacksList[_comboStep - 1];

        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPosition.z = transform.position.z; 

        Vector3 directionToCursor = (cursorPosition - transform.position).normalized;
        float angle = Mathf.Atan2(directionToCursor.y, directionToCursor.x) * Mathf.Rad2Deg - 90f;
        
        attack.transform.SetPositionAndRotation(transform.position + directionToCursor * attack1_Distance, Quaternion.Euler(new Vector3(0, 0, angle)));

        attack.SetActive(true);

        attack1TargetPosition = attack.transform.position + attack.transform.up * attack1_MoveDistance;

        StartCoroutine(MoveAttack1Forward(attack));
    }

    private IEnumerator MoveAttack1Forward(GameObject attack)
    {
        while (Vector3.Distance(attack.transform.position, attack1TargetPosition) > 0.1f)
        {
            attack.transform.position = Vector3.MoveTowards(attack.transform.position, attack1TargetPosition, attack1_MoveSpeed * Time.deltaTime);
            yield return null;
        }

        attack.SetActive(false);
    }


    private void Attack2(int _comboStep)
    {
        GameObject attack = attacksList[_comboStep - 1];

        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPosition.z = transform.position.z;

        Vector3 directionToCursor = (cursorPosition - transform.position).normalized;
        float angle = Mathf.Atan2(directionToCursor.y, directionToCursor.x) * Mathf.Rad2Deg - 90f;

        attack.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(new Vector3(0, 0, angle)));

        attack.SetActive(true);

        attack.transform.rotation = Quaternion.Euler(0, 0, attack.transform.rotation.eulerAngles.z - attack2_Angle);
        attack2StartAngleZ = attack.transform.rotation.eulerAngles.z;
        attack2InitialOffset = attack.transform.position - transform.position;

        StartCoroutine(SwingAttack2Sword(attack));
    }

    private IEnumerator SwingAttack2Sword(GameObject attack)
    {
        float startAngle = attack2StartAngleZ;
        float endAngle = attack2StartAngleZ + 2 * attack2_Angle;

        float elapsedTime = 0f;

        while (elapsedTime < Mathf.Abs(endAngle - startAngle) / (attack2_MoveSpeed * 30))
        {
            float currentAngle = Mathf.Lerp(startAngle, endAngle, elapsedTime * (attack2_MoveSpeed * 30) / Mathf.Abs(endAngle - startAngle));

            attack.transform.position = transform.position + attack2InitialOffset;
            attack.transform.rotation = Quaternion.Euler(0, 0, currentAngle);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        attack.SetActive(false);
    }

    private void Attack3(int _comboStep)
    {
        GameObject attack = attacksList[_comboStep - 1];

        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPosition.z = transform.position.z;

        Vector3 directionToCursor = (cursorPosition - transform.position).normalized;
        float angle = Mathf.Atan2(directionToCursor.y, directionToCursor.x) * Mathf.Rad2Deg - 90f;

        attack.transform.SetPositionAndRotation(transform.position + directionToCursor * attack3_Distance, Quaternion.Euler(new Vector3(0, 0, angle)));

        attack.SetActive(true);

        StartCoroutine(SendAttack3Slices(_comboStep));
    }

    private IEnumerator SendAttack3Slices(int _comboStep)
    {
        for (int i = 0; i < attack3SliceList.Count; i++)
        {
            attack3SliceList[i].SetActive(true);
            float randomOffset = UnityEngine.Random.Range(-attack3_MaxOffset, attack3_MaxOffset);
            attack3SliceList[i].transform.position += attack3SliceList[i].transform.right * randomOffset;

            Vector3 attack3TargetPosition = attack3SliceList[i].transform.position + attack3SliceList[i].transform.up * attack3_MoveDistance;
            StartCoroutine(MoveAttack3Forward(attack3SliceList[i], attack3TargetPosition, i == attack3SliceList.Count - 1));

            yield return new WaitForSeconds(attack3_TimeBetweenSendSlice);
        }
        if (attack3SliceList.Count == 0) attacksList[_comboStep - 1].gameObject.SetActive(false);
    }

    private IEnumerator MoveAttack3Forward(GameObject attack, Vector3 attack3TargetPosition, bool isLast)
    {
        while (Vector3.Distance(attack.transform.position, attack3TargetPosition) > 0.1f)
        {
            attack.transform.position = Vector3.MoveTowards(attack.transform.position, attack3TargetPosition, attack3_MoveSpeed * Time.deltaTime);
            yield return null;
        }

        attack.SetActive(false);
        attack.transform.localPosition = new Vector2(0, 0);

        if (isLast) attack.transform.parent.gameObject.SetActive(false);
    }

    private void ResetCombo()
    {
        comboStep = 0;
    }
}