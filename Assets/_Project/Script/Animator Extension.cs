using UnityEngine;
using UnityEngine.Events;

public class AnimatorExtension : MonoBehaviour
{
    public static UnityEvent AttackPrepared = new();
    public void SendAttackPrepared() {  AttackPrepared.Invoke(); }
    
}
