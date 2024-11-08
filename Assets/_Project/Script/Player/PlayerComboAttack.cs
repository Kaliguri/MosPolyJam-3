using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerComboAttack : MonoBehaviour
{
    [Title("Inputs")]
    [SerializeField] InputActionReference attackInput;

    [Title("Combo Settings")]
    [SerializeField] float timeBetweenAttacks = 0.3f; 
    [SerializeField] float timeBetweenCombo = 2f;

    [Title("AttacksGameObjects")]
    [SerializeField] List<GameObject> attacksList = new();

    [Title("Attack Parameters")]
    [SerializeField] private float attack1_Distance = 1f;

    private float comboCooldownTimer = 0f;
    private float lastClickTime = 0f;
    private bool attacking = false;
    private int comboStep = 0;

    private void Update()
    {
        if (comboCooldownTimer > 0)
        {
            comboCooldownTimer -= Time.deltaTime;
            return;
        }

        if (Time.time - lastClickTime > timeBetweenAttacks && attacking)
        {
            StartComboCooldown();
        }

        if (attackInput.action.WasPressedThisFrame())
        {
            if ((Time.time - lastClickTime <= timeBetweenAttacks || comboStep == 0))
            {
                comboStep++;

                attacking = true;

                lastClickTime = Time.time;

                PerformComboAttack(comboStep);
            }
        }
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
    }

    private void Attack3(int _comboStep)
    {
        //attacksList[_comboStep - 1].SetActive(true);
        StartComboCooldown();
    }

    private void ResetCombo()
    {
        comboStep = 0;
    }

    private void StartComboCooldown()
    {
        Debug.Log("StartComboCooldown");
        comboCooldownTimer = timeBetweenCombo;
        attacking = false;
        comboStep = 0;
    }
}
