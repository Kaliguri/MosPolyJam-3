using Sirenix.OdinInspector;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Title ("DataBase")]
    [SerializeField] DB originalDB;
    [ReadOnly] public DB DB;


    static public GameManager instance;
    void Awake()
    {
        instance = this;

        DB = Instantiate(originalDB);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
