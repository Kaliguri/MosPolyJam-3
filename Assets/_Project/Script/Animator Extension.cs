using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class AnimatorExtension : MonoBehaviour
{
    [ReadOnly] public bool attackPrepared = false;

    public void SendAttackPrepared() {  GetComponentInParent<EnemyFollow>().ShootAtplayerTransform(); }

}
