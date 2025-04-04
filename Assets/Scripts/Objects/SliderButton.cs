using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderButton : MonoBehaviour
{
    private Slider slider;

    private void Start()
    {
        slider = GetComponentInParent<Slider>();
    }

    public void ChangeValue()
    {
        float currentValue = slider.value;
        if (currentValue == 0) slider.value = 1;
        else if (currentValue == 1) slider.value = 0;
    }
}
