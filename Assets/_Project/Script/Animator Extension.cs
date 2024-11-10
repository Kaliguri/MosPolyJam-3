using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class AnimatorExtension : MonoBehaviour
{
    public void SendAttackPrepared() {  GetComponentInParent<EnemyFollow>().Attack(); }

    public void SendStopFalling() { GetComponentInParent<PlayerMovement>().StopFalling(); }

    public void SendCheckAttack() { GetComponentInParent<PlayerComboAttack>().CheckAttack(); }
}
