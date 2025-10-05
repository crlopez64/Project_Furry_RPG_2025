using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The singleton Game Manager.
/// </summary>
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private static List<string> enemyScriptCallsToBattle;
    private static List<string> enemyNamesToBattle;
    private List<HeroStatsStorage> heroesInParty;

    private ItemsMenuManager itemMenuManager;
    private HudHeroManager hudHeroManager;
    private BattleManager battleManager;
    private GameState currentGameState;

    public List<GameObject> heroes;

    /// <summary>
    /// The overall game state of the program.
    /// </summary>
    public enum GameState
    {
        /// <summary>
        /// Debug State.
        /// </summary>
        NONE,
        /// <summary>
        /// Searching menus like the Main Menu, Options menu, etc.
        /// </summary>
        SEARCHING_MENUS,
        /// <summary>
        /// Player in the overworld.
        /// </summary>
        OVERWORLD,
        /// <summary>
        /// Player in the middle of battle.
        /// </summary>
        BATTLE
    }

    private void Awake()
    {
        MakeInstance();
        currentGameState = GameState.OVERWORLD;
        heroesInParty = new List<HeroStatsStorage>(4);
        enemyNamesToBattle = new List<string>(6);
        enemyScriptCallsToBattle = new List<string>(6);

        //Player and Hero Stuff
        SetPriorityInLine(heroes);
        SetHeroesToFollowLine(heroes);
        SetFollowPlayer();
        SetPlayerToMove();
    }
    private void Start()
    {
        if (!battleManager)
        {
            hudHeroManager.TurnOffHud();
        }
        if (hudHeroManager != null)
        {
            AssignHudToHero();
        }
        foreach (GameObject hero in heroes)
        {
            if (hero.activeInHierarchy)
            {
                AddPartyMember(hero.GetComponent<HeroStats>());
            }
        }
    }

    /// <summary>
    /// Move scene from Overworld to Battle.
    /// </summary>
    public static void MoveToBattleScene()
    {
        Debug.Log("Move to battle");
        SceneManager.LoadScene("Test_Battle");
        //TODO: Record player position
    }

    public static void MoveToOverworld()
    {
        Debug.Log("Move to Overworld");
        enemyNamesToBattle.Clear();
        enemyScriptCallsToBattle.Clear();
        SceneManager.LoadScene("Test_Main");
        //TODO: Record player position
    }

    /// <summary>
    /// Add the Enemy Name to eventually index in Battle Manager when scenes transition to Battle.
    /// </summary>
    /// <param name="enemyName"></param>
    public static void AddEnemyToListToBattle(string enemyName, string enemyScriptCall)
    {
        enemyNamesToBattle.Add(enemyName);
        enemyScriptCallsToBattle.Add(enemyScriptCall);
    }

    /// <summary>
    /// Return the list of front-end names to show on screen.
    /// </summary>
    /// <returns></returns>
    public static List<string> GetEnemyNamesToBattle()
    {
        return enemyNamesToBattle;
    }

    /// <summary>
    /// Return the list of enemy script calls to battle to allocate their respective EnemyAttack components.
    /// </summary>
    /// <returns></returns>
    public static List<string> GetEnemyScriptCallsToBattle()
    {
        return enemyScriptCallsToBattle;
    }

    /// <summary>
    /// Set a Party Member in the GameManager's set.
    /// </summary>
    /// <param name="hero"></param>
    public void AddPartyMember(HeroStats hero)
    {
        if (heroesInParty == null)
        {
            heroesInParty = new List<HeroStatsStorage>(4);
        }
        heroesInParty.Add(new HeroStatsStorage(hero));
    }

    /// <summary>
    /// Remove a Party Member from the GameManager's set.
    /// </summary>
    /// <param name="hero"></param>
    public bool RemovePartyMember(string heroName)
    {
        if (heroesInParty == null)
        {
            return false;
        }
        if (heroesInParty.Count == 1)
        {
            return false;
        }
        foreach(HeroStatsStorage hero in heroesInParty)
        {
            if (hero.GetUnitName().Equals(heroName))
            {
                heroesInParty.Remove(hero);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Set the Battle Manager and set Heroes (and maybe Enemies) to Battle Manager.
    /// </summary>
    /// <param name="battleManager"></param>
    public void SetBattleManager(BattleManager battleManager)
    {
        this.battleManager = battleManager;
    }

    /// <summary>
    /// Set the Hud Hero Manager.
    /// </summary>
    /// <param name="hudHeroManager"></param>
    public void SetHudHeroManager(HudHeroManager hudHeroManager)
    {
        this.hudHeroManager = hudHeroManager;
    }

    /// <summary>
    /// Set the Items Manager.
    /// </summary>
    /// <param name="itemMenuManager"></param>
    public void SetItemsManager(ItemsMenuManager itemMenuManager)
    {
        this.itemMenuManager = itemMenuManager;
        this.itemMenuManager.ClearOutItemsMenu();
        this.itemMenuManager.gameObject.SetActive(false);
    }

    /// <summary>
    /// Set the Priority order in line.
    /// </summary>
    /// <param name="heroes"></param>
    public void SetPriorityInLine(List<GameObject> heroes)
    {
        for(int i = 0; i <  heroes.Count; i++)
        {
            heroes[i].GetComponent<HeroMove>().SetOrderInLine((byte)(i + 1));
        }
    }

    /// <summary>
    /// Set who this Unit should follow.
    /// </summary>
    /// <param name="heroes"></param>
    public void SetHeroesToFollowLine(List<GameObject> heroes)
    {
        if (heroes == null)
        {
            Debug.LogError("ERROR: Heroes array is not set!!");
        }
        List<HeroMove> getList = new List<HeroMove>();
        for (int i = 0; i < heroes.Count; i++)
        {
            getList.Add(heroes[i].GetComponent<HeroMove>());
        }
        getList = getList.OrderBy(unit => unit.GetOrderInLine()).ToList();
        HeroMove getHero = null;
        for(int i = 0; i < getList.Count; i++)
        {
            if (getHero == null)
            {
                getHero = getList[i];
                continue;
            }
            getList[i].SetToFollowInLine(getHero);
            getHero = getList[i];
        }
    }

    /// <summary>
    /// Set that the player is able to move.
    /// </summary>
    public void SetPlayerToMove()
    {
        for (int i = 0; i < heroes.Count; i++)
        {
            PlayerMove getPlayerMove = heroes[i].GetComponent<PlayerMove>();
            if (getPlayerMove != null)
            {
                getPlayerMove.StartPlayerMove();
                return;
            }
        }
    }

    /// <summary>
    /// Set who this Unit should be following.
    /// </summary>
    public void SetFollowPlayer()
    {
        for (int i = 1; i < heroes.Count; i++)
        {
            HeroMove getPlayerMove = heroes[i].GetComponent<HeroMove>();
            if (getPlayerMove != null)
            {
                getPlayerMove.SetToFollowInLine(heroes[i-1].GetComponent<HeroMove>());
            }
        }
    }

    /// <summary>
    /// Assign HUD's to heroes.
    /// </summary>
    public void AssignHudToHero()
    {
        AssignHudToHero(heroes);
    }

    /// <summary>
    /// Assign Hud to a specified group of heroes.
    /// </summary>
    /// <param name="heroes"></param>
    public void AssignHudToHero(List<GameObject> heroes)
    {
        if (hudHeroManager == null)
        {
            Debug.LogError("Could not find HudHeroManager");
        }
        for (int i = 0; i < hudHeroManager.GetHudCount(); i++)
        {
            if (i < heroes.Count)
            {
                hudHeroManager.AssignHudToHero(heroes[i].GetComponent<HeroStats>(), i);
                continue;
            }
            hudHeroManager.TurnOffHud(i);
        }
    }

    /// <summary>
    /// Return a list of Hero data.
    /// </summary>
    public List<HeroStatsStorage> GetHeroesData()
    {
        return heroesInParty;
    }

    /// <summary>
    /// Make the singleton instance. Destroy copy if instance already exists.
    /// </summary>
    private void MakeInstance()
    {
        if ((instance != null) && (instance != this))
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

}
