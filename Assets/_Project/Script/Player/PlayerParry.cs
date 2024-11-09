using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerParry : MonoBehaviour
{
    [Title("Inputs")]
    [SerializeField] InputActionReference parryInput;
    [Title("ParrySettings")]
    [SerializeField] public float PerfectParryTime = 0.3f;

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
            parryTime = 0f;
        }
        
        if (parryInput.action.WasReleasedThisFrame())
        {
            isParryState = false;
        }
    }

    private void FixedUpdate()
    {
        if (isParryState)
        {
            parryTime += Time.fixedDeltaTime;
        }
    }
}
