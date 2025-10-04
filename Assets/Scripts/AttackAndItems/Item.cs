using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Item that can alleviate the User, double-edge the User, or harm the target.
/// </summary>
public class Item : BaseItem
{
    //TODO: Deal with Images
    private readonly Dictionary<UnitStats.StatusAilment, int> statusAilments;
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
        Dictionary<StatEffectType, int> statValues, Dictionary<UnitStats.StatusAilment, int> statusAilments, byte statusAilmentChance)
        : base (animationID, name, description, target)
    {
        whereToMovePriorToUse = WhereToMovePriorToUse.STAY_IN_PLACE;
        classification = ItemClassification.ITEM;
        this.statusAilmentChance = statusAilmentChance;
        this.statusAilments = statusAilments;
        this.equipLocation = equipLocation;
        this.statValues = statValues;
        this.itemType = itemType;
    }

    public void UseItem()
    {
        if (statusAilments.Count > 0)
        {
            if (statusAilmentChance > 0)
            {

            }
        }
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
    /// Use all Stat Changes associated with this Item.
    /// </summary>
    /// <param name="statValues"></param>
    private void AddStatChanges(Dictionary<StatEffectType, int> statValues)
    {
        Dictionary<StatEffectType, int>.KeyCollection keys = statValues.Keys;
        foreach (StatEffectType key in keys)
        {
            switch (key)
            {
                case StatEffectType.POWER:
                    continue;
                case StatEffectType.HEALTH_MAX:
                    continue;
                case StatEffectType.MANA_MAX:
                    continue;
                case StatEffectType.HEALTH_CURRENT:
                    continue;
                case StatEffectType.MANA_CURRENT:
                    continue;
                case StatEffectType.ATTACK_PHYSICAL:
                    if (IsEquipment())
                    {

                    }
                    else
                    {

                    }
                    continue;
                case StatEffectType.ATTACK_SPECIAL:
                    if (IsEquipment())
                    {

                    }
                    else
                    {

                    }
                    continue;
                case StatEffectType.DEFENSE_PHYSICAL:
                    if (IsEquipment())
                    {

                    }
                    else
                    {

                    }
                    continue;
                case StatEffectType.DEFENSE_SPECIAL:
                    if (IsEquipment())
                    {

                    }
                    else
                    {

                    }
                    continue;
                case StatEffectType.LUCK:
                    if (IsEquipment())
                    {

                    }
                    else
                    {

                    }
                    continue;
            }
        }
    }

    /// <summary>
    /// Use all Status Ailments associated with this Item.
    /// </summary>
    /// <param name="statusAilments"></param>
    private void UseStatusAilments(Dictionary<UnitStats.StatusAilment, int> statusAilments)
    {
        Dictionary<UnitStats.StatusAilment, int>.KeyCollection keys = statusAilments.Keys;
        foreach (UnitStats.StatusAilment key in keys)
        {
            switch (key)
            {
                case UnitStats.StatusAilment.HEARTY:
                    continue;
                case UnitStats.StatusAilment.STUNNED:
                    continue;
                case UnitStats.StatusAilment.ASLEEP:
                    continue;
                case UnitStats.StatusAilment.CONFUSED:
                    continue;
                case UnitStats.StatusAilment.PARALYZED:
                    continue;
                case UnitStats.StatusAilment.FRIGHTENED:
                    continue;
                case UnitStats.StatusAilment.ENRAGED:
                    continue;
                case UnitStats.StatusAilment.POISONED:
                    continue;
                case UnitStats.StatusAilment.EXHAUSTED:
                    continue;
            }
        }
    }
}
