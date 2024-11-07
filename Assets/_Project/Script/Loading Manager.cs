using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    [Title("Settings")]
    [SerializeField] float delayBeforeLoading;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("LoadingEnd", delayBeforeLoading);
    }

    // Update is called once per frame

    [SerializeField] string menuScene;
    void LoadingEnd()
    {
        SceneManager.LoadScene(menuScene);
    }
}
