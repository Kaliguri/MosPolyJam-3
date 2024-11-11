using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class AnimatorExtension : MonoBehaviour
{
    private Animator animator => GetComponent<Animator>();

    public void SendAttackPrepared() {  GetComponentInParent<EnemyFollow>().Attack(); }

    public void SendAttack2Preparing() 
    {
        var enemyAttack = transform.parent.gameObject.GetComponentInChildren<EnemyAttack2>();
        if (enemyAttack != null)
        {
            enemyAttack.PrepareAttack2();
        }
        var enemyAttack2 = transform.parent.gameObject.GetComponentInChildren<EnemyAttack4>();
        if (enemyAttack2 != null)
        {
            enemyAttack2.PrepareAttack2();
        }
    }

    public void ChangeBoolIsFalling() { animator.SetBool("isFalling", false); }

    public void SendDie() {  GetComponentInParent<EnemyFollow>().Die(); }

    public void SendStopFalling() { GetComponentInParent<PlayerMovement>().StopFalling(); }

    public void SendCheckAttack() { GetComponentInParent<PlayerComboAttack>().CheckAttack(); }
}
