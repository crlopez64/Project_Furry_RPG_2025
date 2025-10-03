using System;
using System.Collections.Generic;
using UnityEngine;
using static BaseItem;

/// <summary>
/// Manager in charge of holding Action Commands for Player to interact with.
/// Does not manage Units damaging other Units; only reporting if an Action Command has succeeded or not.
/// </summary>
public class ActionCommandManager : MonoBehaviour
{
    private BattleManager battleManager;
    private ActionCommandTimelyPress timelyPress;
    private ActionCommandRapidPress rapidPress;
    private ActionCommandSlider slider;
    private ActionCommand currentActionCommand;
    private TestActionCommandResult actionCommandResult;

    /// <summary>
    /// Allocate all Gui Action Command components.
    /// </summary>
    public void AllocateGuiComponents(BattleManager battleManager)
    {
        Debug.Log("Allocating GUI Components");
        AllocateBaseGui(battleManager);
    }

    /// <summary>
    /// Check if correct button was pressed.
    /// </summary>
    /// <param name="actionButtonPressed"></param>
    public void CheckButtonPressed(ActionCommand.ActionButtonPressed actionButtonPressed)
    {
        if (!HasActionCommandToInteract())
        {
            Debug.LogWarning("There's no Action Command to interact with.");
            return;
        }
        if (currentActionCommand.IsCorrectButtonPressed(actionButtonPressed))
        {
            currentActionCommand.CorrectInputPressed();
        }
        else
        {
            currentActionCommand.WrongInputPressed();
        }
    }

    /// <summary>
    /// Prepare a Rapid Press Action for Player to interact with. Only 1 should be active at any given moment.
    /// </summary>
    /// <param name="pressType"></param>
    /// <param name="buttonRequired"></param>
    public void PrepareRapidPress(AttackStep attackStep)
    {
        if (rapidPress.gameObject.activeInHierarchy)
        {
            Debug.Log("Do not have enough Rapid Press Action Commands in Pool. Cancelling request.");
            return;
        }
        Enum.TryParse(attackStep.GetSubType(), out ActionCommandRapidPress.RapidPressSelectType subTypeRapidGet);
        switch (subTypeRapidGet)
        {
            case ActionCommandRapidPress.RapidPressSelectType.SHORT:
                rapidPress.gameObject.GetComponent<ActionCommandRapidPress>().SetRapidPressShort(attackStep);
                break;
            case ActionCommandRapidPress.RapidPressSelectType.MEDIUM:
                rapidPress.gameObject.GetComponent<ActionCommandRapidPress>().SetRapidPressMedium(attackStep);
                break;
            case ActionCommandRapidPress.RapidPressSelectType.LONG:
                rapidPress.gameObject.GetComponent<ActionCommandRapidPress>().SetRapidPressLong(attackStep);
                break;
            case ActionCommandRapidPress.RapidPressSelectType.CONTROL_RAGE:
                rapidPress.gameObject.GetComponent<ActionCommandRapidPress>().SetRapidPressControlRage(0.5f, 20, attackStep, slider.GetComponent<ActionCommandSlider>());
                break;
            default:
                break;
        }
        currentActionCommand = rapidPress;
        rapidPress.gameObject.SetActive(true);
    }

    /// <summary>
    /// Prepare a Timely Press Action for player to interact with.
    /// </summary>
    /// <param name="pressType"></param>
    /// <param name="buttonRequired"></param>
    public void PrepareTimelyPress(AttackStep attackStep)
    {
        if (timelyPress.gameObject.activeInHierarchy)
        {
            Debug.Log("Do not have enough Timely Press Action Commands in Pool. Cancelling request.");
            return;
        }
        timelyPress.transform.position = new Vector3(UnityEngine.Random.Range(410f, 900f), UnityEngine.Random.Range(150f, 500f), 0f);
        Enum.TryParse(attackStep.GetSubType(), out ActionCommandTimelyPress.TimelyPressSpeedType subTypeTimelyGet);
        switch (subTypeTimelyGet)
        {
            case ActionCommandTimelyPress.TimelyPressSpeedType.SLOW:
                timelyPress.gameObject.GetComponent<ActionCommandTimelyPress>().SetTimelyPressSlow(slider.GetComponent<ActionCommandSlider>(), attackStep);
                break;
            case ActionCommandTimelyPress.TimelyPressSpeedType.MEDIUM:
                timelyPress.gameObject.GetComponent<ActionCommandTimelyPress>().SetTimelyPressMedium(slider.GetComponent<ActionCommandSlider>(), attackStep);
                break;
            case ActionCommandTimelyPress.TimelyPressSpeedType.QUICK:
                timelyPress.gameObject.GetComponent<ActionCommandTimelyPress>().SetTimelyPressQuick(slider.GetComponent<ActionCommandSlider>(), attackStep);
                break;
            case ActionCommandTimelyPress.TimelyPressSpeedType.HIGH_NOON_SLOW:
                timelyPress.gameObject.GetComponent<ActionCommandTimelyPress>().SetHighNoonPressSlow(slider.GetComponent<ActionCommandSlider>(), attackStep);
                break;
            case ActionCommandTimelyPress.TimelyPressSpeedType.HIGH_NOON_QUICK:
                timelyPress.gameObject.GetComponent<ActionCommandTimelyPress>().SetHighNoonPressFast(slider.GetComponent<ActionCommandSlider>(), attackStep);
                break;
            default:
                break;
        }
        currentActionCommand = timelyPress;
        timelyPress.gameObject.SetActive(true);
    }

    /// <summary>
    /// Let Action Command report back that Player has succeeded task.
    /// </summary>
    /// <param name="actionType"></param>
    /// <param name="transform"></param>
    public void ReportActionCommandPass(Transform transform, AttackStep attackStep)
    {
        SetActionCommandBonus(transform, attackStep);
        ClearOut();
        battleManager.AdvanceNextAttackAnimation();
        //TODO: Determine if Attack lands or hits
        battleManager.AttackEnemies(attackStep);
    }

    /// <summary>
    /// Let Action Command report back that Player failed task.
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="attackStep"></param>
    public void ReportActionCommandFail(Transform transform, AttackStep attackStep)
    {
        actionCommandResult.gameObject.transform.position = transform.position;
        actionCommandResult.SetText("FAIL");
        actionCommandResult.gameObject.SetActive(true);
        ClearOut();
        battleManager.AdvanceNextAttackAnimation();
        //TODO: Determine if Attack lands or hits
        battleManager.AttackEnemies(attackStep);
    }

    /// <summary>
    /// Debug method call in case an Action Command fails while not the currentActionCommand.
    /// </summary>
    /// <param name="actionCommand"></param>
    public void ReportActionCommandEndedBeforeReady(ActionCommand actionCommand)
    {
        Debug.LogError("ERROR: Somehow failed action command before it was ready for Player to interact with!" +
            "\nAction Command: " + actionCommand.gameObject.name);
    }

    /// <summary>
    /// Clear out all Action Commands GUI. Keep GameObject manager on. 
    /// </summary>
    public void ClearOut()
    {
        currentActionCommand = null;
        rapidPress.gameObject.SetActive(false);
        timelyPress.gameObject.SetActive(false);
        slider.gameObject.SetActive(false);
    }

    /// <summary>
    /// Return if there's an Action Command in progress.
    /// </summary>
    /// <returns></returns>
    public bool HasActionCommandToInteract()
    {
        return currentActionCommand != null;
    }
    
    /// <summary>
    /// Return if the action command specified is the one to interact with.
    /// </summary>
    /// <param name="actionCommand"></param>
    /// <returns></returns>
    public bool IsCurrentActionCommand(ActionCommand actionCommand)
    {
        return actionCommand == currentActionCommand;
    }

    /// <summary>
    /// Allocate the base items and make them notice this Manager.
    /// </summary>
    private void AllocateBaseGui(BattleManager battleManager)
    {
        this.battleManager = battleManager;
        slider = GetComponentInChildren<ActionCommandSlider>(true);
        rapidPress = GetComponentInChildren<ActionCommandRapidPress>(true);
        timelyPress = GetComponentInChildren<ActionCommandTimelyPress>(true);
        actionCommandResult = GetComponentInChildren<TestActionCommandResult>(true);
        timelyPress.SetActionCommandManager(this);
        rapidPress.SetActionCommandManager(this);
        actionCommandResult.ClearText();
    }

    /// <summary>
    /// Set TsetActionCommandBonus text.
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="bonusTypeOnActionCommand"></param>
    private void SetActionCommandBonus(Transform transform, AttackStep attackStep)
    {
        actionCommandResult.gameObject.transform.position = transform.position;
        switch (attackStep.GetBonusTypeOnActionCommand())
        {
            case BaseItem.BonusTypeOnActionCommand.NONE:
                actionCommandResult.SetText("SUCCESS");
                break;
            case BaseItem.BonusTypeOnActionCommand.DAMAGE_OUTPUT:
                actionCommandResult.SetText("DMG+");
                break;
            case BaseItem.BonusTypeOnActionCommand.MANA_GAIN:
                actionCommandResult.SetText("MNA+");
                break;
            case BaseItem.BonusTypeOnActionCommand.CRIT_INCREASE:
                actionCommandResult.SetText("CRIT+");
                break;
        }
        actionCommandResult.gameObject.SetActive(true);
    }
}
