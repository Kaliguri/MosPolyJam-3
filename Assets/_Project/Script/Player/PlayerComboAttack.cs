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

    [Title("Attack Parameters")]

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

    private void PerformComboAttack(int comboStep)
    {
        switch (comboStep)
        {
            case 1:
                Attack1();
                break;
            case 2:
                Attack2();
                break;
            case 3:
                Attack3();
                break;
            default:
                ResetCombo();
                break;
        }
    }

    private void Attack1()
    {
        Debug.Log("Performing Attack 1");
    }

    private void Attack2()
    {
        Debug.Log("Performing Attack 2");
    }

    private void Attack3()
    {
        Debug.Log("Performing Attack 3");
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
