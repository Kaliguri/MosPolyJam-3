using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Title ("DataBase")]
    [SerializeField] DB originalDB;
    [ReadOnly] public DB DB;

    [Title ("Inputs")]
    [SerializeField] InputActionReference restartInput;


    static public GameManager instance;
    void Awake()
    {
        instance = this;

        DB = Instantiate(originalDB);
    }

    void Update()
    {
        if (restartInput.action.IsPressed()) Restart();
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void InputSetActive(bool activeValue)
    {
        var player = FindFirstObjectByType<PlayerTag>().gameObject;

        player.GetComponent<PlayerMovement>().enabled = activeValue;
        player.GetComponent<PlayerComboAttack>().enabled = activeValue;
        player.GetComponent<PlayerParry>().enabled = activeValue;

        player.GetComponent<PlayerMovement>().moveSpeed = 0;
    }
}
