using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class AnimatorExtension : MonoBehaviour
{
    private Animator animator => GetComponent<Animator>();

    public void SendAttackPrepared() {  GetComponentInParent<EnemyFollow>().Attack(); }

    public void ChangeBoolIsFalling() { animator.SetBool("isFalling", false); }

    public void SendDie() {  GetComponentInParent<EnemyFollow>().Die(); }

    public void SendStopFalling() { GetComponentInParent<PlayerMovement>().StopFalling(); }

    public void SendCheckAttack() { GetComponentInParent<PlayerComboAttack>().CheckAttack(); }
}
