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
    [ReadOnly] private int currentWaveNumber = 0;
    [ReadOnly] private int currentEnemyNumber = 0;

    [Title("Update Cards")]
    [SerializeField] GameObject cards;

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
        currentWaveNumber = 0;
        InTheArena = false;
        if (CurrentArenaID == arenaList.Count - 1) LevelEnd();
        else ShowCards();
    }

    private void ShowCards()
    {
        cards.SetActive(true);
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
            var spawnPosition = GetRandomPointInCollider(area);

            Instantiate(spawnVFX, spawnPosition, quaternion.identity);
            Instantiate(enemy, spawnPosition, quaternion.identity);
        }

        availableWave.Remove(wave);

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

    Vector2 GetRandomPointInCollider(Collider2D spawnArea)
    {
        Bounds bounds = spawnArea.bounds;
        Vector2 randomPoint;

        // Пытаемся найти точку, пока она не будет внутри коллайдера
        do
        {
            float x = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
            float y = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);
            randomPoint = new Vector2(x, y);
        }
        while (!spawnArea.OverlapPoint(randomPoint));

        return randomPoint;
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