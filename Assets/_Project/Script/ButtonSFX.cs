using SonityTemplate;
using UnityEngine;

public class ButtonSFX : MonoBehaviour
{
    public void PlayButtonHover()
    {
        TemplateSoundPlayUI.Instance.PlayButtonHover();
    }
    public void PlayButtonClick()
    {
        TemplateSoundPlayUI.Instance.PlayButtonClick();
    }

}
