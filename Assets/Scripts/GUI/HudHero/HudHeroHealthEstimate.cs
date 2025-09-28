using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Display the estimated health the Current Health should roll over to.
/// </summary>
public class HudHeroHealthEstimate : HudHeroTextBase
{
    private Color colorDissappear = new Color(1f, 1f, 1f, 0f);
    private Color colorEstimateAppear = new Color(1f, 1f, 1f, 0.5f);
    //private Color colorEstimateFlashRed = new Color(1f, 0f, 0f, 0.5f);
    //private Color colorEstimateFlashGreen = new Color(0f, 1f, 0f, 0.5f);
    private Color colorEstimateFlashBlue = new Color(0f, 1f, 0.75f, 0.5f);
    private Color colorEstimateFlashLuck = new Color(1f, 0.8f, 0.0f, 0.5f);

    private float timeToFlash = 0.0f;
    private float timeToToggleColor = 0.0f;
    private const float timeToToggleReset = 0.03f;

    private void Start()
    {
        fountainPen.color = colorDissappear;
    }
    private void Update()
    {
        if (TextShouldFlash())
        {
            timeToFlash -= Time.deltaTime;
            timeToToggleColor -= Time.deltaTime;
            if (TimeToToggleColor())
            {
                timeToToggleColor = timeToToggleReset;
                //ToggleColorBlue();
                ToggleColorGold();
            }
        }
    }

    /// <summary>
    /// Set the Health Estimate that should equal CurrentHealth but do not make text appear.
    /// </summary>
    /// <param name="text"></param>
    public void PrepareEstimate(string text)
    {
        SetText(text);
        fountainPen.color = colorDissappear;
    }

    /// <summary>
    /// Set the Health Estimate CurrentHealth to reach to. If text flashing, reset flashing.
    /// </summary>
    /// <param name="text"></param>
    public void SetEstimate(string text)
    {
        SetText(text);
        timeToFlash = 0.0f;
        timeToToggleColor = 0.0f;
        fountainPen.color = colorEstimateAppear;
    }

    /// <summary>
    /// Set the Health Estimate to flash text.
    /// </summary>
    /// <param name="text"></param>
    public void SetEstimateLuck(string text)
    {
        SetText(text);
        timeToFlash = 0.6f;
        timeToToggleColor = timeToToggleReset;
    }

    /// <summary>
    /// Clear out the text by making color alpha to zero.
    /// </summary>
    public void ClearText()
    {
        fountainPen.color = colorDissappear;
        timeToFlash = 0.0f;
        timeToToggleColor = 0.0f;
    }
    /// <summary>
    /// Is text ready to be cleared out?
    /// </summary>
    public bool ReadyToClearText()
    {
        return timeToFlash <= 0.0f;
    }

    /// <summary>
    /// Toggle the text color to blue. If text was dissappeared, do not run method.
    /// </summary>
    private void ToggleColorBlue()
    {
        if (fountainPen.color == colorDissappear)
        {
            return;
        }
        if (fountainPen.color == colorEstimateAppear)
        {
            fountainPen.color = colorEstimateFlashBlue;
            return;
        }
        else if (fountainPen.color == colorEstimateFlashBlue)
        {
            fountainPen.color = colorEstimateAppear;
            return;
        }
        fountainPen.color = colorEstimateAppear;
    }
    /// <summary>
    /// Toggle the text color to gold. If text was dissappeared, do not run method.
    /// </summary>
    private void ToggleColorGold()
    {
        if (fountainPen.color == colorDissappear)
        {
            return;
        }
        if (fountainPen.color == colorEstimateAppear)
        {
            fountainPen.color = colorEstimateFlashLuck;
            return;
        }
        else if (fountainPen.color == colorEstimateFlashLuck)
        {
            fountainPen.color = colorEstimateAppear;
            return;
        }
        fountainPen.color = colorEstimateAppear;
    }

    /// <summary>
    /// Should the text continue to flash color?
    /// </summary>
    private bool TextShouldFlash()
    {
        return timeToFlash > 0.0f;
    }

    /// <summary>
    /// Is it time to toggle color?
    /// </summary>
    /// <returns></returns>
    private bool TimeToToggleColor()
    {
        return timeToToggleColor <= 0.0f;
    }
}
