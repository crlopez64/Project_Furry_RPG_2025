using NUnit.Framework.Constraints;
using System;
using UnityEngine;
/// <summary>
/// Action Command for rapidally pressing 1 button.
/// </summary>
public class ActionCommandRapidPress : ActionCommand
{
    private RapidPressSelectType subType;
    private bool degradeGoal = false;
    private float rapidPressGoal = 0.0f;
    private float rapidPressProgress = 0.0f;
    private float[] rapidPressControlRageGoal;

    /// <summary>
    /// The more specific type of Rapid Press Action Command.
    /// </summary>
    public enum RapidPressSelectType : byte
    {
        SHORT,
        MEDIUM,
        LONG,
        CONTROL_RAGE
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        // If relevant, Slider Stuff
        if (slider != null)
        {
            if (ReachedGoal())
            {
                slider.SetSliderValue(rapidPressProgress, ActionCommandSlider.SliderSetColor.IN_RANGE);
            }
            else
            {
                slider.SetSliderValue(rapidPressProgress, ActionCommandSlider.SliderSetColor.NOT_IN_RANGE);
            }
        }

        // Timer Stuff
        if (TimerActive())
        {
            actionCommandTimer -=Time.deltaTime;
            if (degradeGoal)
            {
                rapidPressProgress -= (Time.deltaTime * 0.3f);
                if (rapidPressProgress <= 0.0f)
                {
                    rapidPressProgress = 0f;
                }
            }
        }
        else
        {
            if (ReachedGoal())
            {
                actionCommandManager.ReportActionCommandPass(actionType, gameObject.transform);
                TurnOffThisGui();
                Debug.Log("Reached goal!!");
            }
            else
            {
                if (actionCommandManager.IsCurrentActionCommand(this))
                {
                    actionCommandManager.ReportActionCommandFail(actionType, gameObject.transform);
                    TurnOffThisGui();
                    Debug.Log("FAILED ACTION COMMAND!!");
                }
                else
                {
                    actionCommandManager.ReportActionCommandEndedBeforeReady(this);
                }
            }
        }
    }
    private void OnEnable()
    {
        if (slider != null)
        {
            slider.gameObject.SetActive(true);
        }
        animator.SetTrigger("SetRapid");
        animator.SetInteger("ButtonSelected", (int)buttonRequired);
    }

    /// <summary>
    /// Get the final String of this Attack Command after considering this subtype.
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
    public static string GetRapidPressSubTypeString(RapidPressSelectType subType)
    {
        if (subType == RapidPressSelectType.CONTROL_RAGE)
        {
            return "RAGE CONTROL";
        }
        return "RAPID PRESS";
    }

    /// <summary>
    /// Add small progress to this Rapid Press Action.
    /// Do nothing is not the active Action Command.
    /// </summary>
    /// <param name="inputName"></param>
    public override void CorrectInputPressed()
    {
        base.CorrectInputPressed();
        if (!actionCommandManager.IsCurrentActionCommand(this))
        {
            return;
        }
        rapidPressProgress += (Time.deltaTime * (subType == RapidPressSelectType.CONTROL_RAGE ? 18 : 10));
        if (ReachedGoal())
        {
            if (actionType == ActionType.RAPID_PRESS)
            {
                actionCommandManager.ReportActionCommandPass(actionType, gameObject.transform);
                TurnOffThisGui();
                Debug.Log("Reached goal!!");
            }
        }
    }

    /// <summary>
    /// Ignore if Player pressed wrong input.
    /// Do nothing is not the active Action Command.
    /// </summary>
    /// <param name="inputName"></param>
    public override void WrongInputPressed()
    {
        base.WrongInputPressed();
        if (!actionCommandManager.IsCurrentActionCommand(this)) 
        {
            return;
        }
        if (degradeGoal)
        {
            DecreaseProgress(1f);
        }
    }

    /// <summary>
    /// Make the Rapid Press last a short while. Goal is made short.
    /// </summary>

    public void SetRapidPressShort(ActionButtonPressed buttonToPress)
    {
        Debug.Log("Set Rapid Press SHORT");
        PrepareRapidPress(buttonToPress);
        actionCommandTimer = 2f;
        rapidPressGoal = 0.4f;
        subType = RapidPressSelectType.SHORT;
    }

    /// <summary>
    /// Make the Rapid Press last a moderate time. Goal is made medium.
    /// </summary>
    public void SetRapidPressMedium(ActionButtonPressed buttonToPress)
    {
        Debug.Log("Set Rapid Press MEDIUM");
        PrepareRapidPress(buttonToPress);
        actionCommandTimer = 3f;
        rapidPressGoal = 0.8f;
        subType = RapidPressSelectType.MEDIUM;
    }

    /// <summary>
    /// Make the Rapid Press last a while. Goal is made long time.
    /// </summary>
    public void SetRapidPressLong(ActionButtonPressed buttonToPress)
    {
        Debug.Log("Set Rapid Press LONG");
        PrepareRapidPress(buttonToPress);
        actionCommandTimer = 5f;
        rapidPressGoal = 1.2f;
        subType = RapidPressSelectType.LONG;
    }

    /// <summary>
    /// Make the Rapid Press degrade slowly while the Action Command remains active. Action Command should only succeed is within a specific range.
    /// </summary>
    /// <param name="ratioTarget">The target ratio to succeed this command (between 0.0f to 1.0f).</param>
    /// <param name="acceptableHalfRange">The accepted half range near the target ratio to succeed this command (between 0 to 25).</param>
    public void SetRapidPressControlRage(float ratioTarget, int acceptableHalfRange, ActionButtonPressed buttonToPress, ActionCommandSlider slider)
    {
        Debug.Log("Set Rapid Press CONTROL RAGE");
        degradeGoal = true;
        actionType = ActionType.RAPID_PRESS;
        subType = RapidPressSelectType.CONTROL_RAGE;
        buttonRequired = buttonToPress;
        rapidPressProgress = 0.0f;
        actionCommandTimer = 10f;
        rapidPressGoal = 2f; //Acts as maximum value instead
        ratioTarget = Mathf.Clamp01(ratioTarget);
        float realRatioTarget = rapidPressGoal * ratioTarget;
        acceptableHalfRange = Mathf.Clamp(acceptableHalfRange, 0, 25);
        float acceptableHalfRangeRatio = (float)acceptableHalfRange / 100;
        //TODO: If Accepable range bleeds otuside of 0% or 100%, clamp ratio target to end of halfRange
        rapidPressControlRageGoal = new float[2] { (realRatioTarget - acceptableHalfRangeRatio), (realRatioTarget + acceptableHalfRangeRatio) };
        this.slider = slider;
        this.slider.PrepareSlider(rapidPressGoal, ActionCommandSlider.SliderSetColor.NOT_IN_RANGE);
        //this.slider.gameObject.SetActive(true);
        Vector3 sliderPosition = new Vector3 (transform.position.x, transform.position.y - 80, transform.position.z);
        this.slider.gameObject.transform.position = sliderPosition;
    }


    public override GameObject TurnOffThisGui()
    {
        Debug.Log("Turn off Rapid Press GUI");
        if (nextActionCommand == null)
        {
            actionCommandManager.ClearOutLatestPreparedActionCommand();
        }
        else
        {
            nextActionCommand = null;
        }
        return gameObject;
    }

    /// <summary>
    /// Decrease step progress if required.
    /// </summary>
    /// <param name="step"></param>
    private void DecreaseProgress(float step)
    {
        rapidPressProgress -= step;
        if (rapidPressProgress < 0.0f)
        {
            rapidPressProgress = 0.0f;
        }
    }

    /// <summary>
    /// Set Rapid Press parameters and zero out progress.
    /// </summary>
    /// <param name="buttonToPress"></param>
    private void PrepareRapidPress(ActionButtonPressed buttonToPress)
    {
        slider = null;
        degradeGoal = false;
        actionType = ActionType.RAPID_PRESS;
        buttonRequired = buttonToPress;
        rapidPressProgress = 0.0f;
    }

    /// <summary>
    /// Did Player reach the goal?
    /// </summary>
    /// <returns></returns>
    private bool ReachedGoal()
    {
        if (subType == RapidPressSelectType.CONTROL_RAGE)
        {
            return rapidPressProgress >= rapidPressControlRageGoal[0] && rapidPressProgress <= rapidPressControlRageGoal[1];
        }
        return rapidPressProgress >= rapidPressGoal;
    }
}
