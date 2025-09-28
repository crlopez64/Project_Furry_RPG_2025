using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Script in charge of buttons to select a Unit or Units.
/// </summary>
public class UnitSelectorManager : MonoBehaviour
{
    private List<GameObject> unitSelectors;
    private UnitSelector unitSelector;
    private EventSystem eventSystem;
    private GameObject selectedUnit;
    private bool allocated;

    private void Awake()
    {
        eventSystem = EventSystem.current;
        unitSelector = GetComponentInChildren<UnitSelector>();
    }

    /// <summary>
    /// Turn on as many buttons as Units being given to be selected.
    /// </summary>
    /// <param name="unitStats"></param>
    public void TurnOnButtons(GameObject[] unitStats)
    {
        for(int i = 0; i < unitStats.Length; i++)
        {
            if (i >= unitSelectors.Capacity)
            {
                AllocateSelectorsAddOne(unitSelector);
            }
            unitSelectors[i].GetComponent<UnitSelector>().TurnOnButton(unitStats[i], this);
        }
        eventSystem.SetSelectedGameObject(unitSelectors[0]);
    }

    /// <summary>
    /// Turn off all buttons an clear out Unit Selected.
    /// </summary>
    public void ClearOutButtons()
    {
        selectedUnit = null;
        foreach(GameObject arrow in unitSelectors)
        {
            arrow.GetComponent<UnitSelector>().TurnOffButton();
            arrow.SetActive(false);
        }
    }

    /// <summary>
    /// Allocate 8 buttons.
    /// </summary>
    public void AllocateSelectors()
    {
        if (allocated)
        {
            return;
        }
        allocated = true;
        unitSelectors = new List<GameObject>(8);
        unitSelectors.Add(unitSelector.gameObject);
        for(int i = 1; i < unitSelectors.Capacity; i++)
        {
            AllocateSelectorsAddOne(unitSelector, false);
        }
    }

    /// <summary>
    /// Set the selected unit. Should only be used by UnitSelector.
    /// </summary>
    /// <param name="selectedUnit"></param>
    public void SetUnitSelected(GameObject selectedUnit)
    {
        this.selectedUnit = selectedUnit;
    }

    /// <summary>
    /// Return the selected unit.
    /// </summary>
    /// <returns></returns>
    public GameObject GetUnitSelected()
    {
        return selectedUnit;
    }

    /// <summary>
    /// Add 1 button to pool.
    /// </summary>
    /// <param name="unitSelector"></param>
    private void AllocateSelectorsAddOne(UnitSelector unitSelector, bool trimExcess = true)
    {
        GameObject newButton = Instantiate(unitSelector.gameObject, unitSelector.transform.position, unitSelector.transform.rotation);
        newButton.transform.SetParent(gameObject.transform);
        unitSelectors.Add(newButton);
        if (trimExcess)
        {
            unitSelectors.TrimExcess();
        }
    }

}
