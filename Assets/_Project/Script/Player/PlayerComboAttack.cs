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

    private Vector3 attack1TargetPosition;
    private float attack2StartAngleZ = 0f;
    private Vector3 attack2InitialOffset;
    private float comboCooldownTimer = 0f;
    private float lastClickTime = 0f;
    public bool attacking = false;
    private int comboStep = 0;
    public bool canMove = true;

    private void Start()
    {
        for (int i = 0; i < attacksList.Count; i++)
        {
            if (i != 1) attacksList[i].GetComponent<PlayerAttack>().SetDamage(attacks_Damage[i]);
            else attacksList[i].GetComponentInChildren<PlayerAttack>().SetDamage(attacks_Damage[i]);
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

        //canMove = ChechIfCanMove();
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
        //attacksList[_comboStep - 1].SetActive(true);
    }

    private void ResetCombo()
    {
        comboStep = 0;
    }
}
