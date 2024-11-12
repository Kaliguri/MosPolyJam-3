using Sirenix.OdinInspector;
using UnityEngine;

public class HPController : MonoBehaviour
{
    [Title("HP")]
    [SerializeField] public float maxHP = 100f;
    [ReadOnly] public float currentHP = 100f;
    [ReadOnly] public bool canRespawn = false;

    void Start()
    {
        currentHP = maxHP;
    }
    public void RecieveDamage(float damage)
    {
        currentHP -= damage;
        if (currentHP <= 0) 
        {
            if (canRespawn) 
            {  
                currentHP = maxHP * 0.5f; canRespawn = false;
                DamageNumberManager.instance.SpawnLegendNeverDieText(gameObject, gameObject.transform.position);
            }
            
            else Death(); 
        }
    }

    public void GetHeal(float heal)
    {
        currentHP += heal;
        if (currentHP > maxHP) { currentHP = maxHP; }
    }

    private void Death()
    {
        if (gameObject.GetComponent<PlayerTag>() == null) 
        {
            Destroy(gameObject);
            LevelManager.SendEnemyDeath();

            if (GameManager.instance.IsTraining) 
            {
                TrainingManager.instance.NextPart(3);
                
                TrainingManager.instance.NextPart(10);
                TrainingManager.instance.EnemyCheckCardSelect();
                TrainingManager.instance.NextPart(8);
                TrainingManager.instance.NextPart(7);
                TrainingManager.instance.NextPart(6);
            }

        }
        else 
        { 
            if (GameManager.instance.IsTraining)
            {
                DamageNumberManager.instance.SpawnLegendNeverDieText(gameObject, gameObject.transform.position);
                currentHP = maxHP;
                FeelFeedbacksManager.instance.DeactiveLowHPImage();
            }
            else
            {
                GameManager.instance.LoseGame();
                Destroy(gameObject);
            }
        }
    }
}
