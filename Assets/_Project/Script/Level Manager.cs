using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Title("Combat 1")]
    [Title("To Block Battle Location")]
    [SerializeField] List<GameObject> DisableGameObjectsListForStartCombat0;
    [SerializeField] List<GameObject> EnableGameObjectsListAfterCombat0;

    [Title("EnemyWave")]
    [SerializeField] List<Collider2D> SpawnAreaListCombat0;
    [SerializeField] List<EnemyWave> WaveCombat0;

    [Title("Settings")]
    [SerializeField] ParticleSystem spawnVFX;

    public static LevelManager instance = null;
    void Awake()
    {
        instance = this;
    }
    public void BattleStateUpate(int combatID, bool isStart)
    {
        if (combatID == 0)
        {
            if (isStart)
            {
                SetActiveForListGameObjects(DisableGameObjectsListForStartCombat0, false); 
            }

            else
            {
                SetActiveForListGameObjects(EnableGameObjectsListAfterCombat0, true); 
            }
        }
    }

    void SetActiveForListGameObjects(List<GameObject> gameObjectsList, bool activeValue)
    {
        foreach (GameObject gameObject in gameObjectsList)
        {
            gameObject.SetActive(activeValue);
        }
    }




}

[Serializable]
public class EnemyWave
{
    public List<GameObject> EnemyList;
}