using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Title("Arena")]
    [SerializeField] List<Arena> arenaList;

    [Title("Settings")]
    [SerializeField] ParticleSystem spawnVFX;

    public static LevelManager instance = null;
    [ReadOnly] private int currentWaveNumber = -1;
    [ReadOnly] public bool InTheArena = false;
    void Awake()
    {
        instance = this;
    }
    public void BattleStateUpdate(int arenaID, bool isStart)
    {

        if (isStart)
        {
            InTheArena = true;
            SetActiveForListGameObjects(arenaList[arenaID].DisableGameObjectsListForStartCombat, false); 
            SpawnWave(arenaID);
        }

        else
        {
            SetActiveForListGameObjects(arenaList[arenaID].EnableGameObjectsListAfterCombat, true); 
            currentWaveNumber = -1;
            InTheArena = false;
        }
        
    }

    void SetActiveForListGameObjects(List<GameObject> gameObjectsList, bool activeValue)
    {
        foreach (GameObject gameObject in gameObjectsList)
        {
            gameObject.SetActive(activeValue);
        }
    }

    void SpawnWave(int arenaID)
    {
        currentWaveNumber++;

        var availableWave = arenaList[arenaID].WaveCombat;
        var waveNumber = UnityEngine.Random.Range(0, availableWave.Count);
        var wave = availableWave[waveNumber];

        foreach (var enemy in wave.EnemyList)
        {
            var areaList = arenaList[arenaID].SpawnAreaListCombat;
            var areaNumber = UnityEngine.Random.Range(0, areaList.Count);
            var area = areaList[areaNumber];
            var spawnPosition = GetRandomPointInBox(area);

            Instantiate(spawnVFX, spawnPosition, quaternion.identity);
            Instantiate(enemy, spawnPosition, quaternion.identity);


        }

    }

    public void EndWave(int arenaID)
    {
        if (arenaList[arenaID].combatCount == currentWaveNumber)
        {
            BattleStateUpdate(arenaID, false);
        }

        else
        {
            SpawnWave(arenaID);
        }

    }

    Vector2 GetRandomPointInBox(Collider2D spawnArea)
    {
        Bounds bounds = spawnArea.bounds; // Получаем границы Box Collider

        // Генерируем случайные координаты внутри границ Box Collider
        float randomX = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
        float randomY = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);

        return new Vector2(randomX, randomY);
    }




}

[Serializable]
public class EnemyWave
{
    public List<GameObject> EnemyList;
}

[Serializable]
public class Arena
{
    [Title("Game Design")]
    public int combatCount;

    [Title("To Block Battle Location")]
    public List<GameObject> DisableGameObjectsListForStartCombat;
    public List<GameObject> EnableGameObjectsListAfterCombat;

    [Title("EnemyWave")]
    public List<Collider2D> SpawnAreaListCombat;
    public List<EnemyWave> WaveCombat;

}