using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    private float targetValue;
    public float smoothSpeed = 10f;

    public void SetHP(float current, float max)
    {
        targetValue = current / max;
    }

    void Update()
    {
        slider.value = Mathf.Lerp(slider.value, targetValue, Time.deltaTime * smoothSpeed);
    }
}