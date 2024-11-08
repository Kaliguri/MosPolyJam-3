using Sirenix.OdinInspector;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    [Title("EscAndSettingsMenues")]
    [SerializeField] GameObject escMenu;
    [SerializeField] GameObject settingsMenu;

    private bool settingsMenuOpen;

    private void Start()
    {
        settingsMenuOpen = false;
    }

    public void OpenCloseSettingsMenu()
    {
        escMenu.SetActive(settingsMenuOpen);

        settingsMenuOpen = !settingsMenuOpen;

        settingsMenu.SetActive(settingsMenuOpen);
    }
}
