using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Script to create the Item Button during a menu requiring a list of inventory or movelist.
/// </summary>
public class ItemButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private ItemsMenuManager itemsMenuManager;
    private ItemButtonName itemName;
    private ItemButtonValue itemValue;
    private Button button;
    private Image image;
    private BaseItem item;

    private void Awake()
    {
        itemName = GetComponentInChildren<ItemButtonName>();
        itemValue = GetComponentInChildren<ItemButtonValue>();
        button = GetComponentInChildren<Button>();
        image = GetComponentInChildren<Image>();
    }

    /// <summary>
    /// Set the button.
    /// </summary>
    /// <param name="itemNameString"></param>
    /// <param name="valueString"></param>
    public void PrepareButton(BaseItem item, ItemsMenuManager itemsMenuManager)
    {
        this.itemsMenuManager = itemsMenuManager;
        this.item = item;
        itemName.SetText(item.GetItemName());
        switch(item.GetClassification())
        {
            case BaseItem.ItemClassification.ATTACK:
                Attack getAttack = (Attack)item;
                itemValue.SetText(getAttack.GetRequiredMana().ToString());
                break;
            case BaseItem.ItemClassification.ITEM:
                itemValue.SetText("x000");
                break;
        }
        
        button.interactable = true;
        image.color = Color.clear;
    }

    public void TurnOnImage()
    {
        image.color = new Color(1f, 1f, 1f, 1f);
    }

    /// <summary>
    /// Clear out the button.
    /// </summary>
    public void ClearButton()
    {
        item = null;
        button.interactable = false;
        itemName.SetText("");
        itemValue.SetText("");
        image.color = Color.clear;
    }

    /// <summary>
    /// Expose the Arrow selecting this button.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnSelect(BaseEventData eventData)
    {
        image.color = Color.white;
        itemsMenuManager.UpdateDescription(item, item.GetClassification());
        itemsMenuManager.SetCurrentItemHighlighted(gameObject);
    }

    /// <summary>
    /// Make dissappear the Arrow pointing this button.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDeselect(BaseEventData eventData)
    {
        image.color = Color.clear;
    }

    /// <summary>
    /// Return Base Item.
    /// </summary>
    /// <returns></returns>
    public BaseItem GetBaseItem()
    {
        return item;
    }
}
