using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class AnimatorExtension : MonoBehaviour
{
    [ReadOnly] public bool attackPrepared = false;

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
