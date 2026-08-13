using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Item that can alleviate the User, double-edge the User, or harm the target.
/// </summary>
public class Item : BaseItem
{
    //TODO: Deal with Images
    /// <summary>
    /// The list of Status Ailments this Item can recover from the target.
    /// </summary>
    private readonly List<UnitStats.StatusAilment> statusAilmentRecovery;
    /// <summary>
    /// The list of Status Ailments this Item can inflict on the target.
    /// </summary>
    private readonly List<UnitStats.StatusAilment> statusAilments;
    /// <summary>
    /// The list of Stat Effects this Item can apply to the target.
    /// </summary>
    private readonly Dictionary<StatEffectType, int> statValues;
    private readonly EquipLocation equipLocation;
    private readonly ItemType itemType;
    private readonly byte statusAilmentChance;

    /// <summary>
    /// The specific item type.
    /// </summary>
    public enum ItemType : byte
    {
        CONSUME,
        THROWABLE,
        EQUIPPABLE
    }

    /// <summary>
    /// The location this item is set for equip.
    /// </summary>
    public enum EquipLocation : byte
    {
        NONE,
        HEAD,
        TORSO,
        HAND_1,
        HAND_2,
        LEGS,
        UNIQUE_1,
        UNIQUE_2
    }

    /// <summary>
    /// Constructor for creating an Item to use.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="itemType"></param>
    /// <param name="statValues"></param>
    public Item(int animationID, string name, string description, ItemType itemType, EquipLocation equipLocation, IntendedTarget target,
        Dictionary<StatEffectType, int> statValues, List<UnitStats.StatusAilment> statusAilmentRecovery, List<UnitStats.StatusAilment> statusAilments, byte statusAilmentChance)
        : base (animationID, name, description, target)
    {
        whereToMovePriorToUse = WhereToMovePriorToUse.STAY_IN_PLACE;
        classification = ItemClassification.ITEM;
        this.statusAilmentChance = statusAilmentChance;
        this.statusAilmentRecovery = statusAilmentRecovery;
        this.statusAilments = statusAilments;
        this.equipLocation = equipLocation;
        this.statValues = statValues;
        this.itemType = itemType;
        if (statusAilmentChance > 100)
        {
            this.statusAilmentChance = 100;
        }
    }

    /// <summary>
    /// Add all stats from this item on the given unit.
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public bool EquipItem(UnitStats unit)
    {
        if (itemType != ItemType.EQUIPPABLE)
        {
            return false;
        }
        return unit.AddEquipmentStatChanges(this);
    }

    /// <summary>
    /// Remove all stats from this item on the given unit.
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public bool UnequipItem(UnitStats unit)
    {
        if (itemType != ItemType.EQUIPPABLE)
        {
            return false;
        }
        return unit.ReduceEquipmentStatChanges(this);
    }

    /// <summary>
    /// Use this item on the given unit.
    /// </summary>
    /// <param name="unit"></param>
    public bool UseItem(UnitStats unit)
    {
        bool usedForSomething = false;
        if (itemType == ItemType.EQUIPPABLE)
        {
            return usedForSomething;
        }
        if (statusAilments.Count > 0)
        {
            usedForSomething |= unit.AddStatusAilments(this);
        }
        if (statusAilmentRecovery.Count > 0)
        {
            usedForSomething |= unit.RemoveStatusAilments(this);
        }
        if (statValues.Count > 0)
        {
            usedForSomething |= unit.AddStatChanges(this);
        }
        return usedForSomething;
    }

    /// <summary>
    /// Return if the Status Ailment Chance is successful.
    /// </summary>
    /// <returns></returns>
    public bool StatusAilmentChanceSuccessful()
    {
        int chance = UnityEngine.Random.Range(0, 100);
        return chance <= statusAilmentChance;
    }

    /// <summary>
    /// Can this item be equipped?
    /// </summary>
    /// <returns></returns>
    public bool IsEquipment()
    {
        return itemType == ItemType.EQUIPPABLE;
    }

    /// <summary>
    /// Return the Equipment Location for this Item.
    /// </summary>
    /// <returns></returns>
    public EquipLocation GetEquipmentLocation()
    {
        return equipLocation;
    }

    /// <summary>
    /// Return Item Type of this item.
    /// </summary>
    /// <returns></returns>
    public ItemType GetItemType()
    {
        return itemType;
    }

    /// <summary>
    /// Return list of Stat Effects this Item can apply to the target.
    /// </summary>
    /// <returns></returns>
    public Dictionary<StatEffectType, int> GetStatValues()
    {
        return statValues;
    }

    /// <summary>
    /// Return list of Status Ailments this Item can inflict on the target.
    /// </summary>
    /// <returns></returns>
    public List<UnitStats.StatusAilment> GetStatusAilments()
    {
        return statusAilments;
    }

    /// <summary>
    /// Return list of Status Ailments this Item can recover from the target.
    /// </summary>
    /// <returns></returns>
    public List<UnitStats.StatusAilment> GetStatusAilmentsRecovery()
    {
        return statusAilmentRecovery;
    }
}
