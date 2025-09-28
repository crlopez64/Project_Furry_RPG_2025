using UnityEngine;

public class ActionCommand : MonoBehaviour
{
    protected ActionCommandManager actionCommandManager;
    protected ActionCommand nextActionCommand;
    protected ActionCommandSlider slider;
    protected Animator animator;
    protected float actionCommandTimer = 0f;
    protected ActionType actionType;
    protected ActionButtonPressed buttonRequired;

    /// <summary>
    /// The type of task the Player will do during an Attack.
    /// </summary>
    public enum ActionType : byte
    {
        //RAPID_PRESS - Rapidly press button to succeed 1 Action
        //SEQUENCE_PRESS - Press sequence of buttons to succeed array of Actions
        //TIMELY_PRESS - Press button at correct time to succeed Action or array of Actions
        //STICK_CONTROL - Hold stick until specified time or location to complete 1 Action
        NONE,
        RAPID_PRESS,
        SEQUENCE_PRESS,
        TIMELY_PRESS,
        STICK_CONTROL
    }

    /// <summary>
    /// A required button to press for a task.
    /// </summary>
    public enum ActionButtonPressed : byte
    {
        NONE,
        BUTTON_SOUTH,
        BUTTON_EAST,
        BUTTON_NORTH,
        BUTTON_WEST
    }

    /// <summary>
    /// A required direction for the stick to be moved to for a task.
    /// </summary>
    public enum ActionStickFlicked : byte
    {
        NONE,
        STICK_TO_SOUTH,
        STICK_TO_EAST,
        STICK_TO_NORTH,
        STICK_TO_WEST
    }

    /// <summary>
    /// Return an array of all the buttons required.
    /// </summary>
    /// <returns></returns>
    public static ActionButtonPressed[] GetAllButtonsRequired()
    {
        return new ActionButtonPressed[]
        {
            ActionButtonPressed.BUTTON_SOUTH,
            ActionButtonPressed.BUTTON_EAST,
            ActionButtonPressed.BUTTON_NORTH,
            ActionButtonPressed.BUTTON_WEST
        };
    }

    /// <summary>
    /// Set the ActionCommandManager to report to.
    /// </summary>
    /// <param name="actionCommandManager"></param>
    public void SetActionCommandManager(ActionCommandManager actionCommandManager)
    {
        this.actionCommandManager = actionCommandManager;
    }

    /// <summary>
    /// Set the next Action Command in sequence.
    /// </summary>
    /// <param name="nextActionCommand"></param>
    public void SetNextActionCommand(ActionCommand nextActionCommand)
    {
        this.nextActionCommand = nextActionCommand;
    }

    /// <summary>
    /// Set that player used correct input.
    /// </summary>
    public virtual void CorrectInputPressed()
    {
        Debug.Log("Correct input placed.");
    }

    /// <summary>
    /// Set that player used wrong input.
    /// </summary>
    public virtual void WrongInputPressed()
    {
        Debug.LogWarning("Placed wrong input.");
    }

    /// <summary>
    /// Return the ActionType of this Action Command.
    /// </summary>
    /// <returns></returns>
    public ActionType GetActionType()
    {
        return actionType;
    }

    /// <summary>
    /// Was the correct button pressed?
    /// </summary>
    /// <param name="buttonPressed"></param>
    /// <returns></returns>
    public bool IsCorrectButtonPressed(ActionButtonPressed buttonPressed)
    {
        return buttonPressed == buttonRequired;
    }

    /// <summary>
    /// Does this Action Command have a Slider being used?
    /// </summary>
    /// <returns></returns>
    public bool HasSlider()
    {
        return slider != null;
    }

    /// <summary>
    /// Return the next Action Command
    /// </summary>
    /// <returns></returns>
    public ActionCommand GetNextActionCommand()
    {
        return nextActionCommand;
    }

    /// <summary>
    /// Return the Slider being used, if any.
    /// </summary>
    /// <returns></returns>
    public ActionCommandSlider ReturnSlider()
    {
        ActionCommandSlider getSlider = slider;
        slider = null;
        return getSlider;
    }

    /// <summary>
    /// Turn off this Action GUI and anything else related to its immediate function.
    /// Make sure this is the very last command an Action Command runs.
    /// </summary>
    public virtual GameObject TurnOffThisGui()
    {
        Debug.Log("Turning off Action Command.");
        return gameObject;
    }

    /// <summary>
    /// Has the Action Type been set?
    /// </summary>
    /// <returns></returns>
    protected bool ActionTypeSet()
    {
        return actionType != ActionType.NONE;
    }

    /// <summary>
    /// Is the ActionTimer Active?
    /// </summary>
    /// <returns></returns>
    protected bool TimerActive()
    {
        return actionCommandTimer > 0.0f;
    }

}
