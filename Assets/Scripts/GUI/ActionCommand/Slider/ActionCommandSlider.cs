using System;
using UnityEngine;
using UnityEngine.UI;

public class ActionCommandSlider : MonoBehaviour
{
    private Color colorBgNormal;
    private Color colorBgActive;
    private Color colorDepleter;
    private Color colorNotInRange;
    private Color colorInRange;

    private Slider slider;
    private ActionCommandSliderFill sliderFill;
    private ActionCommandSliderBackground sliderBackground;

    public enum SliderSetColor
    {
        IN_RANGE,
        NOT_IN_RANGE,
        TIMELY_READY,
        TIMELY_NOT_READY,
        ACT_AS_TIMER
    }
    private void Awake()
    {
        AllocateGuiComponents();
    }

    /// <summary>
    /// Prepare the Slider and colors.
    /// </summary>
    /// <param name="maxvalue"></param>
    /// <param name="sliderSetColor"></param>
    public void PrepareSlider(float maxvalue, SliderSetColor sliderSetColor)
    {
        if (slider == null)
        {
            AllocateGuiComponents();
        }
        slider.minValue = 0;
        slider.maxValue = maxvalue;
        slider.value = 0;
        switch(sliderSetColor)
        {
            case SliderSetColor.NOT_IN_RANGE:
                sliderFill.SetColor(colorNotInRange);
                sliderBackground.SetColor(colorBgNormal);
                break;
            case SliderSetColor.TIMELY_NOT_READY:
                sliderFill.SetColor(colorDepleter);
                sliderBackground.SetColor(colorBgNormal);
                break;
            default:
                sliderFill.SetColor(colorNotInRange);
                sliderBackground.SetColor(colorBgNormal);
                break;
        }
    }

    /// <summary>
    /// Manually set Slider value and colors.
    /// </summary>
    /// <param name="value"></param>
    public void SetSliderValue(float value, SliderSetColor sliderSetColor)
    {
        if (slider == null)
        {
            AllocateGuiComponents();
        }
        slider.value = value;
        switch (sliderSetColor)
        {
            case SliderSetColor.NOT_IN_RANGE:
                sliderFill.SetColor(colorNotInRange);
                sliderBackground.SetColor(colorBgNormal);
                break;
            case SliderSetColor.IN_RANGE:
                sliderFill.SetColor(colorInRange);
                sliderBackground.SetColor(colorBgNormal);
                break;
            case SliderSetColor.TIMELY_NOT_READY:
                sliderFill.SetColor(colorDepleter);
                sliderBackground.SetColor(colorBgNormal);
                break;
            case SliderSetColor.TIMELY_READY:
                sliderFill.SetColor(colorDepleter);
                sliderBackground.SetColor(colorBgActive);
                break;
        }
    }

    /// <summary>
    /// Allocate GUI components.
    /// </summary>
    private void AllocateGuiComponents()
    {
        Debug.Log("Allocating Slider GUI");
        slider = GetComponent<Slider>();
        sliderFill = GetComponentInChildren<ActionCommandSliderFill>(true);
        sliderBackground = GetComponentInChildren<ActionCommandSliderBackground>(true);
        colorBgActive = Color.white;
        colorBgNormal = new Color(0.2f, 0.2f, 0.2f);
        colorDepleter = new Color(0.2f, 0.8f, 0.4f);
        colorInRange = new Color(0.2f, 0.8f, 0.4f);
        colorNotInRange = new Color(0.8f, 0f, 0f);
    }
}
