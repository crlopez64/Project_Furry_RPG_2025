using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Script to test out the battle scene.
/// </summary>
public class TestBattleScene : MonoBehaviour
{
    private HeroStats[] testingHero;
    
    private InputAction battleScene_Move;
    private InputAction battleScene_Action;
    private InputAction battleScene_Action_ButtonNorth;
    private InputAction battleScene_Action_ButtonWest;
    private InputAction battleScene_Action_ButtonEast;
    private InputAction battleScene_Action_ButtonSouth;

    public InputActionAsset inputActionAsset;
    public ActionCommandManager actionCommandManager;
    public BattleManager battleManager;
    public GameManager gameManager;

    void Awake()
    {
        testingHero = FindObjectsByType<HeroStats>(FindObjectsSortMode.InstanceID);
        battleScene_Action = InputSystem.actions.FindAction("BattleScene_Select");
        battleScene_Action_ButtonNorth = InputSystem.actions.FindAction("BattleScene_Action_ButtonNorth");
        battleScene_Action_ButtonWest = InputSystem.actions.FindAction("BattleScene_Action_ButtonWest");
        battleScene_Action_ButtonEast = InputSystem.actions.FindAction("BattleScene_Action_ButtonEast");
        battleScene_Action_ButtonSouth = InputSystem.actions.FindAction("BattleScene_Action_ButtonSouth");
    }

    void Update()
    {
        /** Run Action Command once something in queued **/
        //if (actionCommandManager.ActionCommandEnqueued())
        //{
        //    if (!actionCommandManager.HasActionCommandToInteract())
        //    {
        //        actionCommandManager.ActivateNextActionCommand();
        //    }
        //}
        /** Prepare Action Commands **/
        //if (battleScene_Action.WasPressedThisFrame())
        //{
        //    if (!actionCommandManager.ActionCommandEnqueued())
        //    {
        //        //TestRapidPress();
        //        TestTimelyPress();
        //    }
        //}

        /** BEGIN BATTLE MANAGER **/
        //if (battleScene_Action.WasPressedThisFrame())
        //{
        //    if ((gameManager.testEnemies[0].gameObject.activeInHierarchy) && (gameManager.testEnemies[1].gameObject.activeInHierarchy) && (gameManager.testEnemies[2].gameObject.activeInHierarchy))
        //    {
        //        battleManager.TestMethodBeginBattle();
        //    }
        //    else
        //    {
        //        Debug.Log("Cannot begin battle");
        //    }
        //}

        //if (battleScene_Move.

        // TESTING BATTLE MANAGER
        if (battleScene_Action_ButtonNorth.WasPressedThisFrame())
        {
            battleManager.CheckButtonPressed(ActionCommand.ActionButtonPressed.BUTTON_NORTH);
        }
        if (battleScene_Action_ButtonEast.WasPressedThisFrame())
        {
            battleManager.CheckButtonPressed(ActionCommand.ActionButtonPressed.BUTTON_EAST);
        }
        if (battleScene_Action_ButtonWest.WasPressedThisFrame())
        {
            battleManager.CheckButtonPressed(ActionCommand.ActionButtonPressed.BUTTON_WEST);
        }
        if (battleScene_Action_ButtonSouth.WasPressedThisFrame())
        {
            battleManager.CheckButtonPressed(ActionCommand.ActionButtonPressed.BUTTON_SOUTH);
        }

        // TESTING ACTION COMMAND MANAGER DIRECTLY
        // CAN BE DONE TO DIRECTLY VALIDATE SEQUENCE_PRESS ACTION COMMAND
        //TODO: Once created, validate Sequence_Press Action Command
        // ---------------------------------
        //if (battleScene_Action_ButtonNorth.WasPressedThisFrame())
        //{
        //    actionCommandManager.CheckButtonPressed(ActionCommand.ActionButtonPressed.BUTTON_NORTH);
        //}
        //if (battleScene_Action_ButtonEast.WasPressedThisFrame())
        //{
        //    actionCommandManager.CheckButtonPressed(ActionCommand.ActionButtonPressed.BUTTON_EAST);
        //}
        //if (battleScene_Action_ButtonWest.WasPressedThisFrame())
        //{
        //    actionCommandManager.CheckButtonPressed(ActionCommand.ActionButtonPressed.BUTTON_WEST);
        //}
        //if (battleScene_Action_ButtonSouth.WasPressedThisFrame())
        //{
        //    actionCommandManager.CheckButtonPressed(ActionCommand.ActionButtonPressed.BUTTON_SOUTH);
        //}
    }
    void OnEnable()
    {
        inputActionAsset.FindActionMap("Player").Enable();
    }
    void OnDisable()
    {
        inputActionAsset.FindActionMap("Player").Disable();
    }

    /// <summary>
    /// Enqueue one Rapid Press Action Command with a random button.
    /// </summary>
    public void TestRapidPress()
    {
        int pickRandom = Random.Range(0, 4);
        //switch (pickRandom)
        //{
        //    case 0:
        //        actionCommandManager.PrepareRapidPress(ActionCommandRapidPress.RapidPressSelectType.SHORT, GetRandomButtonToRequire(), BaseItem.BonusTypeOnActionCommand.NONE);
        //        break;
        //    case 1:
        //        actionCommandManager.PrepareRapidPress(ActionCommandRapidPress.RapidPressSelectType.MEDIUM, GetRandomButtonToRequire(), BaseItem.BonusTypeOnActionCommand.NONE);
        //        break;
        //    case 2:
        //        actionCommandManager.PrepareRapidPress(ActionCommandRapidPress.RapidPressSelectType.LONG, GetRandomButtonToRequire(), BaseItem.BonusTypeOnActionCommand.NONE);
        //        break;
        //    case 3:
        //        //Ratio is set at 50% from Max Fill
        //        actionCommandManager.PrepareRapidPress(ActionCommandRapidPress.RapidPressSelectType.CONTROL_RAGE, GetRandomButtonToRequire(), BaseItem.BonusTypeOnActionCommand.NONE);
        //        break;
        //    default:
        //        break;
        //}
    }

    /// <summary>
    /// Enqueue a Timely Press Action Command with a random button.
    /// </summary>
    public void TestTimelyPress()
    {
        int pickRandom = Random.Range(0, 5);
        //switch (pickRandom)
        //{
        //    case 0:
        //        actionCommandManager.PrepareTimelyPress(ActionCommandTimelyPress.TimelyPressSpeedType.SLOW, GetRandomButtonToRequire(), BaseItem.BonusTypeOnActionCommand.NONE);
        //        break;
        //    case 1:
        //        actionCommandManager.PrepareTimelyPress(ActionCommandTimelyPress.TimelyPressSpeedType.MEDIUM, GetRandomButtonToRequire(), BaseItem.BonusTypeOnActionCommand.NONE);
        //        break;
        //    case 2:
        //        actionCommandManager.PrepareTimelyPress(ActionCommandTimelyPress.TimelyPressSpeedType.QUICK, GetRandomButtonToRequire(), BaseItem.BonusTypeOnActionCommand.NONE);
        //        break;
        //    case 3:
        //        actionCommandManager.PrepareTimelyPress(ActionCommandTimelyPress.TimelyPressSpeedType.HIGH_NOON_SLOW, GetRandomButtonToRequire(), BaseItem.BonusTypeOnActionCommand.NONE);
        //        break;
        //    case 4:
        //        actionCommandManager.PrepareTimelyPress(ActionCommandTimelyPress.TimelyPressSpeedType.HIGH_NOON_QUICK, GetRandomButtonToRequire(), BaseItem.BonusTypeOnActionCommand.NONE);
        //        break;
        //    default:
        //        break;
        //}
    }

    private ActionCommand.ActionButtonPressed GetRandomButtonToRequire()
    {
        int pickRandom = Random.Range(0, 4);
        return (ActionCommand.ActionButtonPressed)pickRandom;
    }
}
