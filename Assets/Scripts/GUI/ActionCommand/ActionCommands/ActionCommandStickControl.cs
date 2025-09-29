using UnityEngine;

public class ActionCommandStickControl : ActionCommand
{
    private StickControlType controlType;
    private bool holdingCorrectInput;
    private float sliderProgress = 0.0f;
    private float sliderGoal = 0.0f;

    //TODO: Two Types of Command Stick: Hold Stick to 1 Direction, or Flick Left and Right

    /// <summary>
    /// The more specific type of Stick Control Type Action Command.
    /// </summary>
    public enum StickControlType : byte
    {
        HOLD_STICK_QUICK,
        HOLD_STICK_MEDIUM,
        HOLD_STICK_LONG,
        FLICK_STICK_SHORT,
        FLICK_STICK_LONG
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        // TODO: Figure out how Stick Control will work
    }

    private void Update()
    {
        // Slider Stuff
        if (ReachedGoal())
        {
            slider.SetSliderValue(sliderProgress, ActionCommandSlider.SliderSetColor.IN_RANGE);
        }
        else
        {
            slider.SetSliderValue(sliderProgress, ActionCommandSlider.SliderSetColor.NOT_IN_RANGE);
        }
    }
    /// <summary>
    /// Add progress to correct input.
    /// </summary>
    public override void CorrectInputPressed()
    {
        base.CorrectInputPressed();
        holdingCorrectInput = true;
        sliderProgress += Time.deltaTime;
        if (ReachedGoal())
        {
            actionCommandManager.ReportActionCommandPass(gameObject.transform, bonusTypeOnActionCommand);
        }
    }

    /// <summary>
    /// Fail task if Hold Stick was let go too early, or do nothing otherwise.
    /// </summary>
    public override void WrongInputPressed()
    {
        base.WrongInputPressed();
        if (HoldStickActionCommand())
        {
            if (holdingCorrectInput)
            {
                actionCommandManager.ReportActionCommandFail(ActionType.STICK_CONTROL, gameObject.transform);
                TurnOffThisGui();
            }
        }
    }

    /// <summary>
    /// Make Stick Control such that the Player requires holding a stick in a particular direction for a moment.
    /// </summary>
    public void SetHoldStickQuick()
    {
        PrepareStickCommand(0.5f, 1f, StickControlType.HOLD_STICK_QUICK);
    }

    /// <summary>
    /// Make Stick Control such that the Player requires holding a stick in a particular direction for a bit.
    /// </summary>
    public void SetHoldStickMedium()
    {
        PrepareStickCommand(1f, 1.5f, StickControlType.HOLD_STICK_MEDIUM);
    }

    /// <summary>
    /// Make Stick Control such that the Player requires holding a stick in a particular direction for a good while.
    /// </summary>
    public void SetHoldStickLong()
    {
        PrepareStickCommand(2f, 2.5f, StickControlType.HOLD_STICK_LONG);
    }

    /// <summary>
    /// Make Stick Control such that the Player requires flicking a stick in a particular direction for a bit.
    /// </summary>
    public void SetFlickStickShort()
    {
        PrepareStickCommand(1f, 1.5f, StickControlType.FLICK_STICK_SHORT);
    }

    /// <summary>
    /// Make Stick Control such that the Player requires flicking a stick in a particular direction for a good while.
    /// </summary>
    public void SetFlickStickLong()
    {
        PrepareStickCommand(3f, 4f, StickControlType.FLICK_STICK_SHORT);
    }

    /// <summary>
    /// Prepare the Stick Command.
    /// </summary>
    /// <param name="sliderGoal"></param>
    private void PrepareStickCommand(float sliderGoal, float timerSet, StickControlType controlType)
    {
        holdingCorrectInput = false;
        sliderProgress = 0.0f;
        actionCommandTimer = timerSet;
        this.sliderGoal = sliderGoal;
        this.controlType = controlType;
        slider.PrepareSlider(sliderGoal, ActionCommandSlider.SliderSetColor.NOT_IN_RANGE);
    }

    /// <summary>
    /// Has Player reached Goal?
    /// </summary>
    /// <returns></returns>
    private bool ReachedGoal()
    {
        return sliderProgress >= sliderGoal;
    }

    /// <summary>
    /// Is the asking Action Command asking for Player to hold stick?
    /// </summary>
    /// <returns></returns>
    private bool HoldStickActionCommand()
    {
        return (int)controlType < 3;
    }
}
