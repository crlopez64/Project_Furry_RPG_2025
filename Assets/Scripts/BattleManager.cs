using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manager to keep track Flow of Battle.
/// </summary>
public class BattleManager : MonoBehaviour
{
    private GameManager gameManager;
    private CameraControl cameraControl;
    private ActionCommandManager actionCommandManager;
    private UnitSelectorManager unitSelectorManager;
    private UnitCurrentSignal unitCurrentSignal;
    private ItemsMenuManager itemsMenuManager;
    private MainBattleMenu mainBattleMenu;
    private HudNarrator hudNarrator;
    private GameObject currentUnitsTurn;
    private BaseItem getBaseItemFromUnit;
    private Queue<AttackStep> strikesPrepared;
    private UnitAttack[] targetsToUseBaseItem;
    private Queue<GameObject> turnOrder;
    private List<GameObject> enemies;
    private List<GameObject> heroes;
    private LayerMask unwalkableMask;
    private BattleStateForPlayer currentBattleState;
    private NavigatingMenus currentMenu;
    private bool currentBattleStateFinishedBool;

    public enum BattleStateForPlayer : byte
    {
        NONE,
        /// <summary>
        /// Introduce battle scene.
        /// </summary>
        INTRODUCE_BATTLE,
        /// <summary>
        /// Special case Player not in battle (attacking or defending).
        /// </summary>
        NOT_IN_BATTLE,
        /// <summary>
        /// A unit's turn has finished (Enemy finishing Attack; Player finishing Attack).
        /// </summary>
        FINISH_TURN,
        /// <summary>
        /// Enemy moving to its Target prior to attacking.
        /// </summary>
        ENEMY_MOVING_TO_TARGET,
        /// <summary>
        /// Enemy attacking.
        /// </summary>
        ENEMY_ATTACK,
        /// <summary>
        /// Enemy moving back to base once Attacking is finished.
        /// </summary>
        ENEMY_MOVING_BACK_TO_BASE,
        /// <summary>
        /// Player navigating menus to determine what to do.
        /// </summary>
        PLAYER_NAVIGATING_MENUS,
        /// <summary>
        /// Player moving to a location prior to attacking.
        /// </summary>
        PLAYER_MOVING_TO_TARGET,
        /// <summary>
        /// Player attacking.
        /// </summary>
        PLAYER_ATTACK,
        /// <summary>
        /// Player moving back to base once Attacking is finished.
        /// </summary>
        PLAYER_MOVING_BACK_TO_BASE
    }

    /// <summary>
    /// If during NAVIGATING_MENUS, determine which State Player is on.
    /// Else, always NONE
    /// </summary>
    private enum NavigatingMenus : byte
    {
        NONE,
        MAIN_MENU,
        ITEMS_MENU_INVENTORY,
        ITEMS_MENU_ATTACKS,
        ITEMS_MENU_SKILLS,
        SELECT_TARGET
    }

    private void Awake()
    {
        cameraControl = FindAnyObjectByType<CameraControl>();
        actionCommandManager = FindAnyObjectByType<ActionCommandManager>(FindObjectsInactive.Include);
        unitSelectorManager = FindAnyObjectByType<UnitSelectorManager>(FindObjectsInactive.Include);
        unitCurrentSignal = FindAnyObjectByType<UnitCurrentSignal>(FindObjectsInactive.Include);
        itemsMenuManager = FindAnyObjectByType<ItemsMenuManager>(FindObjectsInactive.Include);
        mainBattleMenu = FindAnyObjectByType<MainBattleMenu>(FindObjectsInactive.Include);
        hudNarrator = FindAnyObjectByType<HudNarrator>(FindObjectsInactive.Include);
        gameManager = FindAnyObjectByType<GameManager>(FindObjectsInactive.Include);
        if (FindAnyObjectByType<GameManager>() != null)
        {
            FindAnyObjectByType<GameManager>().SetBattleManager(this);
        }
        strikesPrepared = new Queue<AttackStep>(8);
    }
    private void Start()
    {
        // HUD Allocate
        PrepareHudNarrator();
        PrepareItemMenuManager();
        PrepareActionCommandManager();
        PrepareUnitSelectorManager();
        PrepareUnitCurrentSignal();
        CloseMainBattleMenu();
        unwalkableMask = LayerMask.GetMask("InfrastructureAndWalls");

        // Heroes and HUD Allocate
        heroes = gameManager.FindHeroes();
        gameManager.AssignHudToHero();

        // Prepare Heroes and Enemies
        List<string> getListOfEnemies = GameManager.GetEnemiesToBattle();
        PrepareEnemies(getListOfEnemies);
        PrepareHeroes();

        // Determine Turn Order
        DetermineTurnOrder(heroes, enemies); //TODO: Add surprise attacks?

        // Prepare Current State of Battle to beginning
        currentBattleState = BattleStateForPlayer.INTRODUCE_BATTLE;
        currentMenu = NavigatingMenus.NONE;

        //Begin battle!
        BeginBattle();
    }
    private void Update()
    {
        if (currentBattleStateFinishedBool)
        {
            currentBattleStateFinishedBool = false;
            GetNextTurn();
        }
    }

    //TODO: Add Prepare_Sequence_Press Command here
    //TODO: Add Prepare_Stick_Control Command here

    ///// <summary>
    ///// Prepare a Rapid Press Action Command per the Attack requesting it.
    ///// </summary>
    ///// <param name="actionCommandSubType"></param>
    //public void PrepareDebugRapidPress(string actionCommandSubType, ActionCommand.ActionButtonPressed buttonRequired)
    //{
    //    if (!Enum.TryParse(actionCommandSubType, out ActionCommandRapidPress.RapidPressSelectType getSubType))
    //    {
    //        Debug.LogError("ERROR: Could not get Action Command from the following string subtype: " + actionCommandSubType);
    //    }
    //    actionCommandManager.PrepareRapidPress(getSubType, buttonRequired);
    //}

    ///// <summary>
    ///// Prepare a Timely Press Action Command per the Attack requesting it.
    ///// </summary>
    ///// <param name="actionCommandSubType"></param>
    ///// <param name="buttonRequired"></param>
    //public void PrepareDebugTimelyPress(string actionCommandSubType, ActionCommand.ActionButtonPressed buttonRequired)
    //{
    //    if (!Enum.TryParse(actionCommandSubType, out ActionCommandTimelyPress.TimelyPressSpeedType getSubType))
    //    {
    //        Debug.LogError("ERROR: Could not get Action Command from the following string subtype: " + actionCommandSubType);
    //    }
    //    actionCommandManager.PrepareTimelyPress(getSubType, buttonRequired);
    //}

    /// <summary>
    /// Add Heroes to Battle Manager's attention.
    /// </summary>
    /// <param name="heroes"></param>
    public void PrepareHeroes()
    {
        MoveHeroesToLocation(heroes);
    }

    /// <summary>
    /// Add enemies to Battle Manager's attention.
    /// </summary>
    /// <param name="enemies"></param>
    public void PrepareEnemies(List<string> listOfEnemies)
    {
        SpawnEnemies(listOfEnemies);
        MoveEnemiesToLocation(enemies);
    }

    /// <summary>
    /// Begin the battle.
    /// </summary>
    private void BeginBattle()
    {
        if (currentBattleState != BattleStateForPlayer.INTRODUCE_BATTLE)
        {
            return;
        }
        Debug.LogWarning("BEGIN RPG BATTLE SEQUENCE!!!!");
        CurrentBattleStateFinished();
    }

    /// <summary>
    /// Open up the main battle menu to its first step.
    /// </summary>
    public void OpenMainBattleMenu()
    {
        currentMenu = NavigatingMenus.MAIN_MENU;
        mainBattleMenu.OpenMainMenu();
        mainBattleMenu.gameObject.SetActive(true);
        cameraControl.BattleBegin(heroes, enemies);
        CloseItemsMenu();
    }

    /// <summary>
    /// Close out the main battle menu.
    /// </summary>
    public void CloseMainBattleMenu()
    {
        currentMenu = NavigatingMenus.NONE;
        mainBattleMenu.gameObject.SetActive(false);
    }

    /// <summary>
    /// Set currentBattleStateFinished to true. If finishing Hero's turn, turn off their HUD saying it's their turn.
    /// </summary>
    public void CurrentBattleStateFinished()
    {
        Debug.Log("There's a Unit that finished!!");
        currentBattleStateFinishedBool = true;
        currentBattleState = BattleStateForPlayer.FINISH_TURN;
        currentMenu = NavigatingMenus.NONE;
        targetsToUseBaseItem = null;
        if (currentUnitsTurn == null)
        {
            return;
        }
        // Turn off the "NOW!" HUD Plate
        if (currentUnitsTurn.GetComponent<HeroStats>() != null)
        {
            currentUnitsTurn.GetComponent<HeroStats>().NoLongerHerosTurn();
        }
        unitCurrentSignal.ResetSignalToCharacter();
    }

    /// <summary>
    /// After validating it's the Player's turn, and the Hero is still alive, interpret a button pressed for an Action Command.
    /// </summary>
    /// <param name="buttonPressed"></param>
    public void CheckButtonPressed(ActionCommand.ActionButtonPressed buttonPressed)
    {
        //TODO: What happens on two button presses at the same time?
        if (!PlayersTurn())
        {
            Debug.LogWarning("It's not the Player's turn yet");
            return;
        }
        //TODO: Check if the chosen Hero's turn is still alive.
        switch (currentBattleState)
        {
            case BattleStateForPlayer.NOT_IN_BATTLE:
                // This is for if there's Menus or Dialogue to be running/skipping
                AllHeroesSurviveFatalHit();
                return;
            case BattleStateForPlayer.ENEMY_MOVING_TO_TARGET:
                //TODO: Have Heroes Block or Parry incoming attacks
                return;
            case BattleStateForPlayer.ENEMY_ATTACK:
                //TODO: Have Heroes Block or Parry incoming attacks
                return;
            case BattleStateForPlayer.ENEMY_MOVING_BACK_TO_BASE:
                //Do nothing.
                return;
            case BattleStateForPlayer.PLAYER_NAVIGATING_MENUS:
                //TODO: If currentHealth == 0 in the middle of navigating menus, cut them off and move onto next Unit in queue
                DetermineMenu(buttonPressed);
                return;
            case BattleStateForPlayer.PLAYER_MOVING_TO_TARGET:
                //Do nothing.
                return;
            case BattleStateForPlayer.PLAYER_ATTACK:
                // If in middle of Drowning in Actions on Fatal Hit (healthEstimate == 0), have Hero survive that Fatal Hit.
                HeroStats getHeroStats = currentUnitsTurn.GetComponent<HeroStats>();
                getHeroStats.SurviveFatalHit();
                actionCommandManager.CheckButtonPressed(buttonPressed);
                return;
            case BattleStateForPlayer.PLAYER_MOVING_BACK_TO_BASE:
                //Do nothing.
                return;
        }
    }

    /// <summary>
    /// Determine the Turn Order for everyone.
    /// </summary>
    /// <param name="heroes"></param>
    /// <param name="enemies"></param>
    public void DetermineTurnOrder(List<GameObject> heroes, List<GameObject> enemies)
    {
        List<GameObject> everyone = new(heroes.Count + enemies.Count);
        everyone.AddRange(heroes);
        everyone.AddRange(enemies);
        everyone = everyone.OrderByDescending(unit => unit.GetComponent<UnitStats>().GetStatSpeed()).
            ThenBy(unit => unit.GetComponent<UnitStats>().GetStatLuck()).
            ThenBy(unit => unit.GetComponent<UnitStats>().GetStatDefenseSpecial()).
            ThenBy(unit => unit.GetComponent<UnitStats>().GetStatDefensePhysical()).
            ThenBy(unit => unit.GetComponent<UnitStats>().GetStatAttackSpecial()).
            ThenBy(unit => unit.GetComponent<UnitStats>().GetStatAttackPhysical()).ToList();
        turnOrder = new Queue<GameObject>(everyone);
    }

    /// <summary>
    /// Get next Unit's turn and update current Battle State. Enqueue finished Unit's turn back into the Queue.
    /// If it's Player's turn, activate Battle Menu.
    /// </summary>
    public void GetNextTurn()
    {
        Debug.LogWarning("Next Turn!!");
        // Place back to Queue
        if (currentUnitsTurn != null)
        {
            turnOrder.Enqueue(currentUnitsTurn);
        }
        // Update Current Unit's Turn
        currentUnitsTurn = turnOrder.Dequeue();
        currentBattleState = (currentUnitsTurn.GetComponent<HeroStats>() != null) ?
            BattleStateForPlayer.PLAYER_NAVIGATING_MENUS : BattleStateForPlayer.ENEMY_MOVING_TO_TARGET;
        unitCurrentSignal.SetSignalToCharacter(currentUnitsTurn);
        //Turn on Menus if Player's turn
        if (currentBattleState != BattleStateForPlayer.PLAYER_NAVIGATING_MENUS)
        {
            CloseMainBattleMenu();
            return;
        }
        currentMenu = NavigatingMenus.MAIN_MENU;
        currentUnitsTurn.GetComponent<HeroStats>().ThisHerosTurn();
        OpenMainBattleMenu();
        TurnOnHudNarratorWithTimer("What will " + currentUnitsTurn.GetComponent<UnitStats>().GetUnitName() + " do?");
    }

    /// <summary>
    /// Advance the current unit turn's animation step. Within that animation, Action Command should be made.
    /// </summary>
    public void AdvanceNextAttackAnimation()
    {
        currentUnitsTurn.GetComponent<UnitAttack>().ActivateNextAttackStep();
    }

    /// <summary>
    /// Prepare an Action Command. Does not advance animation.
    /// </summary>
    public void PrepareNextActionCommand()
    {
        if (strikesPrepared == null)
        {
            Debug.LogError("Strikes prepared is empty!!");
            return;
        }
        if (strikesPrepared.Count <= 0)
        {
            Debug.Log("Strikes was empty. Skipping...");
            return;
        }
        AttackStep nextStrike = strikesPrepared.Dequeue();
        ActionCommand.ActionButtonPressed getButtonRequired = nextStrike.GetRequiredButton();
        switch (nextStrike.GetActionCommandType())
        {
            case ActionCommand.ActionType.RAPID_PRESS:
                Enum.TryParse(nextStrike.GetSubType(), out ActionCommandRapidPress.RapidPressSelectType subTypeRapidGet);
                actionCommandManager.PrepareRapidPress(nextStrike);
                break;
            case ActionCommand.ActionType.SEQUENCE_PRESS:
                //Enum.TryParse(attackStep.GetSubType(), out ActionCommandSequencePress.SequencePressType subTypeSequenceGet);
                //actionCommandManager.PrepareSequencePress(subTypeSequenceGet, getButtonRequired, nextStrike.GetBonusTypeOnActionCommand());
                Debug.LogError("ERROR: Sequence Press not yet implemented!!");
                break;
            case ActionCommand.ActionType.TIMELY_PRESS:
                actionCommandManager.PrepareTimelyPress(nextStrike);
                break;
            case ActionCommand.ActionType.STICK_CONTROL:
                Debug.LogError("ERROR: Stick Control not yet implemented!!");
                break;
        }
    }

    /// <summary>
    /// Update current BattleState to cut off Player input.
    /// </summary>
    public void ForceStopPlayerInput()
    {
        currentBattleState = BattleStateForPlayer.FINISH_TURN;
    }

    /// <summary>
    /// Place a Unit into the Turn Order if not already present in Queue.
    /// </summary>
    /// <param name="unit"></param>
    public void ResurrectUnit(UnitStats unit)
    {
        if (turnOrder.Contains(unit.gameObject))
        {
            return;
        }
        List<GameObject> newTurnOrderList = new(turnOrder);
        newTurnOrderList.Insert((newTurnOrderList[0].GetComponent<UnitStats>().GetStatSpeed() < unit.GetStatSpeed()) ? 0 : 2, unit.gameObject);
        turnOrder = new Queue<GameObject>(newTurnOrderList);
    }

    /// <summary>
    /// Remove a Unit from the Queue.
    /// </summary>
    /// <param name="unitToEliminate"></param>
    public void EliminateUnit(UnitStats unitToEliminate)
    {
        //Remove from queue
        List<GameObject> newTurnOrderList = new(turnOrder);
        foreach (GameObject unit in newTurnOrderList)
        {
            if (unit == unitToEliminate.gameObject)
            {
                newTurnOrderList.Remove(unit.gameObject);
                break;
            }
        }
        turnOrder = new Queue<GameObject>(newTurnOrderList);
    }

    /// <summary>
    /// Is it the Player's turn to attack?
    /// </summary>
    /// <returns></returns>
    public bool PlayersTurn()
    {
        //Anything above 5 are Player states
        return (int)currentBattleState >= 6;
    }

    /// <summary>
    /// Are all the Heroes defeated?
    /// </summary>
    /// <returns></returns>
    public bool HeroesDefeated()
    {
        int count = 0;
        foreach (GameObject unit in heroes)
        {
            if (unit.GetComponent<HeroStats>().IsDefeated())
            {
                count++;
            }
        }
        return count == heroes.Count;
    }

    /// <summary>
    /// Are all the Enemies defeated?
    /// </summary>
    /// <returns></returns>
    public bool EnemiesDefeated()
    {
        int count = 0;
        foreach (GameObject unit in enemies)
        {
            if (unit.GetComponent<EnemyStats>().IsDefeated())
            {
                count++;
            }
        }
        return count == enemies.Count;
    }

    /// <summary>
    /// Return the animation ID for Unit to refer to.
    /// </summary>
    /// <returns></returns>
    public int GetBaseItemAnimationID()
    {
        if (getBaseItemFromUnit == null)
        {
            Debug.LogError("ERROR: BaseItem is null at the time of getting Anim_ID!!");
            return -1;
        }
        return getBaseItemFromUnit.GetAnimationID();
    }

    /// <summary>
    /// Start cleaning up the End of Battle. Any Health Rolling stops immediately in its tracks.
    /// </summary>
    public void FinishBattle()
    {
        for (int i = 0; i < heroes.Count; i++)
        {
            heroes[i].GetComponent<HeroStats>().DamageRollerStop();
        }
    }

    /// <summary>
    /// Load up all Action Commands in the order the Attack was instantiated via Script from a Skill.
    /// </summary>
    /// <param name="attack"></param>
    /// <param name="targetsToAttack"></param>
    /// <param name="useActionCommands"></param>
    public void PrepareItem(Item item, UnitAttack[] targetsToUseBaseItem)
    {
        this.targetsToUseBaseItem = targetsToUseBaseItem;
        Debug.Log("Use item!!");
    }

    /// <summary>
    /// Attack all the Targets.
    /// </summary>
    /// <param name="attackStep"></param>
    public void AttackEnemies(AttackStep attackStep)
    {
        if (targetsToUseBaseItem == null)
        {
            Debug.LogError("WAIT!! No targets set!!");
        }
        foreach (UnitAttack unit in targetsToUseBaseItem)
        {
            UnitStats getStats = unit.GetComponent<UnitStats>();
            if (getStats == null)
            {
                Debug.LogError("ERROR: Should have UnitStats!!");
            }
            if (getStats.IsDefeated())
            {
                //Ignore enemy is already defeated.
                continue;
            }
            if (getStats.DidAttackLand(100))
            {
                unit.ActivateGettingHit();
                Debug.Log("TODO: Activate hit particles");
                getStats.TakeDamage(100, currentUnitsTurn.GetComponent<UnitStats>(), UnitStats.StatType.ATTACK_PHYSICAL);
            }
            else
            {
                Debug.LogWarning("Miss the attack!!");
            }
        }
    }

    /// <summary>
    /// Turn on the top dialogue for a moment.
    /// </summary>
    /// <param name="text"></param>
    private void TurnOnHudNarratorWithTimer(string text)
    {
        hudNarrator.SetTextWithTimer(text);
        hudNarrator.gameObject.SetActive(true);
    }

    /// <summary>
    /// Have all Heroes survive Fatal Hits.
    /// </summary>
    private void AllHeroesSurviveFatalHit()
    {
        for (int i = 0; i < heroes.Count; i++)
        {
            heroes[i].GetComponent<HeroStats>().SurviveFatalHit();
        }
    }

    /// <summary>
    /// Prepare Action Comand Manager and turn it off.
    /// </summary>
    private void PrepareActionCommandManager()
    {
        actionCommandManager.AllocateGuiComponents(this);
        actionCommandManager.ClearOut();
        actionCommandManager.gameObject.SetActive(true);
    }

    /// <summary>
    /// Clear out the text and clear out the back panel.
    /// </summary>
    private void PrepareHudNarrator()
    {
        hudNarrator.HidePanel();
        if (!hudNarrator.gameObject.activeInHierarchy)
        {
            hudNarrator.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Prepare ItemMenuManager and clear out the menu.
    /// </summary>
    private void PrepareItemMenuManager()
    {
        itemsMenuManager.ClearOutItemsMenu();
        itemsMenuManager.gameObject.SetActive(false);
    }

    /// <summary>
    /// Prepare the UnitSelectorManager and clear it out.
    /// </summary>
    private void PrepareUnitSelectorManager()
    {
        unitSelectorManager.AllocateSelectors();
        unitSelectorManager.ClearOutButtons();
    }

    /// <summary>
    /// Prepare the UnitCurrentSignal.
    /// </summary>
    private void PrepareUnitCurrentSignal()
    {
        unitCurrentSignal.SetBattleManager(this);
        unitCurrentSignal.ResetSignalToCharacter();
    }

    /// <summary>
    /// Spawn the enemies.
    /// </summary>
    private void SpawnEnemies(List<string> listOfEnemies)
    {
        enemies = new List<GameObject>(listOfEnemies.Count);
        //Enemy Unit Holder will have EnemyStats automatically
        GameObject enemy = FindAnyObjectByType<EnemyUnitHolder>(FindObjectsInactive.Include).gameObject;
        enemy.GetComponent<EnemyUnitHolder>().CreateEnemy(listOfEnemies[0]);
        enemies.Add(enemy);
        for (int i = 1; i < listOfEnemies.Count; i++)
        {
            GameObject getNext = Instantiate(enemy, enemy.transform.position, enemy.transform.rotation);
            getNext.GetComponent<EnemyUnitHolder>().CreateEnemy(listOfEnemies[i]);
            enemies.Add(getNext);
        }
    }

    /// <summary>
    /// Spawn the heroes to its region.
    /// </summary>
    /// <param name="heroes"></param>
    private void MoveHeroesToLocation(List<GameObject> heroes)
    {
        float yBasePosition = 5f;
        for (int i = 0; i < heroes.Count; i++)
        {
            yBasePosition -= 2f;
            Vector3 basePosition = new Vector3(-5f, yBasePosition);
            float xPosition = UnityEngine.Random.Range(basePosition.x - 2f, basePosition.x + 2f);
            float yPosition = UnityEngine.Random.Range(basePosition.y - 1f, basePosition.y + 1f);
            heroes[i].transform.position = new Vector3(xPosition, yPosition);
        }
    }

    /// <summary>
    /// Spawn the enemies to its region.
    /// </summary>
    /// <param name="enemies"></param>
    private void MoveEnemiesToLocation(List<GameObject> enemies)
    {
        float yBasePosition = 5f;
        for (int i = 0; i < enemies.Count; i++)
        {
            yBasePosition -= 2f;
            Vector3 basePosition = new Vector3(5f, yBasePosition);
            float xPosition = UnityEngine.Random.Range(basePosition.x - 2f, basePosition.x + 2f);
            float yPosition = UnityEngine.Random.Range(basePosition.y - 1f, basePosition.y + 1f);
            enemies[i].transform.position = new Vector3(xPosition, yPosition);
        }
    }

    /// <summary>
    /// Execute this Unit's turn. First move to location, then activate attack.
    /// </summary>
    /// <param name="itemOrAttack"></param>
    private void ExecuteTurn()
    {
        //Attack will be activated on Update
        ExecuteTurnMoveToAttackLocation();
    }

    /// <summary>
    /// Have the unit whose turn it is that they arrived to their location.
    /// </summary>
    public void ExecuteTurnUnitReportAtLocation(UnitMove unit)
    {
        if (unit.gameObject != currentUnitsTurn)
        {
            Debug.LogError("ERROR!! Unit asking to report at location is not this Unit's turn!!");
            return;
        }
        if (PlayersTurn())
        {
            currentBattleState = BattleStateForPlayer.PLAYER_ATTACK;
            ExecuteTurnPrepareAttack((Attack)getBaseItemFromUnit, targetsToUseBaseItem, true);
        }
        if (EnemysTurn())
        {
            currentBattleState = BattleStateForPlayer.ENEMY_ATTACK;
            ExecuteTurnPrepareAttack((Attack)getBaseItemFromUnit, targetsToUseBaseItem, false);
        }
    }

    /// <summary>
    /// Have unit return to location once done attacking.
    /// </summary>
    public void ExecuteTurnMoveToReturnLocation()
    {
        if (currentUnitsTurn == null)
        {
            return;
        }
        switch (getBaseItemFromUnit.GetWhereToMovePriorToUse())
        {
            case BaseItem.WhereToMovePriorToUse.STAY_IN_PLACE:
                CurrentBattleStateFinished();
                break;
            case BaseItem.WhereToMovePriorToUse.MOVE_TO_TARGET:
                Debug.Log("Move!! Get original position");
                break;
        }
    }

    /// <summary>
    /// Should the current Unit move prior to attacking?
    /// </summary>
    /// <param name="itemOrAttack"></param>
    private void ExecuteTurnMoveToAttackLocation()
    {
        if (currentUnitsTurn == null)
        {
            return;
        }
        switch (getBaseItemFromUnit.GetWhereToMovePriorToUse())
        {
            case BaseItem.WhereToMovePriorToUse.STAY_IN_PLACE:
                Debug.Log("Unit stay in place on Attack!!");
                if (PlayersTurn())
                {
                    currentBattleState = BattleStateForPlayer.PLAYER_ATTACK;
                    ExecuteTurnPrepareAttack((Attack)getBaseItemFromUnit, targetsToUseBaseItem, true);
                }
                if (EnemysTurn())
                {
                    currentBattleState = BattleStateForPlayer.ENEMY_ATTACK;
                    ExecuteTurnPrepareAttack((Attack)getBaseItemFromUnit, targetsToUseBaseItem, false);
                }
                return;
            case BaseItem.WhereToMovePriorToUse.MOVE_TO_TARGET:
                Debug.Log("Unit moves prior to Attack!!");
                // Get Final position to move to
                Bounds boundsOfTargets = new Bounds();
                foreach (UnitAttack target in targetsToUseBaseItem)
                {
                    boundsOfTargets.Encapsulate(target.transform.position);
                }
                if (PlayersTurn())
                {
                    Debug.Log("Player to move to location!!");
                    currentBattleState = BattleStateForPlayer.PLAYER_MOVING_TO_TARGET;
                    currentUnitsTurn.GetComponent<UnitMove>().MoveUnitDirectlyToLocation(boundsOfTargets.center + (Vector3.left * 2));
                }
                if (EnemysTurn())
                {
                    Debug.Log("Enemy to move to location!!");
                    currentBattleState = BattleStateForPlayer.ENEMY_MOVING_TO_TARGET;
                    currentUnitsTurn.GetComponent<UnitMove>().MoveUnitDirectlyToLocation(boundsOfTargets.center + (Vector3.right * 2));
                }
                return;
        }
    }

    /// <summary>
    /// Load up all Action Commands in the order the Attack was instantiated via Script from an Attack.
    /// Should not be directly called.
    /// </summary>
    /// <param name="attack"></param>
    private void ExecuteTurnPrepareAttack(Attack attack, UnitAttack[] targetsToUseBaseItem, bool useActionCommands = false)
    {
        if (currentUnitsTurn == null)
        {
            return;
        }
        this.targetsToUseBaseItem = targetsToUseBaseItem;
        AttackStep[] getStrikes = attack.GetStrikes();
        for (int i = 0; i < getStrikes.Length; i++)
        {
            strikesPrepared.Enqueue(getStrikes[i]);
        }
        currentUnitsTurn.GetComponent<UnitAttack>().AnimationBeginAttack();
    }

    /// <summary>
    /// Determine which menu to select.
    /// </summary>
    /// <param name="buttonPressed"></param>
    private void DetermineMenu(ActionCommand.ActionButtonPressed buttonPressed)
    {
        switch (currentMenu)
        {
            case NavigatingMenus.MAIN_MENU:
                OpenAnItemsMenu(buttonPressed);
                cameraControl.FocusCameraToHeroDuringMenus(currentUnitsTurn);
                return;
            case NavigatingMenus.ITEMS_MENU_ATTACKS:
                //TODO: Check if when selecting with same button as current menu, if pressing other buttons as "BACK" feels better to use
                if (buttonPressed == ActionCommand.ActionButtonPressed.BUTTON_EAST)
                {
                    OpenMainBattleMenu();
                    return;
                }
                if (buttonPressed == ActionCommand.ActionButtonPressed.BUTTON_SOUTH)
                {
                    //TODO: Collect Attack or Item from Items menu
                    getBaseItemFromUnit = itemsMenuManager.GetItemHighlighted();
                    TurnOnUnitSelector(getBaseItemFromUnit);
                }
                return;
            case NavigatingMenus.ITEMS_MENU_SKILLS:
                if (buttonPressed == ActionCommand.ActionButtonPressed.BUTTON_EAST)
                {
                    OpenMainBattleMenu();
                    return;
                }
                if (buttonPressed == ActionCommand.ActionButtonPressed.BUTTON_SOUTH)
                {
                    //TODO: Collect Attack or Item from Items menu
                    getBaseItemFromUnit = itemsMenuManager.GetItemHighlighted();
                    TurnOnUnitSelector(getBaseItemFromUnit);
                }
                return;
            case NavigatingMenus.ITEMS_MENU_INVENTORY:
                if (buttonPressed == ActionCommand.ActionButtonPressed.BUTTON_EAST)
                {
                    OpenMainBattleMenu();
                    return;
                }
                if (buttonPressed == ActionCommand.ActionButtonPressed.BUTTON_SOUTH)
                {
                    //TODO: Collect Attack or Item from Items menu
                    getBaseItemFromUnit = itemsMenuManager.GetItemHighlighted();
                    TurnOnUnitSelector(getBaseItemFromUnit);
                }
                return;
            case NavigatingMenus.SELECT_TARGET:
                //TODO: Check if when selecting with same button as current menu, if pressing other buttons as "BACK" feels better to use
                if (buttonPressed == ActionCommand.ActionButtonPressed.BUTTON_EAST)
                {
                    GoBackToItemsMenu();
                    return;
                }
                if (buttonPressed == ActionCommand.ActionButtonPressed.BUTTON_SOUTH)
                {
                    cameraControl.FocusCameraToCenterOfBattle();
                    itemsMenuManager.ClearOutItemsMenu();
                    targetsToUseBaseItem = SetTargetsToUseBaseItem(unitSelectorManager.GetUnitSelected(), getBaseItemFromUnit);
                    unitSelectorManager.ClearOutButtons();
                    ExecuteTurn();
                }
                return;
            default:
                return;
        }
    }

    /// <summary>
    /// Return the targets to use a base item.
    /// </summary>
    /// <param name="selectedUnit"></param>
    /// <param name="baseItem"></param>
    /// <returns></returns>
    private UnitAttack[] SetTargetsToUseBaseItem(GameObject selectedUnit, BaseItem baseItem)
    {
        UnitAttack[] groupGet = null;
        switch (baseItem.GetIntendedTarget())
        {
            case BaseItem.IntendedTarget.TEAM_ALL:
                groupGet = new UnitAttack[heroes.Count];
                for (int i = 0; i < heroes.Count; i++)
                {
                    groupGet[i] = heroes[i].GetComponent<UnitAttack>();
                }
                break;
            case BaseItem.IntendedTarget.ENEMY_ALL:
                groupGet = new UnitAttack[enemies.Count];
                for (int i = 0; i < heroes.Count; i++)
                {
                    groupGet[i] = heroes[i].GetComponent<UnitAttack>();
                }
                break;
        }
        if (groupGet == null)
        {
            groupGet = new UnitAttack[1];
            groupGet[0] = selectedUnit.GetComponent<UnitAttack>();
        }
        return groupGet;
    }


    /// <summary>
    /// Return back to a specific Items Menu if from Select Target.
    /// </summary>
    private void GoBackToItemsMenu()
    {
        if (currentMenu != NavigatingMenus.SELECT_TARGET)
        {
            return;
        }
        switch(getBaseItemFromUnit.GetClassification())
        {
            case BaseItem.ItemClassification.ATTACK:
                currentMenu = NavigatingMenus.ITEMS_MENU_ATTACKS;
                break;
            case BaseItem.ItemClassification.SKILL:
                currentMenu = NavigatingMenus.ITEMS_MENU_SKILLS;
                break;
            case BaseItem.ItemClassification.ITEM:
                currentMenu = NavigatingMenus.ITEMS_MENU_INVENTORY;
                break;
            default:
                Debug.LogError("ERROR: Invalid item type!!");
                return;
        }
        getBaseItemFromUnit = null;
        unitSelectorManager.ClearOutButtons();
        itemsMenuManager.ReOpenMenu();
    }

    /// <summary>
    /// Turn On Unit Selector and turn on the right amount of buttons.
    /// </summary>
    /// <param name="item"></param>
    private void TurnOnUnitSelector(BaseItem item)
    {
        currentMenu = NavigatingMenus.SELECT_TARGET;
        TurnOffMenuMoment();
        switch (item.GetIntendedTarget())
        {
            case BaseItem.IntendedTarget.SELF:
                unitSelectorManager.TurnOnButtons(new GameObject[] { currentUnitsTurn });
                break;
            case BaseItem.IntendedTarget.TEAM_ONE:
                unitSelectorManager.TurnOnButtons(heroes.ToArray());
                break;
            case BaseItem.IntendedTarget.TEAM_ALL:
                //unitSelectorManager.TurnOnButtons(enemies);
                break;
            case BaseItem.IntendedTarget.ENEMY_ONE:
                unitSelectorManager.TurnOnButtons(enemies.ToArray());
                break;
            case BaseItem.IntendedTarget.ENEMY_ALL:
                //unitSelectorManager.TurnOnButtons(enemies);
                break;
            case BaseItem.IntendedTarget.EVERYONE:
                List<GameObject> everyone = new List<GameObject>(heroes.Count + enemies.Count);
                foreach(GameObject unit in heroes)
                {
                    everyone.Add(unit);
                }
                foreach(GameObject unit in enemies)
                {
                    everyone.Add(unit);
                }
                unitSelectorManager.TurnOnButtons(everyone.ToArray());
                break;
        }
    }

    /// <summary>
    /// Open up an Items menu, be it Attack menu, Skills menu, or Items menu.
    /// </summary>
    /// <param name="buttonPressed"></param>
    private void OpenAnItemsMenu(ActionCommand.ActionButtonPressed buttonPressed)
    {
        //currentMenu = NavigatingMenus.ITEMS_MENU;
        switch (buttonPressed)
        {
            case ActionCommand.ActionButtonPressed.BUTTON_SOUTH:
                currentMenu = NavigatingMenus.ITEMS_MENU_ATTACKS;
                mainBattleMenu.OpenSubMenu();
                OpenAttackMenu();
                return;
            case ActionCommand.ActionButtonPressed.BUTTON_WEST:
                currentMenu = NavigatingMenus.ITEMS_MENU_SKILLS;
                mainBattleMenu.OpenSubMenu();
                OpenSkillsMenu();
                return;
            case ActionCommand.ActionButtonPressed.BUTTON_EAST:
                Debug.Log("TODO: Make Defend!!");
                return;
            case ActionCommand.ActionButtonPressed.BUTTON_NORTH:
                currentMenu = NavigatingMenus.ITEMS_MENU_INVENTORY;
                mainBattleMenu.OpenSubMenu();
                OpenInventoryMenu();
                return;
            default:
                Debug.LogError("ERROR: No other action to do!! " + buttonPressed.ToString());
                return;
        }
    }

    /// <summary>
    /// Open up the Attack Menu.
    /// </summary>
    private void OpenAttackMenu()
    {
        if (currentBattleState != BattleStateForPlayer.PLAYER_NAVIGATING_MENUS)
        {
            Debug.Log("WAIT! Not your turn.");
            return;
        }
        currentMenu = NavigatingMenus.ITEMS_MENU_ATTACKS;
        itemsMenuManager.OpenMenu("ATTACK", currentUnitsTurn.GetComponent<UnitAttack>().GetCurrentAttackList());
    }

    /// <summary>
    /// Open up the Skills Menu.
    /// </summary>
    private void OpenSkillsMenu()
    {
        if (currentBattleState != BattleStateForPlayer.PLAYER_NAVIGATING_MENUS)
        {
            Debug.Log("WAIT! Not your turn.");
            return;
        }
        currentMenu = NavigatingMenus.ITEMS_MENU_SKILLS;
        itemsMenuManager.OpenMenu("SKILLS", currentUnitsTurn.GetComponent<UnitAttack>().GetCurrentSkillsList());
    }

    /// <summary>
    /// Open up the Item Menu.
    /// </summary>
    private void OpenInventoryMenu()
    {
        if (currentBattleState != BattleStateForPlayer.PLAYER_NAVIGATING_MENUS)
        {
            Debug.Log("WAIT! Not your turn.");
            return;
        }
        currentMenu = NavigatingMenus.ITEMS_MENU_INVENTORY;
        itemsMenuManager.OpenMenu("INVENTORY", new List<Item> { /**The Empty List**/ });
    }

    /// <summary>
    /// Close the Items menu (Where Attacks, SKills, and Inventory lives).
    /// </summary>
    private void CloseItemsMenu()
    {
        if (!itemsMenuManager.gameObject.activeInHierarchy)
        {
            return;
        }
        itemsMenuManager.ClearOutItemsMenu();
        TurnOffMenuMoment();
    }

    /// <summary>
    /// Disable the menu but not lose the data stored in it.
    /// </summary>
    private void TurnOffMenuMoment()
    {
        itemsMenuManager.gameObject.SetActive(false);
    }

    /// <summary>
    /// Is it currently the Enemy's turn?
    /// </summary>
    /// <returns></returns>
    private bool EnemysTurn()
    {
        return currentUnitsTurn.GetComponent<EnemyAttack>() != null;
    }
}
