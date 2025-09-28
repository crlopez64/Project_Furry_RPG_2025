
using UnityEngine;
using TMPro;

/// <summary>
/// Control one Hero's GUI.
/// </summary>
public class HudHeroMaster : MonoBehaviour
{
    private HudUnitStatusAilmentsManager unitStatusAilmentsManager;
    private HudHeroHealthEstimate healthEstimate;
    private HudHeroHealthCurrent healthCurrent;
    private HudHeroManaCurrent manaCurrent;
    private HudHeroCurrentTurn currentTurn;
    private HudHeroHealthMax healthMax;
    private HudHeroName hudHeroName;
    private HudHeroManaMax manaMax;

    /// <summary>
    /// Allocate the children components.
    /// </summary>
    public void AllocateGuiCompoenents()
    {
        unitStatusAilmentsManager = GetComponentInChildren<HudUnitStatusAilmentsManager>(true);
        healthEstimate = GetComponentInChildren<HudHeroHealthEstimate>(true);
        healthCurrent = GetComponentInChildren<HudHeroHealthCurrent>(true);
        manaCurrent = GetComponentInChildren<HudHeroManaCurrent>(true);
        currentTurn = GetComponentInChildren<HudHeroCurrentTurn>(true);
        healthMax = GetComponentInChildren<HudHeroHealthMax>(true);
        hudHeroName = GetComponentInChildren<HudHeroName>(true);
        manaMax = GetComponentInChildren<HudHeroManaMax>(true);
        SetCurrentTurnOff();
    }

    /// <summary>
    /// Turn on that this is this Hero's turn.
    /// </summary>
    public void SetCurrentTurnOn()
    {
        currentTurn.gameObject.SetActive(true);
    }

    /// <summary>
    /// Turn off the Hero's turn display.
    /// </summary>
    public void SetCurrentTurnOff()
    {
        currentTurn.gameObject.SetActive(false);
    }

    /// <summary>
    /// Begin the HUD with everything set.
    /// </summary>
    /// <param name="selectHero"></param>
    public void StartHud(HeroStats selectHero)
    {
        // Base Case: If Health Estimate is for some reason not equal to Current health prior to beginning a battle
        if (!selectHero.HealthEstimateAligned())
        {
            selectHero.DamageRollerStop();
        }
        hudHeroName.SetText(selectHero.GetUnitName());
        healthMax.SetText("/" + selectHero.GetMaxHealth().ToString());
        healthCurrent.SetText(selectHero.GetCurrentHealth().ToString());
        manaCurrent.SetText(selectHero.GetCurrentMana().ToString());
        manaMax.SetText("/" + selectHero.GetMaxMana().ToString());
        healthEstimate.PrepareEstimate(selectHero.GetCurrentHealth().ToString());
        unitStatusAilmentsManager.ClearAllAilment();
    }

    /// <summary>
    /// Set Health max text.
    /// </summary>
    /// <param name="text"></param>
    public void SetHealthMax(string text)
    {
        healthMax.SetText("/" + text);
    }

    /// <summary>
    /// Set Health current text.
    /// </summary>
    /// <param name="text"></param>
    public void SetHealthCurrent(string text, string healthStatus)
    {
        healthCurrent.SetText(text, healthStatus);
    }

    /// <summary>
    /// Set Health current text and momentarily change its color on hit.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="healthtStatus"></param>
    /// <param name="hitType"></param>

    public void SetHealthCurrentOnHit(string text, string healthStatus)
    {
        healthCurrent.SetText(text, healthStatus);
    }

    /// <summary>
    /// Set Health Estimate text normally.
    /// </summary>
    /// <param name="text"></param>
    public void SetHealthEstimate(string text)
    {
        healthEstimate.SetEstimate(text);
    }

    /// <summary>
    /// Give Current Health a splash of color depending on hit type.
    /// </summary>
    /// <param name="hitType"></param>
    public void SetCurrentHealthColorSplash(string hitType)
    {
        healthCurrent.SetMomentaryColor(hitType);
    }

    /// <summary>
    /// Set Health Estimate text and flash it.
    /// </summary>
    /// <param name="text"></param>
    public void SetHealthEstimateFlash(string text)
    {
        healthEstimate.SetEstimateLuck(text);
    }

    /// <summary>
    /// Once ready, clear out the Health Estimate text.
    /// </summary>
    public void ClearHealthEstimate()
    {
        if (healthEstimate.ReadyToClearText())
        {
            healthEstimate.ClearText();
        }
    }

    /// <summary>
    /// Clear out Health Estimate text, regardless if ready or not.
    /// </summary>
    public void ClearHealthEstimateHard()
    {
        healthEstimate.ClearText();
    }

    /// <summary>
    /// Set Mana max text.
    /// </summary>
    /// <param name="text"></param>
    public void SetManaMax(string text)
    {
        manaMax.SetText("/" + text);
    }

    /// <summary>
    /// Set Mana current text.
    /// </summary>
    /// <param name="text"></param>
    public void SetManaCurrent(string text)
    {
        manaCurrent.SetText(text);
    }

}
