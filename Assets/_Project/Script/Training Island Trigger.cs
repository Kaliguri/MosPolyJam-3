using UnityEngine;

public class TrainingIslandTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D colider2D)
    {
        if (colider2D.gameObject.GetComponent<PlayerTag>() is not null)
        {
            if (TrainingManager.instance is not null)  if (TrainingManager.instance != null) StartCoroutine(TrainingManager.instance.NextPart(2));
        }
    }
}
