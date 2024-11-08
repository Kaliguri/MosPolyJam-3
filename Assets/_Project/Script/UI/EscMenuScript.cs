using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class EscMenuScript : MonoBehaviour
{
    [Title("Inputs")]
    [SerializeField] InputActionReference escInput;

    [Title("EscMenu")]
    [SerializeField] GameObject escMenu;

    private bool escMenuOpen;

    private void Start()
    {
        escMenuOpen = false;
    }

    void Update()
    {
        if (escInput.action.WasPressedThisFrame()) OpenCloseEscMenu();
    }

    public void OpenCloseEscMenu()
    {
        escMenuOpen = !escMenuOpen;

        if (escMenuOpen)
        {
            ChangeUI(escMenuOpen);
            Time.timeScale = 0f;
        }
        else
        {
            ChangeUI(escMenuOpen);
            Time.timeScale = 1f;
        }
    }

    private void ChangeUI(bool escMenuOpen)
    {
        escMenu.SetActive(escMenuOpen);
    }
}
