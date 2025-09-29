using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// An Attack to use against an Opponent. If an "Attack" is a multi-hit, require multiple objects of this class.
/// </summary>
public class Attack : BaseItem
{
    private readonly byte requiredLevel;
    private readonly UnitStats.UnitType attackType;
    private readonly AttackStep[] strikes;
    

    //TODO: Add more stuff once the basic function of Battle works
    private int requiredMana;

    public Attack(int animationID, string name, string description, byte requiredLevel, int requiredMana,
        IntendedTarget target, UnitStats.UnitType attackType, AttackStep[] strikes)
        : base(animationID, name, description, target)
    {
        whereToMovePriorToUse = WhereToMovePriorToUse.STAY_IN_PLACE;
        classification = ItemClassification.ATTACK;
        this.attackType = attackType;
        this.requiredLevel = requiredLevel;
        this.requiredMana = requiredMana;
        this.strikes = strikes;
    }

    public Attack(int animationID, ItemClassification classification, string name, string description, byte requiredLevel, int requiredMana,
        IntendedTarget target, UnitStats.UnitType attackType, AttackStep[] strikes)
        : base(animationID, name, description, target)
    {
        whereToMovePriorToUse = WhereToMovePriorToUse.STAY_IN_PLACE;
        this.classification = classification;
        this.attackType = attackType;
        this.requiredLevel = requiredLevel;
        this.requiredMana = requiredMana;
        this.strikes = strikes;
    }

    /// <summary>
    /// Return the Attack Type of this attack.
    /// </summary>
    /// <returns></returns>
    public UnitStats.UnitType GetAttackType()
    {
        return attackType;
    }

    /// <summary>
    /// Is this an unlocked move by the Unit's level?
    /// </summary>
    /// <param name="requiredLevel"></param>
    /// <returns></returns>
    public bool UnlockedAttack(byte usersLevel)
    {
        return usersLevel >= requiredLevel;
    }

    /// <summary>
    /// Does User have required Mana?
    /// </summary>
    /// <param name="currentMana"></param>
    /// <returns></returns>
    public bool CanUseMove(int currentMana)
    {
        return currentMana >= requiredMana;
    }

    /// <summary>
    /// Should Unit move in front of Target before activating Action Commands/Attacking?
    /// </summary>
    /// <returns></returns>
    public bool UnitShouldMoveToTarget()
    {
        return attackType == UnitStats.UnitType.NORMAL;
    }

    /// <summary>
    /// Get the Required Mana for this move.
    /// </summary>
    /// <returns></returns>
    public int GetRequiredMana()
    {
        return requiredMana;
    }

    /// <summary>
    /// Return all strikes via Array for this Attack.
    /// </summary>
    /// <returns></returns>
    public AttackStep[] GetStrikes()
    {
        return strikes;
    }

    /// <summary>
    /// Get the unique Action Command Type relative to the base Action Command Type.
    /// </summary>
    /// <returns></returns>
    public string GetActionCommandSubTypes()
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < strikes.Length; i++)
        {
            builder.Append(GetSubType(strikes[i]));
            if (i + 1 < strikes.Length)
            {
                builder.Append(", ");
            }
        }
        return builder.ToString();
    }

    private string GetSubType(AttackStep step)
    {
        switch (step.GetActionCommandType())
        {
            case ActionCommand.ActionType.RAPID_PRESS:
                switch (step.GetSubType())
                {
                    case "SHORT":
                        return "RAPID SHORT";
                    case "MEDIUM":
                        return "RAPID MEDIUM";
                    case "LONG":
                        return "RAPID LONG";
                    case "RAGE_CONTROL":
                        return "RAGE CONTROL";
                    default:
                        return "DEBUG " + step.GetActionCommandType();
                }
            case ActionCommand.ActionType.TIMELY_PRESS:
                switch (step.GetSubType())
                {
                    case "SLOW":
                        return "TIMELY SLOW";
                    case "MEDIUM":
                        return "TIMELY MEDIUM";
                    case "QUICK":
                        return "TIMELY QUICK";
                    case "HIGH_NOON_SLOW":
                        return "HIGH NOON SLOW";
                    case "HIGH_NOON_QUICK":
                        return "HIGH NOON QUICK";
                    default:
                        return "DEBUG " + step.GetActionCommandType();
                }
        }
        return "";
    }
}
