using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBar : MonoBehaviour
{
    [Title("Settings")]
    [SerializeField] float _lerpSpeed = 0.08f;

    [Title("GameObject Reference")]
    [SerializeField] Slider _secondarySlider;
    [SerializeField] Slider _primarySlider;

    public static PlayerHPBar instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private float _maxHealth
    {
        get
        {
            var player = FindFirstObjectByType<PlayerMovement>();
            if (player != null)
            {
                var hpController = player.gameObject.GetComponent<HPController>();
                if (hpController != null)
                {
                    return hpController.maxHP;
                }
            }
            return 100f; 
        }
    }

    private float _currentHealth
    {
        get
        {
            var player = FindFirstObjectByType<PlayerMovement>();
            if (player != null)
            {
                var hpController = player.gameObject.GetComponent<HPController>();
                if (hpController != null)
                {
                    return hpController.currentHP;
                }
            }
            return 100f;
        }
    }

    void Start()
    {
        _primarySlider.maxValue = _maxHealth;
        _primarySlider.value = _currentHealth;
        _secondarySlider.maxValue = _maxHealth;
        _secondarySlider.value = _currentHealth;
    }

    void Update()
    {
        if (_primarySlider.value != _currentHealth)
        {
            _primarySlider.value = _currentHealth;
        }

        if (_primarySlider.value != _secondarySlider.value)
        {
            _secondarySlider.value = Mathf.Lerp(_secondarySlider.value, _currentHealth, _lerpSpeed);
        }
    }

    public void SetMaxHP()
    {
        _primarySlider.maxValue = _maxHealth;
        _secondarySlider.maxValue = _maxHealth;
    }
}
