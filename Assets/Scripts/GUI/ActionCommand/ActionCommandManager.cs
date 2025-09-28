using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager in charge of holding Action Commands for Player to interact with.
/// Does not manage Units damaging other Units; only reporting if an Action Command has succeeded or not.
/// </summary>
public class ActionCommandManager : MonoBehaviour
{
    private Queue<ActionCommand> actionCommandSequence = new Queue<ActionCommand>();
    private Stack<GameObject> timelyPressAvailable;
    private Stack<GameObject> rapidPressAvailable;
    private Stack<GameObject> slidersAvailable;
    private ActionCommandTimelyPress timelyPress;
    private ActionCommandRapidPress rapidPress;
    private ActionCommandSlider slider;

    /// <summary>
    /// The last Action Command to have been prepared.
    /// </summary>
    private ActionCommand latestPreparedActionCommand;
    /// <summary>
    /// The current Action Command the Player will interact with.
    /// </summary>
    private ActionCommand currentActionCommand;

    private TestActionCommandResult testActionCommandResult;

    /// <summary>
    /// Allocate all Gui Action Command components.
    /// </summary>
    public void AllocateGuiComponents()
    {
        Debug.Log("Allocating GUI Components");
        AllocateBaseGui();

        //Sliders
        slidersAvailable = new Stack<GameObject>(3);
        slidersAvailable.Push(slider.gameObject);
        for (int i = 0; i < 2; i++)
        {
            AddToPool(slidersAvailable, slider.gameObject);
        }

        //Rapid Press
        rapidPressAvailable = new Stack<GameObject>(1);
        rapidPressAvailable.Push(rapidPress.gameObject);

        //Timely Presses
        timelyPressAvailable = new Stack<GameObject>(3);
        timelyPressAvailable.Push(timelyPress.gameObject);
        for (int i = 0; i < 2; i++)
        {
            AddToPool(timelyPressAvailable, timelyPress.gameObject);
        }

        //Have all ActionCommand GUI report to this Manager
        ReportPoolToManager(rapidPressAvailable);
        ReportPoolToManager(timelyPressAvailable);
    }

    /// <summary>
    /// Clear out Latest Prepared Action Command to prevent bugs in future.
    /// </summary>
    public void ClearOutLatestPreparedActionCommand()
    {
        latestPreparedActionCommand = null;
    }

    /// <summary>
    /// Check if correct button was pressed.
    /// </summary>
    /// <param name="actionButtonPressed"></param>
    public void CheckButtonPressed(ActionCommand.ActionButtonPressed actionButtonPressed)
    {
        if (!HasActionCommandToInteract())
        {
            Debug.LogWarning("There's no Action Command to interact with.");
            return;
        }
        if (currentActionCommand.IsCorrectButtonPressed(actionButtonPressed))
        {
            currentActionCommand.CorrectInputPressed();
        }
        else
        {
            currentActionCommand.WrongInputPressed();
        }
    }

    /// <summary>
    /// Prepare a Rapid Press Action for Player to interact with. Only 1 should be active at any given moment.
    /// </summary>
    /// <param name="pressType"></param>
    /// <param name="buttonRequired"></param>
    public void PrepareRapidPress(ActionCommandRapidPress.RapidPressSelectType pressType, ActionCommand.ActionButtonPressed buttonRequired)
    {
        if (!ActionCommandAvailableToActivate(rapidPressAvailable))
        {
            Debug.Log("Do not have enough Rapid Press Action Commands in Pool. Cancelling request.");
            return;
        }
        GameObject getAction = rapidPressAvailable.Pop();
        switch (pressType)
        {
            case ActionCommandRapidPress.RapidPressSelectType.SHORT:
                getAction.GetComponent<ActionCommandRapidPress>().SetRapidPressShort(buttonRequired);
                break;
            case ActionCommandRapidPress.RapidPressSelectType.MEDIUM:
                getAction.GetComponent<ActionCommandRapidPress>().SetRapidPressMedium(buttonRequired);
                break;
            case ActionCommandRapidPress.RapidPressSelectType.LONG:
                getAction.GetComponent<ActionCommandRapidPress>().SetRapidPressLong(buttonRequired);
                break;
            case ActionCommandRapidPress.RapidPressSelectType.CONTROL_RAGE:
                GameObject getSlider = slidersAvailable.Pop();
                getAction.GetComponent<ActionCommandRapidPress>().SetRapidPressControlRage(0.5f, 20, buttonRequired, getSlider.GetComponent<ActionCommandSlider>());
                break;
            default:
                break;
        }
        SetNextActionCommandAndEnqueueToSequence(getAction);
    }

    /// <summary>
    /// Prepare a Timely Press Action for player to interact with.
    /// </summary>
    /// <param name="pressType"></param>
    /// <param name="buttonRequired"></param>
    public void PrepareTimelyPress(ActionCommandTimelyPress.TimelyPressSpeedType pressType, ActionCommand.ActionButtonPressed buttonRequired)
    {
        if (!ActionCommandAvailableToActivate(timelyPressAvailable))
        {
            Debug.Log("Do not have enough Timely Press Action Commands in Pool. Cancelling request.");
            return;
        }
        GameObject getAction = timelyPressAvailable.Pop();
        getAction.transform.position = new Vector3(Random.Range(410f, 900f), Random.Range(150f, 500f), 0f);
        GameObject getSlider = slidersAvailable.Pop();
        switch (pressType)
        {
            case ActionCommandTimelyPress.TimelyPressSpeedType.SLOW:
                getAction.GetComponent<ActionCommandTimelyPress>().SetTimelyPressSlow(getSlider.GetComponent<ActionCommandSlider>(), buttonRequired);
                break;
            case ActionCommandTimelyPress.TimelyPressSpeedType.MEDIUM:
                getAction.GetComponent<ActionCommandTimelyPress>().SetTimelyPressMedium(getSlider.GetComponent<ActionCommandSlider>(), buttonRequired);
                break;
            case ActionCommandTimelyPress.TimelyPressSpeedType.QUICK:
                getAction.GetComponent<ActionCommandTimelyPress>().SetTimelyPressQuick(getSlider.GetComponent<ActionCommandSlider>(), buttonRequired);
                break;
            case ActionCommandTimelyPress.TimelyPressSpeedType.HIGH_NOON_SLOW:
                timelyPress.SetHighNoonPressSlow(slider);
                getAction.GetComponent<ActionCommandTimelyPress>().SetHighNoonPressSlow(getSlider.GetComponent<ActionCommandSlider>());
                break;
            case ActionCommandTimelyPress.TimelyPressSpeedType.HIGH_NOON_QUICK:
                getAction.GetComponent<ActionCommandTimelyPress>().SetHighNoonPressFast(getSlider.GetComponent<ActionCommandSlider>());
                break;
            default:
                break;
        }
        SetNextActionCommandAndEnqueueToSequence(getAction);
    }

    /// <summary>
    /// Take a loose Slider gameObject and place it back to the pool. Do nothing if already inside pool.
    /// </summary>
    /// <param name="slider"></param>
    public void SetSliderBackToPool(ActionCommandSlider slider)
    {
        Debug.Log("Set Slider back to Pool");
        if (slidersAvailable.Contains(slider.gameObject))
        {
            return;
        }
        slidersAvailable.Push(slider.gameObject);
    }

    /// <summary>
    /// Activate the next Action Command for Player to interact with.
    /// </summary>
    public void ActivateNextActionCommand()
    {
        if (!ActionCommandEnqueued())
        {
            Debug.LogWarning("NOTE: ActionCommand queue is empty!!");
            return;
        }
        currentActionCommand = actionCommandSequence.Dequeue();
        if (!currentActionCommand.gameObject.activeInHierarchy)
        {
            currentActionCommand.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Highlight the next un-enabled Action Command in queue. If currentActionCommand is empty, this does nothing.
    /// </summary>
    public void HighlightNextActionCommandInQueue()
    {
        if (currentActionCommand == null)
        {
            return;
        }
        ActionCommand[] getArray = actionCommandSequence.ToArray();
        for(int i = 0; i < getArray.Length; i++)
        {
            if (!getArray[i].gameObject.activeInHierarchy)
            {
                getArray[i].gameObject.SetActive(true);
                return;
            }
        }
    }

    /// <summary>
    /// Let Action Command report back that Player has succeeded task.
    /// </summary>
    /// <param name="actionType"></param>
    /// <param name="transform"></param>
    public void ReportActionCommandPass(ActionCommand.ActionType actionType, Transform transform)
    {
        Debug.Log("Reporting Success!!");
        testActionCommandResult.gameObject.transform.position = transform.position;
        testActionCommandResult.SetText("SUCCESS");
        testActionCommandResult.gameObject.SetActive(true);
        ReturnActionCommands();
    }

    /// <summary>
    /// Let Action Command report back that Player failed task.
    /// </summary>
    /// <param name="actionType"></param>
    /// <param name="transform"></param>
    public void ReportActionCommandFail(ActionCommand.ActionType actionType, Transform transform)
    {
        Debug.LogWarning("Reporting Fail...");
        testActionCommandResult.gameObject.transform.position = transform.position;
        testActionCommandResult.SetText("FAIL");
        testActionCommandResult.gameObject.SetActive(true);
        ReturnActionCommands();
    }

    /// <summary>
    /// Debug method call in case an Action Command fails while not the currentActionCommand.
    /// </summary>
    /// <param name="actionCommand"></param>
    public void ReportActionCommandEndedBeforeReady(ActionCommand actionCommand)
    {
        Debug.LogError("ERROR: Somehow failed action command before it was ready for Player to interact with!" +
            "\nAction Command: " + actionCommand.gameObject.name);
    }

    /// <summary>
    /// Clear out all Action Commands GUI. Keep GameObject manager on.
    /// </summary>
    public void ClearOut()
    {
        ClearOutPool(slidersAvailable);
        ClearOutPool(rapidPressAvailable);
        ClearOutPool(timelyPressAvailable);
    }

    /// <summary>
    /// Return if there's any more Action Commands in queue to run.
    /// </summary>
    /// <returns></returns>
    public bool ActionCommandEnqueued()
    {
        return actionCommandSequence.Count > 0;
    }

    /// <summary>
    /// Return if there's an Action Command in progress.
    /// </summary>
    /// <returns></returns>
    public bool HasActionCommandToInteract()
    {
        return currentActionCommand != null;
    }
    
    /// <summary>
    /// Return if the action command specified is the one to interact with.
    /// </summary>
    /// <param name="actionCommand"></param>
    /// <returns></returns>
    public bool IsCurrentActionCommand(ActionCommand actionCommand)
    {
        return actionCommand == currentActionCommand;
    }

    /// <summary>
    /// Allocate the base items.
    /// </summary>
    private void AllocateBaseGui()
    {
        slider = GetComponentInChildren<ActionCommandSlider>(true);
        rapidPress = GetComponentInChildren<ActionCommandRapidPress>(true);
        timelyPress = GetComponentInChildren<ActionCommandTimelyPress>(true);
        testActionCommandResult = GetComponentInChildren<TestActionCommandResult>(true);
        testActionCommandResult.ClearText();
    }

    /// <summary>
    /// Turn off the entire pool.
    /// </summary>
    /// <param name="pool"></param>
    private void ClearOutPool(Stack<GameObject> pool)
    {
        if (pool == null)
        {
            Debug.Log("Pool is null! Try to allocate GUI again. " + pool.ToString());
            AllocateGuiComponents();
        }
        if (pool.Count == 0)
        {
            Debug.LogError("ERROR: Pool exists but is empty!! " + pool.ToString());
        }
        foreach (GameObject item in pool)
            {
            item.SetActive(false);
        }
    }

    /// <summary>
    /// Instantiate a game object and place it to its respective pool.
    /// </summary>
    /// <param name="pool"></param>
    /// <param name="toCopy"></param>
    private void AddToPool(Stack<GameObject> pool, GameObject toCopy)
    {
        if (toCopy.GetType() != typeof(GameObject))
        {
            return;
        }
        GameObject instantiated = Instantiate(toCopy, toCopy.gameObject.transform.position, toCopy.gameObject.transform.rotation);
        instantiated.transform.SetParent(transform);
        pool.Push(instantiated);
    }
    /// <summary>
    /// Set all ActionCommand GUI within a pool to this manager.
    /// </summary>
    /// <param name="pool"></param>
    private void ReportPoolToManager(Stack<GameObject> pool)
    {
        foreach (GameObject item in pool)
        {
            item.GetComponent<ActionCommand>().SetActionCommandManager(this);
        }
    }

    /// <summary>
    /// Return a slider.
    /// </summary>
    /// <param name="slider"></param>
    public void ReturnSlider(ActionCommandSlider slider)
    {
        Debug.Log("Returning Slider");
        slider.gameObject.SetActive(false);
        slidersAvailable.Push(slider.gameObject);
    }

    /// <summary>
    /// Return Slider and currentActionCommand back to its pools.
    /// </summary>
    private void ReturnActionCommands()
    {
        Debug.Log("Return action command back to Pool...");
        //TODO: Once created, place back Sequence_Press and Stick_Control
        switch (currentActionCommand.GetActionType())
        {
            case ActionCommand.ActionType.RAPID_PRESS:
                ReturnToPools(rapidPressAvailable);
                break;
            case ActionCommand.ActionType.TIMELY_PRESS:
                ReturnToPools(timelyPressAvailable);
                break;
            default:
                return;
        }
    }

    /// <summary>
    /// Return the currentActionCommand back to its respective pool. If Action Command has Slider, turn it off.
    /// </summary>
    /// <param name="pool"></param>
    private void ReturnToPools(Stack<GameObject> pool)
    {
        ActionCommand nextActionCommand = currentActionCommand.GetNextActionCommand();
        //TurnOffThisGui() should take care of returning slider
        GameObject usedActionCommand = currentActionCommand.TurnOffThisGui();
        pool.Push(usedActionCommand);
        currentActionCommand = nextActionCommand;
        if (nextActionCommand == null)
        {
            Debug.Log("WAIT!!");
        }
    }

    /// <summary>
    /// Set Next Action Command when that Action Command deactivates; place current Action Command to Queue.
    /// </summary>
    /// <param name="getAction"></param>
    private void SetNextActionCommandAndEnqueueToSequence(GameObject getAction)
    {
        if (latestPreparedActionCommand != null)
        {
            Debug.Log("Replace latest prepared Action Command");
            getAction.GetComponent<ActionCommand>().SetNextActionCommand(latestPreparedActionCommand);
        }
        else
        {
            Debug.Log("Update latest Prepared Action CMD from null");
            latestPreparedActionCommand = getAction.GetComponent<ActionCommand>();
        }
        actionCommandSequence.Enqueue(getAction.GetComponent<ActionCommand>());
    }

    /// <summary>
    /// Return if the pool in question still has available Command Actions for Player to interact with.
    /// </summary>
    /// <param name="pool"></param>
    /// <returns></returns>
    private bool ActionCommandAvailableToActivate(Stack<GameObject> pool)
    {
        return pool.Count > 0;
    }

}
