using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class AnimatorExtension : MonoBehaviour
{
    public void SendAttackPrepared() {  GetComponentInParent<EnemyFollow>().ShootAtplayerTransform(); }

    public void SendStopFalling() { GetComponentInParent<PlayerMovement>().StopFalling(); }

}
