using UnityEngine;

public class RestoreMusic : MonoBehaviour
{
    public void MusicRestore()
    {
        FindFirstObjectByType<MusicManager>().StartMusic();
    }
}
