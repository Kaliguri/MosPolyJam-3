using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] float damage = 10f;

    private void OnEnable()
    {
        Debug.Log("Awake" + gameObject.name);
    }

    public float GetDamage()
    {
        return damage;
    }
}
