using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerParry : MonoBehaviour
{
    [Title("Inputs")]
    [SerializeField] InputActionReference parryInput;

    [Title("ParrySettings")]
    [SerializeField] public float perfectParryTime = 0.3f;

    [Title("Read Only")]
    [ReadOnly] public float parryTime = 0f;
    [ReadOnly] public bool isParryState = false;

    [Title("GameObject Reference")]
    [SerializeField] GameObject parryShield;

    [Title("VFX")]
    [SerializeField] ParticleSystem normalParryVFX;
    [SerializeField] ParticleSystem perfectParryVFX;


    [Title("SFX")]

    public static PlayerParry instance = null;

    private void Awake()
    {
        if (instance == null) instance = this;
        parryShield.SetActive(false);
    }

    private void Update()
    {
        if (parryInput.action.WasPressedThisFrame())        ParryActivate();
        else if (parryInput.action.WasReleasedThisFrame())  ParryDeactivate();
    }

    void ParryActivate()
    {
        isParryState = true;
        parryShield.SetActive(true);
        parryTime = 0f;
        //Debug.Log("ParryState");

    }

    void ParryDeactivate()
    {
        isParryState = false;
        parryShield.SetActive(false);
        //Debug.Log("NormalState");
    }

    private void FixedUpdate()
    {
        if (isParryState)
        {
            parryTime += Time.fixedDeltaTime;
        }
    }

    public void ParryCast(bool isPerfectParry, Vector2 parryPosition)
    {
        if (isPerfectParry)
        { 
            Instantiate(perfectParryVFX, parryPosition, quaternion.identity);
        }

        else
        {
            Instantiate(normalParryVFX, parryPosition, quaternion.identity);
        }
    }
}
