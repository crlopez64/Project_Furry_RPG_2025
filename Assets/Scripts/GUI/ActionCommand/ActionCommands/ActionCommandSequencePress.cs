using JetBrains.Annotations;
using UnityEngine;

/// <summary>
/// Action Command for pressing multiple buttons in order.
/// </summary>
public class ActionCommandSequencePress : ActionCommand
{
    private SequencePressType sequencePressType;
    private SequencePressTryCount tryCount;
    private int correctSoFar;
    private int sequenceGoal;

    /// <summary>
    /// Whether or not a mistake determines the Pass or Fail result of the Action Comand.
    /// </summary>
    public enum SequencePressTryCount : byte
    {
        ONE_LIFE,
        INFINITE_LIFE
    }

    /// <summary>
    /// The more specific type of Sequence Press Action Command.
    /// </summary>
    public enum SequencePressType
    {
        REVEAL_ALL,
        REVEAL_ONE,
        REVEAL_ONE_PARTIAL_OK
    }

    private void Update()
    {
        if (TimerActive())
        {
            actionCommandTimer -= Time.deltaTime;
        }
        else
        {
            //Determine if failed or report partial progress
        }
    }

    /// <summary>
    /// Move onto the next button in sequence or report Pass to Action Command Manager.
    /// </summary>
    public override void CorrectInputPressed()
    {
        Debug.Log("Correct input placed.");
        correctSoFar++;
        if (AllButtonsSuceeded())
        {
            actionCommandManager.ReportActionCommandPass(actionType, gameObject.transform);
        }
    }

    /// <summary>
    /// If ONE_LIFE, fail entire command sequence.
    /// If REVEAL_ONE, fail entire sequence
    /// If REVEAL_ONE_PARTIAL_OK, report Pass if at least 1 button is correct.
    /// </summary>
    public override void WrongInputPressed()
    {
        Debug.LogWarning("Placed wrong input.");
    }

    /// <summary>
    /// Set SequencePressRevealAll.
    /// </summary>
    /// <param name="sequencePressType"></param>
    public void SetSequencePressRevealAll(SequencePressType sequencePressType, SequencePressTryCount tryCount, ActionButtonPressed[] buttonsRequiredInSequence)
    {
        correctSoFar = 0;
        sequenceGoal = buttonsRequiredInSequence.Length;
        actionCommandTimer = 3f;
        this.tryCount = tryCount;
        this.sequencePressType = sequencePressType;
        //TODO: Figure out how to requier multiple buttons in order
    }

    /// <summary>
    /// Has the Player completed all button presses?
    /// </summary>
    /// <returns></returns>
    private bool AllButtonsSuceeded()
    {
        return correctSoFar > sequenceGoal;
    }
}
