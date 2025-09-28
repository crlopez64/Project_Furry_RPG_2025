using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static ActionCommand;
using static UnityEditor.Progress;

/// <summary>
/// Manager in charge of listing out a List of Items, be it actual Items or Skills List.
/// </summary>
public class ItemsMenuManager : MonoBehaviour
{
    private EventSystem eventSystem;
    private ItemsMenuTitleOfMenu titleOfMenu;
    private ItemDescriptionMain description;
    private List<GameObject> buttons;
    private string currentItemHighlightedString;
    private GameObject currentItemHighlighted;

    private void Awake()
    {
        eventSystem = EventSystem.current;
        titleOfMenu = GetComponentInChildren<ItemsMenuTitleOfMenu>(true);
        description = GetComponentInChildren<ItemDescriptionMain>(true);
        description.AllocateDescriptionParts();
        AllocateButtons();
        FindAnyObjectByType<GameManager>().SetItemsManager(this);
    }

    /// <summary>
    /// Set the Event System such that the last button highlighted is to be re-highlighted again.
    /// </summary>
    public void ReOpenMenu()
    {
        Debug.Log("Re-Open a menu again... ");
        currentItemHighlightedString = currentItemHighlighted.GetComponent<ItemButton>().GetBaseItem().GetItemName().ToUpper();
        Debug.Log("Supposed item: " + currentItemHighlightedString);
        currentItemHighlighted.GetComponent<ItemButton>().TurnOnImage();
        eventSystem.SetSelectedGameObject(currentItemHighlighted);
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Open up the menu and show the list of Attacks.
    /// </summary>
    /// <param name="setTitle"></param>
    /// <param name="items"></param>
    public void OpenMenu(string setTitle, List<Attack> items)
    {
        titleOfMenu.SetText(setTitle);
        if(items != null)
        {
            for (int i = 0; i < items.Count; i++)
            {
                buttons[i].GetComponent<ItemButton>().PrepareButton(items[i], this);
                buttons[i].SetActive(true);
            }
        }
        if (description == null)
        {
            description = GetComponentInChildren<ItemDescriptionMain>(true);
        }
        if (items == null)
        {
            UpdateDescription();
        }
        else if (items.Count == 0)
        {
            UpdateDescription();
        }
        else
        {
            UpdateDescription(items[0], items[0].GetClassification());
        }

        //Set First Button
        eventSystem.SetSelectedGameObject(buttons[0]);
        buttons[0].GetComponent<ItemButton>().TurnOnImage();
        currentItemHighlighted = buttons[0];
        currentItemHighlightedString = currentItemHighlighted.GetComponent<ItemButton>().GetBaseItem().GetItemName().ToUpper();
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Open up the menu and show list of Items from the Inventory.
    /// </summary>
    /// <param name="setTitle"></param>
    /// <param name="items"></param>
    public void OpenMenu(string setTitle, List<Item> items)
    {
        titleOfMenu.SetText(setTitle);
        if (items != null)
        {
            for (int i = 0; i < items.Count; i++)
            {
                buttons[i].GetComponent<ItemButton>().PrepareButton(items[i], this);
                buttons[i].SetActive(true);
            }
        }
        if (description == null)
        {
            description = GetComponentInChildren<ItemDescriptionMain>(true);
        }
        if (items == null)
        {
            UpdateDescription();
        }
        else if (items.Count == 0)
        {
            UpdateDescription();
        }
        else
        {
            UpdateDescription(items[0], items[0].GetClassification());
        }

        //Set First Button
        eventSystem.SetSelectedGameObject(buttons[0]);
        buttons[0].GetComponent<ItemButton>().TurnOnImage();
        currentItemHighlighted = buttons[0];
        currentItemHighlightedString = currentItemHighlighted.GetComponent<ItemButton>().GetBaseItem().GetItemName().ToUpper();
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Set current Item highlighted without updating the Event System.
    /// </summary>
    /// <param name="item"></param>
    public void SetCurrentItemHighlighted(GameObject itemButton)
    {
        currentItemHighlighted = itemButton;
        currentItemHighlightedString = currentItemHighlighted.GetComponent<ItemButton>().GetBaseItem().GetItemName().ToUpper();
    }

    /// <summary>
    /// Wipe out Item Menu but keep the backpanel turned on.
    /// </summary>
    public void ClearOutItemsMenu()
    {
        if (titleOfMenu == null)
        {
            titleOfMenu = GetComponentInChildren<ItemsMenuTitleOfMenu>(true);
        }
        if (description == null)
        {
            description = GetComponentInChildren<ItemDescriptionMain>(true);
        }
        currentItemHighlighted = null;
        currentItemHighlightedString = "";
        titleOfMenu.ClearText();
        description.ClearText();
        eventSystem.SetSelectedGameObject(null);
        DisableButtons();
    }

    /// <summary>
    /// Show an empty Description.
    /// </summary>
    public void UpdateDescription()
    {
        description.ClearText();
        description.gameObject.SetActive(true);
    }

    /// <summary>
    /// Update the description of an item.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="classification"></param>
    public void UpdateDescription(BaseItem item, BaseItem.ItemClassification classification)
    {
        switch (classification)
        {
            case BaseItem.ItemClassification.ATTACK:
                Attack getAttack = (Attack)item;
                description.SetText(
                    getAttack.GetDescription(),
                    getAttack.GetIntendedTargetString(),
                    getAttack.GetActionCommandSubTypes());
                break;
            case BaseItem.ItemClassification.ITEM:
                Item getItem = (Item)item;
                description.SetText(
                    getItem.GetDescription(),
                    "TODO",
                    "TODO");
                break;
            default:
                description.ClearText();
                break;
        }
        description.gameObject.SetActive(true);
    }

    /// <summary>
    /// Return current item highlighted.
    /// </summary>
    /// <returns></returns>
    public BaseItem GetItemHighlighted()
    {
        return currentItemHighlighted.GetComponent<ItemButton>().GetBaseItem();
    }

    /// <summary>
    /// Allocate and initialize buttons for Menu.
    /// </summary>
    private void AllocateButtons()
    {
        buttons = new List<GameObject>(20);
        GameObject getButton = GetComponentInChildren<ItemButton>(true).gameObject;
        if (getButton == null)
        {
            Debug.LogError("ERROR: Button is not present for Item Menu Manager to allocate more buttons!!");
        }
        buttons.Add(getButton);
        for (int i = 1; i < buttons.Capacity; i++)
        {
            GameObject newButton = Instantiate(getButton, getButton.transform.position, getButton.transform.rotation);
            Transform masterList = getButton.transform.parent;
            newButton.transform.SetParent(masterList);
            buttons.Add(newButton);
        }
    }

    /// <summary>
    /// Disable all the buttons in the menu.
    /// </summary>
    private void DisableButtons()
    {
        foreach(GameObject button in buttons)
        {
            button.GetComponent<ItemButton>().ClearButton();
            button.SetActive(false);
        }
    }
    
}
