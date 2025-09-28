using System.Collections.Generic;

/// <summary>
/// Item that can alleviate the User, double-edge the User, or harm the target.
/// </summary>
public class Item : BaseItem
{
    //TODO: Deal with Images
    private readonly ItemType itemType;
    private readonly Dictionary<UnitStats.StatusAilment, int> statusAilments;
    private readonly byte statusAilmentChance;

    public enum ItemType : byte
    {
        CONSUME,
        THROWABLE,
        EQUIPPABLE
    }

    /// <summary>
    /// Constructor for creating an Item to use.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="itemType"></param>
    /// <param name="statValues"></param>
    public Item(int animationID, string name, string description, ItemType itemType, IntendedTarget target, Dictionary<StatEffectType, int> statValues, Dictionary<UnitStats.StatusAilment, int> statusAilments, byte statusAilmentChance)
        : base (animationID, name, description, target)
    {
        whereToMovePriorToUse = WhereToMovePriorToUse.STAY_IN_PLACE;
        classification = ItemClassification.ITEM;
        this.itemType = itemType;
        this.statusAilments = statusAilments;
        this.statusAilmentChance = statusAilmentChance;
    }

    /// <summary>
    /// Can this item be equipped?
    /// </summary>
    /// <returns></returns>
    public bool CanEquip()
    {
        return itemType == ItemType.EQUIPPABLE;
    }

    /// <summary>
    /// Return Item Type of this item.
    /// </summary>
    /// <returns></returns>
    public ItemType GetItemType()
    {
        return itemType;
    }
}
