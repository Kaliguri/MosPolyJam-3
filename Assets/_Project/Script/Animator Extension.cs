using UnityEngine;
using UnityEngine.Events;

public class AnimatorExtension : MonoBehaviour
{
    public bool attackPrepared = false;

    public static UnityEvent AttackPrepared = new();
    public void SendAttackPrepared() {  AttackPrepared.Invoke(); }

    private void FixedUpdate()
    {
        if (attackPrepared) 
        {
            attackPrepared = false;
            SendAttackPrepared(); 
        }
    }

}
