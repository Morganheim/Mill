using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _minValueText;
    [SerializeField] private TextMeshProUGUI _maxValueText;
    [SerializeField] private TextMeshProUGUI _currentValueText;

    [field: SerializeField] public Slider Slider { get; private set; }

    private void Awake()
    {
        _minValueText.text = Slider.minValue.ToString();
        _maxValueText.text = Slider.maxValue.ToString();
        _currentValueText.text = Slider.value.ToString();
    }

    public void OnValueChanged(float value)
    {
        _currentValueText.text = Mathf.RoundToInt(value).ToString();
    }

    public void UpdateSliderValues(int minValue, int maxValue, int currentValue)
    {
        Slider.minValue = minValue;
        Slider.maxValue = maxValue;
        Slider.value = currentValue;

        _minValueText.text = Slider.minValue.ToString();
        _maxValueText.text = Slider.maxValue.ToString();
        _currentValueText.text = Slider.value.ToString();

        //reevaluate change
        OnValueChanged(Slider.value);
    }
}
