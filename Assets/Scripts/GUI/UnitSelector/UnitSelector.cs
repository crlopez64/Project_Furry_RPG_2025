using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

/// <summary>
/// Class that is in charge of selecting a unit and hovering over a Unit.
/// </summary>
public class UnitSelector : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public UnitSelectorManager unitSelectorManager;
    private GameObject unit;
    private Image image;
    private Button button;
    private bool makeFloat;
    private bool toggleGoalPosition;
    private Vector3 velocityRef;
    private Vector3 maxPosition;
    private Vector3 minPosition;
    private Vector3 currentGoal;

    private void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();

        //TODO: For Team, turn on button, but make inactive1
    }

    private void Update()
    {
        if (makeFloat)
        {
            transform.position = Vector3.SmoothDamp(transform.position, currentGoal, ref velocityRef, 0.3f);
            if (Vector3.Distance(transform.position, currentGoal) < 1f)
            {
                ToggleCurrentGoal();
            }
        }
        else
        {
            transform.position = minPosition;
        }
    }
    /// <summary>
    /// Expose the Arrow selecting this button.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnSelect(BaseEventData eventData)
    {
        makeFloat = true;
        unitSelectorManager.SetUnitSelected(unit);
    }
    /// <summary>
    /// Make dissappear the Arrow pointing this button.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDeselect(BaseEventData eventData)
    {
        makeFloat = false;
    }

    /// <summary>
    /// Have the button hover over the unit and turn on button.
    /// </summary>
    /// <param name="unit"></param>
    public void TurnOnButton(GameObject unit, UnitSelectorManager unitSelectorManager)
    {
        this.unit = unit;
        this.unitSelectorManager = unitSelectorManager;
        toggleGoalPosition = false;
        Bounds getBounds = unit.GetComponentInChildren<Renderer>().bounds;
        Vector3 getOnScreenTopBound = Camera.main.WorldToScreenPoint(getBounds.extents);
        Vector3 getPosition = Camera.main.WorldToScreenPoint(unit.transform.position);
        minPosition = new Vector3(getPosition.x, getPosition.y + (2*getOnScreenTopBound.y/3), 0);
        maxPosition = new Vector3(getPosition.x, getPosition.y + (2*getOnScreenTopBound.y/3) + (getOnScreenTopBound.y/12), 0);
        transform.position = minPosition;
        currentGoal = maxPosition;
        button.interactable = true;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Turn off the interactable for this button.
    /// </summary>
    public void TurnOnButtonFake()
    {
        button.interactable = false;
    }

    /// <summary>
    /// Turn off this button and stop making it float.
    /// </summary>
    public void TurnOffButton()
    {
        unit = null;
        makeFloat = false;
        toggleGoalPosition = false;
        minPosition = Vector3.zero;
        maxPosition = Vector3.zero;
        currentGoal = Vector3.zero;
        gameObject.SetActive(false);
    }

    private void ToggleCurrentGoal()
    {
        toggleGoalPosition = !toggleGoalPosition;
        currentGoal = toggleGoalPosition ? minPosition : maxPosition;
    }
}
