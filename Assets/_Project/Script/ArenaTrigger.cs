using Unity.VisualScripting;
using UnityEngine;

public class ArenaTrigger : MonoBehaviour
{
    [SerializeField] int arenaId;
    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.GetComponent<PlayerTag>() != null)
        if (!LevelManager.instance.InTheArena)
        {
            LevelManager.instance.StartArena(arenaId);
        }
    }
}
