using UnityEngine;

/// <summary>
/// GUI to display current Health to this Hero.
/// </summary>
public class HudHeroHealthCurrent : HudHeroTextBase
{
    private float lerpTimer = 0.0f;
    private const float lerpTimerReset = 1.0f;

    private Color colorNormal = new Color(1f, 1f, 1f, 1f);
    private Color colorWarning = new Color(1f, 0.5f, 0f, 1f);
    private Color colorDanger = new Color(1f, 0.3f, 0f, 1f);
    private Color colorTakeDamage = Color.red;
    private Color colorHealthRestore = Color.green;
    private Color colorOnHit = Color.green;
    private Color currentColor = new Color(1f, 1f, 1f, 1f);

    private void Update()
    {
        if (ChangeColorBack())
        {
            lerpTimer -= (Time.deltaTime * 3);
            fountainPen.color = Color.Lerp(currentColor, colorOnHit, lerpTimer);
        }
        else
        {
            lerpTimer = 0.0f;
            fountainPen.color = currentColor;
        }
    }

    /// <summary>
    /// Set Hero health and update color based on health status.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="status"></param>
    public void SetText(string text, string status)
    {
        SetText(text);
        switch (status)
        {
            case "GOOD":
                currentColor = colorNormal;
                break;
            case "WARNING":
                currentColor = colorWarning;
                break;
            case "DANGER":
                currentColor = colorDanger;
                break;
        }
    }

    /// <summary>
    /// Give text some momentary color.
    /// </summary>
    /// <param name="hitType"></param>
    public void SetMomentaryColor(string hitType)
    {
        lerpTimer = lerpTimerReset;
        switch (hitType)
        {
            case "RESTORE":
                colorOnHit = colorHealthRestore;
                break;
            case "DAMAGE":
                colorOnHit = colorTakeDamage;
                break;
            default:
                colorOnHit = colorTakeDamage;
                break;
        }
    }

    /// <summary>
    /// Is there time to revert text back to its intended color?
    /// </summary>
    /// <returns></returns>
    private bool ChangeColorBack()
    {
        return lerpTimer > 0.0f;
    }
}
