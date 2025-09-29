using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A base item that holds a name, an animation ID, and some description.
/// Can either be an Attack or Item.
/// </summary>
public class BaseItem
{
    protected WhereToMovePriorToUse whereToMovePriorToUse;
    protected ItemClassification classification;
    private readonly IntendedTarget target;
    private readonly string description;
    private readonly string itemName;
    private readonly int animationID;

    /// <summary>
    /// Does this BaseItem classify as an Attack or an Item for Inventory?
    /// </summary>
    public enum ItemClassification : byte
    {
        /// <summary>
        /// Debug purposes.
        /// </summary>
        NONE,
        /// <summary>
        /// BaseItem is an Attack (requires no Mana).
        /// </summary>
        ATTACK,
        /// <summary>
        /// BaseItem is an Attack that requires Mana to use.
        /// </summary>
        SKILL,
        /// <summary>
        /// BaseItem is an Item from Inventory.
        /// </summary>
        ITEM
    }

    /// <summary>
    /// The inteneded target of this Item.
    /// </summary>
    public enum IntendedTarget : byte
    {
        /// <summary>
        /// Target is user of BaseItem.
        /// </summary>
        SELF,
        /// <summary>
        /// Target is 1 unit in your Team.
        /// </summary>
        TEAM_ONE,
        /// <summary>
        /// Target is entire Team.
        /// </summary>
        TEAM_ALL,
        /// <summary>
        /// Target is 1 Enemy.
        /// </summary>
        ENEMY_ONE,
        /// <summary>
        /// Target is all Enemies.
        /// </summary>
        ENEMY_ALL,
        /// <summary>
        /// Everyone, including the User, is Target.
        /// </summary>
        EVERYONE
    }

    /// <summary>
    /// Where should the user of this BaseItem move prior to using the Move?
    /// </summary>
    public enum WhereToMovePriorToUse : byte
    {
        /// <summary>
        /// Unit should not move to a location and immediately attack.
        /// </summary>
        STAY_IN_PLACE,
        /// <summary>
        /// Unit should move to a location prior to attacking.
        /// </summary>
        MOVE_TO_TARGET
    }

    /// <summary>
    /// The bonus type to include to a BaseItem if there's an Action Command tied to it.
    /// </summary>
    public enum BonusTypeOnActionCommand : byte
    {
        /// <summary>
        /// There is no Bonus on this BaseItem.
        /// </summary>
        NONE,
        /// <summary>
        /// On Action Command Pass, increase Damage Output.
        /// </summary>
        DAMAGE_OUTPUT,
        /// <summary>
        /// On Action Command Pass, increase Mana Gain.
        /// </summary>
        MANA_GAIN,
        /// <summary>
        /// On Action Command Pass, increase Crit rate on attack.
        /// </summary>
        CRIT_INCREASE
    }

    /// <summary>
    /// Types for a base Item (be it an actual Item or Attack) can adjust with.
    /// </summary>
    public enum StatEffectType : byte
    {
        /// <summary>
        /// The Attack Power that will determine final Damage onto Target.
        /// </summary>
        POWER,
        /// <summary>
        /// Directly adjust the Current Health of the Unit.
        /// </summary>
        HEALTH_CURRENT,
        /// <summary>
        /// Directly adjust the Current Mana of the Unit.
        /// </summary>
        MANA_CURRENT,
        /// <summary>
        /// Directly adjust the Max Health of the Unit.
        /// </summary>
        HEALTH_MAX,
        /// <summary>
        /// Directly adjust the Max Mana of the Unit.
        /// </summary>
        MANA_MAX,
        /// <summary>
        /// Adjust the stage of Physical Attack (make Physical Attacks stronger or weaker).
        /// </summary>
        ATTACK_PHYSICAL,
        /// <summary>
        /// Adjust the stage of Special Attack (make Special Attacks stronger or weaker).
        /// </summary>
        ATTACK_SPECIAL,
        /// <summary>
        /// Adjust the stage of Physical Defense (reduce or increase incoming Attack Stat).
        /// </summary>
        DEFENSE_PHYSICAL,
        /// <summary>
        /// Adjust the stage of Special Defense (reduce or increase incoming Attack Stat).
        /// </summary>
        DEFENSE_SPECIAL,
        /// <summary>
        /// Adjsut the stage of Luck (surviving Fatal Hit, make Crit Damage, reduce incoming Damage, heal more).
        /// </summary>
        LUCK
    }

    protected BaseItem(int animationID, string itemName, string description, IntendedTarget target)
    {
        if (animationID == 0)
        {
            Debug.LogError("ERROR: Animation ID attempted to be set as zero!! Need non-zero value for: " + itemName);
            return;
        }
        this.animationID = animationID;
        this.target = target;
        this.itemName = itemName;
        this.description = description;
    }

    /// <summary>
    /// Get the Animation ID.
    /// </summary>
    /// <returns></returns>
    public int GetAnimationID()
    {
        return animationID;
    }

    /// <summary>
    /// Get the Base Item's name.
    /// </summary>
    /// <returns></returns>
    public string GetItemName()
    {
        return itemName;
    }

    /// <summary>
    /// Get the Base Item's basic description.
    /// </summary>
    /// <returns></returns>
    public string GetDescription()
    {
        return description;
    }

    /// <summary>
    /// Return the Intended target of this item.
    /// </summary>
    /// <returns></returns>
    public IntendedTarget GetIntendedTarget()
    {
        return target;
    }

    /// <summary>
    /// Return where the User should move to prior to using this BaseItem.
    /// </summary>
    /// <returns></returns>
    public WhereToMovePriorToUse GetWhereToMovePriorToUse()
    {
        return whereToMovePriorToUse;
    }

    /// <summary>
    /// Get the intended target in text form.
    /// </summary>
    /// <returns></returns>
    public string GetIntendedTargetString()
    {
        switch (target)
        {
            case IntendedTarget.SELF:
                return "Self";
            case IntendedTarget.ENEMY_ONE:
                return "Enemy";
            case IntendedTarget.ENEMY_ALL:
                return "All Enemies";
            case IntendedTarget.EVERYONE:
                return "Everyone";
            default:
                return "DEBUG: " + target.ToString();
        }
    }

    /// <summary>
    /// Return the Classification of this item.
    /// </summary>
    /// <returns></returns>
    public ItemClassification GetClassification()
    {
        return classification;
    }

    /// <summary>
    /// Get all Status Ailments an item may inflict.
    /// </summary>
    /// <returns></returns>
    public Dictionary<UnitStats.StatusAilment, int>.KeyCollection GetStatusAilments(Dictionary<UnitStats.StatusAilment, int> ailmentsList)
    {
        return ailmentsList.Keys;
    }

    /// <summary>
    /// Get all Item Effect Types of an item.
    /// </summary>
    /// <returns></returns>
    public Dictionary<StatEffectType, int>.KeyCollection GetItemEffects(Dictionary<StatEffectType, int> effectsList)
    {
        return effectsList.Keys;
    }

    
}
