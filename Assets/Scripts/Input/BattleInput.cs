using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Script in charge of User Input during battle.
/// </summary>
public class BattleInput : MonoBehaviour
{
    private BattleManager battleManager;
    private InputAction battleScene_Action_ButtonNorth;
    private InputAction battleScene_Action_ButtonWest;
    private InputAction battleScene_Action_ButtonEast;
    private InputAction battleScene_Action_ButtonSouth;

    public InputActionAsset inputActionAsset;

    void Awake()
    {
        // Allocate Components
        battleManager = GetComponent<BattleManager>();
        //TODO: Add stick when Stick Control is ready?
        battleScene_Action_ButtonNorth = InputSystem.actions.FindAction("BattleScene_Action_ButtonNorth");
        battleScene_Action_ButtonWest = InputSystem.actions.FindAction("BattleScene_Action_ButtonWest");
        battleScene_Action_ButtonEast = InputSystem.actions.FindAction("BattleScene_Action_ButtonEast");
        battleScene_Action_ButtonSouth = InputSystem.actions.FindAction("BattleScene_Action_ButtonSouth");

        //Activate Player
        inputActionAsset.FindActionMap("Player").Enable();
    }

    private void Update()
    {
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
    }
}
