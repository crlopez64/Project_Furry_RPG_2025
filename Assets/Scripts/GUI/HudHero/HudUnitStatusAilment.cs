using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class in charge of a singular Status Ailment image.
/// </summary>
public class HudUnitStatusAilment : MonoBehaviour
{
    private Image image;
    private UnitStats.StatusAilment status;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    

    /// <summary>
    /// Update the Status Update Ailment to the appropriate image.
    /// </summary>
    /// <param name="status"></param>
    public void SetStatusAilmentImage(UnitStats.StatusAilment status)
    {
        Debug.Log("TODO: Update image!!");
        this.status = status;
        image.enabled = true;
    }

    /// <summary>
    /// Clear image.
    /// </summary>
    public void ClearStatusAilmentImage()
    {
        image.enabled = false;
        status = UnitStats.StatusAilment.NONE;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Helper method to report if the HUD is already reporting an incoming Ailment to report.
    /// Return TRUE if Ailment to report is NONE.
    /// </summary>
    /// <param name="addToReport"></param>
    /// <returns></returns>
    public bool AlreadyHasAilment(UnitStats.StatusAilment addToReport)
    {
        if (addToReport == UnitStats.StatusAilment.NONE)
        {
            return true;
        }
        return addToReport == status;
    }

    /// <summary>
    /// Return if this GUI is already exposing an Ailment.
    /// </summary>
    /// <returns></returns>
    public bool FilledAilment()
    {
        return status != UnitStats.StatusAilment.NONE;
    }
}
