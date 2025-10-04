using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of a Hero's inventory and equipment.
/// </summary>
public class HeroItems : MonoBehaviour
{
    private Item[] equipment;
    private List<Item> personalInventory;

    private void Awake()
    {
        equipment = new Item[6];
        personalInventory = new List<Item>(20);
    }
    private void Start()
    {
        
    }

    /// <summary>
    /// Set this Hero's equipment.
    /// </summary>
    /// <param name="equipment"></param>
    public void SetEquipment(Item[] equipment)
    {
        this.equipment = equipment;
    }

    /// <summary>
    /// Set this Hero's personal inventory, up to 20 items.
    /// </summary>
    /// <param name="inventory"></param>
    public void SetPersonalInventory(Item[] inventory)
    {
        personalInventory = new List<Item>(20);
        for(int i = 0; i < inventory.Length; i++)
        {
            if (i >= personalInventory.Capacity)
            {
                return;
            }
            personalInventory[i] = inventory[i];
        }
    }

    /// <summary>
    /// Add to inventory.
    /// </summary>
    /// <param name="item"></param>
    public void AddToInventory(Item item)
    {
        personalInventory.Add(item);
    }

    /// <summary>
    /// Equip an item.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="itemIndexedAt"></param>
    public void EquipItem(Item item, int itemIndexedAt)
    {
        if (!item.IsEquipment())
        {
            return;
        }
        if (!EquipSlotEmpty(item.GetEquipmentLocation()))
        {
            equipment[(int)(item.GetEquipmentLocation() - 1)] = item;
            personalInventory.Remove(item);
        }
        else
        {
            //TODO: Update stats from removing item and adding new item.
            Item removeFromEquipment = equipment[(int)(item.GetEquipmentLocation() - 1)];
            equipment[(int)(item.GetEquipmentLocation() - 1)] = item;
            personalInventory[itemIndexedAt] = removeFromEquipment;
        }
    }

    /// <summary>
    /// Can this Hero add to personal Inventory?
    /// </summary>
    /// <returns></returns>
    public bool CanAddToInventory()
    {
        return personalInventory.Count < 20;
    }

    /// <summary>
    /// Is the Equipment slot empty?
    /// </summary>
    /// <param name="equipLocation"></param>
    /// <returns></returns>
    public bool EquipSlotEmpty(Item.EquipLocation equipLocation)
    {
        return equipment[(int)(equipLocation - 1)] == null;
    }
}
