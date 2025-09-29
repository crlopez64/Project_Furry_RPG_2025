using System.Collections.Generic;
using UnityEngine;
using static ActionCommand;
using static BaseItem;
using static UnitStats;

/// <summary>
/// A step for a single strike or to create an array of strikes for an Attack class.
/// </summary>
public class AttackStep
{
    private readonly Dictionary<StatusAilment, int> statusAilments;
    private readonly Dictionary<StatEffectType, int> statValues;
    private readonly BonusTypeOnActionCommand bonusTypeOnActionCommand;
    private readonly ActionButtonPressed[] buttonsRequiredToSelect;
    private readonly ActionType actionCommandType;
    private readonly byte statusAilmentChance;
    private readonly string subType;

    public AttackStep(ActionType actionCommandType, string subType,
        BonusTypeOnActionCommand bonusTypeOnActionCommand, ActionButtonPressed[] buttonsRequiredToSelect, 
        Dictionary<StatEffectType, int> statValues, Dictionary<StatusAilment, int> statusAilments,
        byte statusAilmentChance)
    {
        this.bonusTypeOnActionCommand = bonusTypeOnActionCommand;
        this.buttonsRequiredToSelect = buttonsRequiredToSelect;
        this.statusAilmentChance = statusAilmentChance;
        this.actionCommandType = actionCommandType;
        this.statusAilments = statusAilments;
        this.statValues = statValues;
        this.subType = subType;
    }

    /// <summary>
    /// Return a required button, or a random one if multiple given.
    /// </summary>
    /// <returns></returns>
    public ActionButtonPressed GetRequiredButton()
    {
        if (buttonsRequiredToSelect == null)
        {
            Debug.LogError("ERROR: Buttons Required To Select is null!!");
        }
        if (buttonsRequiredToSelect.Length == 0)
        {
            Debug.LogError("ERROR: Buttons Required To Select is empty!!");
        }
        if (buttonsRequiredToSelect.Length == 1)
        {
            return buttonsRequiredToSelect[0];
        }
        int getValue = Random.Range(0, buttonsRequiredToSelect.Length);
        return buttonsRequiredToSelect[getValue];
    }

    /// <summary>
    /// Does this Strike have Status Ailments to inflict?
    /// </summary>
    /// <returns></returns>
    public bool HasStatusAilments()
    {
        return statusAilments != null;
    }

    /// <summary>
    /// Does this Attack succeed in making Status Ailments?
    /// </summary>
    /// <returns></returns>
    public bool ActivateStatusAilments()
    {
        return Random.Range(0, 100) < statusAilmentChance;
    }

    /// <summary>
    /// Return the Subtype of an ActionType  in String form.
    /// </summary>
    /// <returns></returns>
    public string GetSubType()
    {
        return subType;
    }

    /// <summary>
    /// Get Action Command.
    /// </summary>
    /// <returns></returns>
    public ActionType GetActionCommandType()
    {
        return actionCommandType;
    }

    /// <summary>
    /// Return what type of Bonus this BaseItem should give, provided it requires an Action Command.
    /// </summary>
    /// <returns></returns>
    public BonusTypeOnActionCommand GetBonusTypeOnActionCommand()
    {
        return bonusTypeOnActionCommand;
    }
}
