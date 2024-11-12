using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    [Title("VFX")]
    [SerializeField] ParticleSystem spawnVFX;

    [Title("Settings")]
    [SerializeField] float waitingTimeBeforeWave = 2f;
    [ReadOnly] public bool InTheArena = false;
    [ReadOnly] public int CurrentArenaID = -1;
    [ReadOnly] [SerializeField] private int currentWaveNumber = 1;
    [ReadOnly][SerializeField] private int currentEnemyNumber = 0;

    [Title("For Level Complete")]
    [SerializeField] GameObject CompleteUI;

    [Title("Arena")]
    [SerializeField] List<Arena> arenaList;


    public static UnityEvent EnemyDeath = new();
    public static void SendEnemyDeath() {  EnemyDeath.Invoke(); }

    public static LevelManager instance = null;
    void Awake()
    {
        instance = this;
        EnemyDeath.AddListener(WaveStateCheck);
    }
    public void StartArena(int arenaId)
    {
        if (arenaId > CurrentArenaID)
        {
            CurrentArenaID ++;
            InTheArena = true;
            SetActiveForListGameObjects(arenaList[CurrentArenaID].DisableGameObjectsListForStartCombat, false); 
            StartCoroutine(SpawnWave());
        }
        
    }

    void FinishArena()
    {
        SetActiveForListGameObjects(arenaList[CurrentArenaID].EnableGameObjectsListAfterCombat, true); 
        currentWaveNumber = 1;
        InTheArena = false;
        if (CurrentArenaID == arenaList.Count - 1) LevelEnd();

    }

    void LevelEnd()
    {
        CompleteUI.SetActive(true);
    }

    void SetActiveForListGameObjects(List<GameObject> gameObjectsList, bool activeValue)
    {
        foreach (GameObject gameObject in gameObjectsList)
        {
            gameObject.SetActive(activeValue);
        }
    }

    IEnumerator SpawnWave()
    {
        yield return new WaitForSeconds(waitingTimeBeforeWave);

        currentWaveNumber++;

        var availableWave = arenaList[CurrentArenaID].WaveCombat;
        var waveNumber = UnityEngine.Random.Range(0, availableWave.Count);
        var wave = availableWave[waveNumber];

        currentEnemyNumber = wave.EnemyList.Count;

        foreach (var enemy in wave.EnemyList)
        {
            var areaList = arenaList[CurrentArenaID].SpawnAreaListCombat;
            var areaNumber = UnityEngine.Random.Range(0, areaList.Count);
            var area = areaList[areaNumber];
            var spawnPosition = GetRandomPointInBox(area);

            Instantiate(spawnVFX, spawnPosition, quaternion.identity);
            Instantiate(enemy, spawnPosition, quaternion.identity);
        }

    }

    void WaveStateCheck()
    {
        currentEnemyNumber --;

        if (currentEnemyNumber == 0)
        {
            EndWave();
        }
    }

    public void EndWave()
    {
        if (arenaList[CurrentArenaID].combatCount == currentWaveNumber)
        {
            FinishArena();
        }

        else
        {
            StartCoroutine(SpawnWave());
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