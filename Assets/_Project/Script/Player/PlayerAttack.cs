using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float damage = 10f;

    private void OnEnable()
    {
        Debug.Log("Awake" + gameObject.name);
    }

    public float GetDamage()
    {
        return damage;
    }

    public void SetDamage(float _damage)
    {
        damage = _damage;
    }
}
