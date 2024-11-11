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
    [SerializeField] public float perfectParryTime = 0.5f;


    [Title("Read Only")]
    [ReadOnly] public float parryTime = 0f;
    [ReadOnly] public bool isParryState = false;

    [Title("GameObject Reference")]
    [SerializeField] GameObject parryShield;

    [Title("Colors for Shield")]
    [SerializeField] Color PerfectColor;
    [SerializeField] Color NormalColor;

    [Title("VFX")]
    [SerializeField] ParticleSystem normalParryVFX;
    [SerializeField] ParticleSystem perfectParryVFX;


    [Title("SFX")]


    public bool canParry = true;

    public static PlayerParry instance = null;

    private void Awake()
    {
        if (instance == null) instance = this;

        SetPerfectColorForShield();
        ShieldActiveOrDeactive(false);
    }

    private void Update()
    {
        if (parryInput.action.WasPressedThisFrame() && canParry)        ParryActivate();
        else if (!canParry && isParryState) ParryDeactivate();
        else if (parryInput.action.WasReleasedThisFrame())  ParryDeactivate();
    }

    void ParryActivate()
    {
        isParryState = true;
        ShieldActiveOrDeactive(true);
        SetPerfectColorForShield();
        parryTime = 0f;
        //Debug.Log("ParryState");

    }

    void ParryDeactivate()
    {
        isParryState = false;
        ShieldActiveOrDeactive(false);
        //Debug.Log("NormalState");
    }

    void ShieldActiveOrDeactive(bool activeValue)
    {
        if (activeValue) parryShield.transform.localPosition = new Vector3(0,0,0);
        else             parryShield.transform.localPosition = new Vector3(9999,9999,9999);
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

    void SetPerfectColorForShield()
    {
        parryShield.GetComponent<SpriteRenderer>().color = PerfectColor;
        Invoke("SetNormalColorForShield", perfectParryTime);
    }

    void SetNormalColorForShield()
    {
        parryShield.GetComponent<SpriteRenderer>().color = NormalColor;
    }
}
