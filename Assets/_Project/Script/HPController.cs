using Sirenix.OdinInspector;
using UnityEngine;

public class HPController : MonoBehaviour
{
    [Title("HP")]
    [SerializeField] public float MaxHP = 100f;
    public float currentHP = 100f;


    public void RecieveDamage(float damage)
    {
        currentHP -= damage;
        if (currentHP <= 0) { Death(); }
    }

    private void Death()
    {
        if (gameObject.GetComponent<PlayerMovement>() == null) Destroy(gameObject);
        else { Debug.Log("You Lose!"); }
    }

    public float GetCurrentHP()
    {
        return currentHP;
    }

    public float GetMaxHP()
    {
        return MaxHP;
    }
}
