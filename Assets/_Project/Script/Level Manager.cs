using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Title("To Block Battle Location")]
    [Header("Combat 1")]
    [SerializeField] List<GameObject> DisableGameObjectsListForStartCombat0;
    [SerializeField] List<GameObject> EnableGameObjectsListAfterCombat0;

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
