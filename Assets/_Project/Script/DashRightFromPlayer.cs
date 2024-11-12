using UnityEngine;

public class DashRightFromPlayer : MonoBehaviour
{
    [SerializeField] float alfthaIfDashed = 0.2f;

   public void SetVisability(bool visability)
    {
        if (visability)
        {
            Color color = GetComponentInChildren<SpriteRenderer>().color;
            color.a = 1f;
            GetComponentInChildren<SpriteRenderer>().color = color;
        }
        else
        {
            Color color = GetComponentInChildren<SpriteRenderer>().color;
            color.a = alfthaIfDashed;
            GetComponentInChildren<SpriteRenderer>().color = color;
        }
    }
}
