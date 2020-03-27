using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinBar : MonoBehaviour
{
    private Slider slider;
    public Gradient gradient;
    public Image fill;



    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Start()
    {
        if(GameManager.instance != null)
        {
            GradientColorKey[] winKey = {
                new GradientColorKey(gradient.colorKeys[0].color, GameManager.instance.winThresholdPercentage),
                new GradientColorKey(gradient.colorKeys[1].color,1f) };

            gradient.SetKeys(winKey, gradient.alphaKeys);
        }
    }

    public void SetMaxBar(int maxValue)
    {
        slider.maxValue = maxValue;
        slider.value = 0;
        fill.color = gradient.Evaluate(0f);
    }

    public void SetBar(int value)
    {
        slider.value = value;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
    public void IncrementBar()
    {
        slider.value++;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
    public float GetValue()
    {
        return slider.value;
    }
    public float GetNormalizedValue()
    {
        return slider.normalizedValue;
    }
    public float GetMaxValue()
    {
        return slider.maxValue;
    }
}
