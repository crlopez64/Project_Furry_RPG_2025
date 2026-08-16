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
    private static List<string> enemyNamesToBattle;
    private List<HeroStatsStorage> heroesInTransit; //<= Can probably update to only be used during Overworld-Battle transitions

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
        Debug.Log("GameManager awake!");
        MakeInstance();
        currentGameState = GameState.OVERWORLD;
        heroesInTransit = new List<HeroStatsStorage>(4);
        enemyNamesToBattle = new List<string>(6);

        //Player and Hero Stuff
        SetPriorityInLine(heroes);
        SetHeroesToFollowLine(heroes);
        SetPlayerToMove();

        //Set Debug Stats
        foreach(GameObject hero in heroes)
        {
            if (!hero.activeInHierarchy)
            {
                continue;
            }
            hero.GetComponent<HeroStats>().SetDebugStats();
        }
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
        SceneManager.LoadScene("Test_Main");
        //TODO: Record player position
    }

    /// <summary>
    /// Update stats for active Heroes in Overworld to the battle.
    /// </summary>
    public static void PrepareHeroesToBattle()
    {
        if (instance == null)
        {
            Debug.LogError("GameManager instance is null. Cannot prepare heroes to battle.");
            return;
        }
        if (instance.heroesInTransit == null)
        {
            instance.heroesInTransit = new List<HeroStatsStorage>(4);
        }
        if (instance.heroes == null)
        {
            // Update when cleaning up proper game flow?
            Debug.LogWarning("No heroes assigned to GameManager when preparing for battle.");
            return;
        }
        foreach (GameObject hero in instance.heroes)
        {
            if (!hero.activeInHierarchy)
            {
                continue;
            }
            HeroStats heroStats = hero.GetComponent<HeroStats>();
            if (heroStats == null)
            {
                Debug.LogWarning($"Hero GameObject '{hero.name}' does not have a HeroStats component.");
                continue;
            }
            instance.heroesInTransit.Add(new HeroStatsStorage(heroStats));
        }
        Debug.Log("Heroes in Transit: " + instance.heroesInTransit.Count);
    }

    /// <summary>
    /// Add the Enemy Name to eventually index in Battle Manager when scenes transition to Battle.
    /// </summary>
    /// <param name="enemyName"></param>
    public static void PrepareEnemiesToBattle(string enemyName)
    {
        enemyNamesToBattle.Add(enemyName);
    }

    /// <summary>
    /// Return a list of Hero data.
    /// </summary>
    public static List<HeroStatsStorage> GetHeroesDataInTransit()
    {
        return instance.heroesInTransit;
    }

    /// <summary>
    /// Return the list of enemies to battle.
    /// </summary>
    /// <returns></returns>
    public static List<string> GetEnemyNamesToBattle()
    {
        return enemyNamesToBattle;
    }

    /// <summary>
    /// Set a Party Member in the GameManager's set.
    /// </summary>
    /// <param name="hero"></param>
    public void AddPartyMember(HeroStats hero)
    {   
        if (heroesInTransit == null)
        {
            heroesInTransit = new List<HeroStatsStorage>(4);
        }
        heroesInTransit.Add(new HeroStatsStorage(hero));
        
    }

    /// <summary>
    /// Remove a Party Member from the GameManager's set.
    /// </summary>
    /// <param name="hero"></param>
    public bool RemovePartyMember(string heroName)
    {
        if (heroesInTransit == null)
        {
            return false;
        }
        if (heroesInTransit.Count == 1)
        {
            return false;
        }
        foreach(HeroStatsStorage hero in heroesInTransit)
        {
            if (hero.GetUnitName().Equals(heroName))
            {
                heroesInTransit.Remove(hero);
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
    /// Set each Hero to follow the Hero in front of them in line.
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
