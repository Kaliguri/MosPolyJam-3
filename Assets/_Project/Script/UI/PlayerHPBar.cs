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

    private float _maxHeatlh => FindFirstObjectByType<PlayerMovement>().gameObject.GetComponent<HPController>().maxHP;
    private float _currentHeatlh => FindFirstObjectByType<PlayerMovement>().gameObject.GetComponent<HPController>().currentHP;
    void Start()
    {
        _primarySlider.maxValue = _maxHeatlh;
        _primarySlider.value = _currentHeatlh;
        _secondarySlider.maxValue = _maxHeatlh;
        _secondarySlider.value = _currentHeatlh;
    }

    void Update()
    {
        if (_primarySlider.value != _currentHeatlh)
        {
            _primarySlider.value = _currentHeatlh;
        }

        if (_primarySlider.value != _secondarySlider.value)
        {
            _secondarySlider.value = Mathf.Lerp(_secondarySlider.value, _currentHeatlh, _lerpSpeed);
        }
    }
}
