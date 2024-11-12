using Sirenix.OdinInspector;
using Sonity;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Title("Music")]
    [SerializeField] SoundEvent mainMenuMusic;
    [SerializeField] SoundEvent trainingMusic;
    [SerializeField] SoundEvent gameplayMusic;

    [Title("Settings")]
    [SerializeField] bool IsMainManu;
    [SerializeField] bool IsTraining;
    [SerializeField] bool IsGameplay;

    void Start()
    {
        StartMusic();
        
    }

    public void StartMusic()
    {
        if (IsMainManu)      mainMenuMusic.PlayMusic();
        else if (IsTraining) trainingMusic.PlayMusic();
        else if (IsGameplay) gameplayMusic.PlayMusic();
    }
}
