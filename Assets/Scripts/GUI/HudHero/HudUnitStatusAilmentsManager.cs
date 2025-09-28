using UnityEngine;

/// <summary>
/// Manager to keep track of a Unit's ailments.
/// </summary>
public class HudUnitStatusAilmentsManager : MonoBehaviour
{
    private HudUnitStatusAilment[] statusAilmentReport;

    private void Awake()
    {
        statusAilmentReport = GetComponentsInChildren<HudUnitStatusAilment>(true);
    }

    /// <summary>
    /// Update GUI with incoming Status ailment. Ignore if Ailment is already being reported.
    /// </summary>
    /// <param name="statusAilment"></param>
    public void SetAilmentToReport(UnitStats.StatusAilment statusAilment)
    {
        if (statusAilment == UnitStats.StatusAilment.NONE)
        {
            return;
        }
        if (AlreadyHasAilment(statusAilment))
        {
            return;
        }
        for(int i = 0; i < statusAilmentReport.Length; i++)
        {
            if (!statusAilmentReport[i].FilledAilment())
            {
                statusAilmentReport[i].SetStatusAilmentImage(statusAilment);
                return;
            }
        }
    }

    /// <summary>
    /// Remove a reported Ailment.
    /// </summary>
    /// <param name="statusAilment"></param>
    public void RemoveAilmentReport(UnitStats.StatusAilment statusAilment)
    {
        foreach (HudUnitStatusAilment statusAilmentImage in statusAilmentReport)
        {
            if (statusAilmentImage.AlreadyHasAilment(statusAilment))
            {
                statusAilmentImage.ClearStatusAilmentImage();
                return;
            }
        }
    }

    /// <summary>
    /// Clear all reported Ailment GUI.
    /// </summary>
    public void ClearAllAilment()
    {
        foreach (HudUnitStatusAilment statusAilmentImage in statusAilmentReport)
        {
            statusAilmentImage.ClearStatusAilmentImage();
        }
    }

    /// <summary>
    /// Is the GUI already showing this Ailment?
    /// </summary>
    /// <param name="statusAilment"></param>
    /// <returns></returns>
    private bool AlreadyHasAilment(UnitStats.StatusAilment statusAilment)
    {
        foreach(HudUnitStatusAilment statusAilmentImage in statusAilmentReport)
        {
            if (statusAilmentImage.AlreadyHasAilment(statusAilment))
            {
                return true;
            }
        }
        return false;
    }
}
