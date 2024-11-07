using Sirenix.OdinInspector;
using Sonity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] string gameScene;
    public void StartGame()
    {
        OffSounds();
        Invoke("LoadGameScene", 0.3f);
        
    }


    public void ExitGame()
    {
        Application.Quit();
    }

    void OffSounds()
    {
        SoundManager.Instance.StopEverything(false);
    }

    void LoadGameScene()
    {
        SceneManager.LoadScene(gameScene);
    }
}
