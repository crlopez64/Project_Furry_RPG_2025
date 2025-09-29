using System;
using UnityEngine;
using UnityEngine.UI;
using static ActionCommandRapidPress;

/// <summary>
/// Action Command for pressing 1 timely press.
/// </summary>
public class ActionCommandTimelyPress : ActionCommand
{
    private TimelyPressSpeedType subType;
    private bool setTimelyReadyAnim;
    private bool highNoonDrawn;
    private float sliderProgress = 0.0f;
    private float sliderGoal = 0.0f;
    private float depleterMultiplier = 1f;

    /// <summary>
    /// The more specific type of Timely Press Action Command.
    /// </summary>
    public enum TimelyPressSpeedType : byte
    {
        SLOW,
        MEDIUM,
        QUICK,
        HIGH_NOON_SLOW,
        HIGH_NOON_QUICK
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        setTimelyReadyAnim = false;
        highNoonDrawn = false;
    }
    private void Update()
    {
        // Activate High Noon Draw
        if (HighNoonActionCommand())
        {
            if (!HighNoonDrawn())
            {
                if (ReadyToActivateHighNoonButton())
                {
                    animator.SetInteger("ButtonSelected", (int)buttonRequired);
                    highNoonDrawn = true;
                }
            }
            
        }

        // Determine if it's ready for Player to press button
        if (DrainerReadyForPlayer())
        {
            slider.SetSliderValue(sliderProgress, ActionCommandSlider.SliderSetColor.TIMELY_READY);
            if (!setTimelyReadyAnim)
            {
                animator.SetTrigger("SetTimelyReady");
                setTimelyReadyAnim = true;
            }
        }
        else
        {
            slider.SetSliderValue(sliderProgress, ActionCommandSlider.SliderSetColor.TIMELY_NOT_READY);
        }

        // Drain goes down
        if (DrainerDepleted())
        {
            if (actionCommandManager.IsCurrentActionCommand(this))
            {
                actionCommandManager.ReportActionCommandFail(actionType, gameObject.transform);
            }
            else
            {
                actionCommandManager.ReportActionCommandEndedBeforeReady(this);
            }
        }
        else
        {
            sliderProgress -= (Time.deltaTime * depleterMultiplier);
        }
    }
    private void OnEnable()
    {
        animator.SetTrigger("SetTimely");
        if (slider != null)
        {
            slider.gameObject.SetActive(true);
        }
        if (HighNoonActionCommand())
        {
            animator.SetInteger("ButtonSelected", (int)(ActionButtonPressed.BUTTON_WEST + 1));
        }
        else
        {
            animator.SetInteger("ButtonSelected", (int)buttonRequired);
        }
    }

    /// <summary>
    /// Get the final String of this Attack Command after considering this subtype.
    /// </summary>
    /// <param name="subType"></param>
    /// <returns></returns>
    public static string GetTimelyPressSubTypeString(TimelyPressSpeedType subType)
    {
        if ((subType == TimelyPressSpeedType.HIGH_NOON_SLOW) || (subType == TimelyPressSpeedType.HIGH_NOON_QUICK))
        {
            return "HIGH NOON";
        }
        return "TIMELY PRESS";
    }

    /// <summary>
    /// Determine if button was pressed at correct time or not.
    /// </summary>
    public override void CorrectInputPressed()
    {
        base.CorrectInputPressed();
        if (!actionCommandManager.IsCurrentActionCommand(this))
        {
            return;
        }
        if (DrainerReadyForPlayer())
        {
            actionCommandManager.ReportActionCommandPass(gameObject.transform, bonusTypeOnActionCommand);
        }
        else
        {
            actionCommandManager.ReportActionCommandFail(actionType, gameObject.transform);
        }
    }

    /// <summary>
    /// Completely fail this Action Command due to wrong input.
    /// </summary>
    public override void WrongInputPressed()
    {
        base.WrongInputPressed();
        actionCommandManager.ReportActionCommandFail(actionType, gameObject.transform);
    }

    /// <summary>
    /// Make the Timely Press last a while before requiring button press.
    /// </summary>
    /// <param name="slider"></param>
    /// <param name="buttonToPress"></param>
    public void SetTimelyPressSlow(ActionCommandSlider slider, ActionButtonPressed buttonToPress, BaseItem.BonusTypeOnActionCommand bonusTypeOnActionCommand)
    {
        PrepareTimelyPress(TimelyPressSpeedType.SLOW, ActionType.TIMELY_PRESS, buttonToPress, bonusTypeOnActionCommand);
        PrepareSlider(slider, TimelyPressSpeedType.SLOW);
    }

    /// <summary>
    /// Make the Timely Press last a bit of a while before requiring button press.
    /// </summary>
    /// <param name="slider"></param>
    /// <param name="buttonToPress"></param>
    public void SetTimelyPressMedium(ActionCommandSlider slider, ActionButtonPressed buttonToPress, BaseItem.BonusTypeOnActionCommand bonusTypeOnActionCommand)
    {
        PrepareTimelyPress(TimelyPressSpeedType.MEDIUM, ActionType.TIMELY_PRESS, buttonToPress, bonusTypeOnActionCommand);
        PrepareSlider(slider, TimelyPressSpeedType.MEDIUM);
    }

    /// <summary>
    /// Make the Timely Press last a short while before requiring button press.
    /// </summary>
    /// <param name="slider"></param>
    /// <param name="buttonToPress"></param>
    public void SetTimelyPressQuick(ActionCommandSlider slider, ActionButtonPressed buttonToPress, BaseItem.BonusTypeOnActionCommand bonusTypeOnActionCommand)
    {
        PrepareTimelyPress(TimelyPressSpeedType.QUICK, ActionType.TIMELY_PRESS, buttonToPress, bonusTypeOnActionCommand);
        PrepareSlider(slider, TimelyPressSpeedType.QUICK);
    }

    /// <summary>
    /// Make the Timely Press last a while before requiring a random button press.
    /// </summary>
    /// <param name="slider"></param>
    public void SetHighNoonPressSlow(ActionCommandSlider slider, BaseItem.BonusTypeOnActionCommand bonusTypeOnActionCommand)
    {
        PrepareTimelyPress(TimelyPressSpeedType.HIGH_NOON_SLOW, ActionType.TIMELY_PRESS, SelectRandomButton(), bonusTypeOnActionCommand);
        PrepareSlider(slider, TimelyPressSpeedType.HIGH_NOON_SLOW);
    }

    /// <summary>
    /// Make the Timely Press last a short while before requiring a random button press.
    /// </summary>
    /// <param name="slider"></param>
    public void SetHighNoonPressFast(ActionCommandSlider slider, BaseItem.BonusTypeOnActionCommand bonusTypeOnActionCommand)
    {
        PrepareTimelyPress(TimelyPressSpeedType.HIGH_NOON_QUICK, ActionType.TIMELY_PRESS, SelectRandomButton(), bonusTypeOnActionCommand);
        PrepareSlider(slider, TimelyPressSpeedType.HIGH_NOON_QUICK);
    }

    /// <summary>
    /// Prepare the Timely Press stuff.
    /// </summary>
    /// <param name="speedType"></param>
    private void PrepareTimelyPress(TimelyPressSpeedType speedType, ActionType actionType, ActionButtonPressed buttonToPress, BaseItem.BonusTypeOnActionCommand bonusTypeOnActionCommand)
    {
        this.bonusTypeOnActionCommand = bonusTypeOnActionCommand;
        this.actionType = actionType;
        buttonRequired = buttonToPress;
        setTimelyReadyAnim = false;
        highNoonDrawn = false;
        subType = speedType;
    }

    /// <summary>
    /// Prepare the Slider.
    /// </summary>
    /// <param name="actionType"></param>
    /// <param name="speedType"></param>
    private void PrepareSlider(ActionCommandSlider slider, TimelyPressSpeedType speedType)
    {
        // TODO: On late development: actually want slider or moer dynamic progress item?
        this.slider = slider;
        sliderProgress = 3.0f;
        SetSliderGoal(speedType);
        slider.PrepareSlider(sliderProgress, ActionCommandSlider.SliderSetColor.TIMELY_NOT_READY);
        float halfWidth = (slider.GetComponent<RectTransform>().rect.width/2) + (GetComponentInChildren<RectTransform>().rect.width/2);
        Vector3 sliderPosition = new Vector3(transform.position.x + halfWidth, transform.position.y - 35, transform.position.z);
        slider.gameObject.transform.position = sliderPosition;
    }

    /// <summary>
    /// Return the goal required for Player to press button.
    /// </summary>
    /// <param name="speedType"></param>
    /// <returns></returns>
    private void SetSliderGoal(TimelyPressSpeedType speedType)
    {
        switch(speedType)
        {
            case TimelyPressSpeedType.SLOW:
                sliderGoal = 0.8f;
                depleterMultiplier = 2.5f;
                return;
            case TimelyPressSpeedType.MEDIUM:
                sliderGoal = 0.8f;
                depleterMultiplier = 3.0f;
                return;
            case TimelyPressSpeedType.QUICK:
                sliderGoal = 0.8f;
                depleterMultiplier = 4.0f;
                return;
            case TimelyPressSpeedType.HIGH_NOON_SLOW:
                sliderGoal = 0.8f;
                depleterMultiplier = 2.5f;
                break;
            case TimelyPressSpeedType.HIGH_NOON_QUICK:
                sliderGoal = 0.8f;
                depleterMultiplier = 3.5f;
                return;
            default:
                sliderGoal = 0.01f;
                depleterMultiplier = 10.0f;
                return;
        }
    }

    /// <summary>
    /// Was High Noon already activated?
    /// </summary>
    /// <returns></returns>
    private bool HighNoonDrawn()
    {
        return highNoonDrawn;
    }

    /// <summary>
    /// Should High Noon activate its real button?
    /// </summary>
    /// <returns></returns>
    private bool ReadyToActivateHighNoonButton()
    {
        return (sliderProgress <= sliderGoal + 0.2f);
    }

    /// <summary>
    /// Is Player currently doing a High Noon Action Command?
    /// </summary>
    /// <returns></returns>
    private bool HighNoonActionCommand()
    {
        return (int)subType >= 3;
    }

    /// <summary>
    /// Is it time to press button at correct time?
    /// </summary>
    /// <returns></returns>
    private bool DrainerReadyForPlayer()
    {
        return sliderProgress <= sliderGoal;
    }

    /// <summary>
    /// Has the Drainer depleted?
    /// </summary>
    /// <returns></returns>
    private bool DrainerDepleted()
    {
        return sliderProgress <= 0f;
    }

    /// <summary>
    /// Select a random button for Player to press.
    /// </summary>
    /// <returns></returns>
    private ActionButtonPressed SelectRandomButton()
    {
        Array buttons = Enum.GetValues(typeof(ActionButtonPressed));
        int value = UnityEngine.Random.Range(0, buttons.Length);
        return (ActionButtonPressed)buttons.GetValue(value);
    }
}
