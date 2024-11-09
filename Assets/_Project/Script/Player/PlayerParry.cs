using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerParry : MonoBehaviour
{
    [Title("Inputs")]
    [SerializeField] InputActionReference parryInput;
    [Title("ParrySettings")]
    [SerializeField] public float perfectParryTime = 0.3f;

    [Title("ParryParametrs")]
    [ReadOnly] public float parryTime = 0f;
    [ReadOnly] public bool isParryState = false;

    public static PlayerParry instance = null;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Update()
    {
        if (parryInput.action.WasPressedThisFrame())
        {
            isParryState = true;
            Debug.Log("ParryState");
            parryTime = 0f;
        }
        else if (parryInput.action.WasReleasedThisFrame())
        {
            isParryState = false;
            Debug.Log("NormalState");
        }
    }

    private void FixedUpdate()
    {
        if (isParryState)
        {
            parryTime += Time.fixedDeltaTime;
        }
    }

    public void ParryCast(bool isPerfectParry)
    {

    }
}
