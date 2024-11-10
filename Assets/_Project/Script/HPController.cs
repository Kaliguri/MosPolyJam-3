using Sirenix.OdinInspector;
using UnityEngine;

public class HPController : MonoBehaviour
{
    [Title("HP")]
    [SerializeField] public float maxHP = 100f;
    [ReadOnly] public float currentHP = 100f;

    void Start()
    {
        currentHP = maxHP;
    }
    public void RecieveDamage(float damage)
    {
        currentHP -= damage;
        if (currentHP <= 0) { Death(); }
    }

    private void Death()
    {
        if (gameObject.GetComponent<PlayerMovement>() == null) 
        {
            Destroy(gameObject);
            LevelManager.SendEnemyDeath();

            if (GameManager.instance.IsTraining) StartCoroutine(TrainingManager.instance.NextPart(3));

        }
        else 
        { 
            if (GameManager.instance.IsTraining)
            {
                DamageNumberManager.instance.SpawnLegendNeverDieText(gameObject, gameObject.transform.position);
                currentHP = maxHP;
            }
            else
            {
                Debug.Log("You Lose!"); 
            }
        }
    }
}
