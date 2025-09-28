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
    private static List<string> enemiesToBattle;
    
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
        enemiesToBattle = new List<string>(6);

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
        enemiesToBattle.Clear();
        SceneManager.LoadScene("Test_Main");
        //TODO: Record player position
    }

    /// <summary>
    /// Add the Enemy Name to eventually index in Battle Manager when scenes transition to Battle.
    /// </summary>
    /// <param name="enemyName"></param>
    public static void AddEnemyToListToBattle(string enemyName)
    {
        enemiesToBattle.Add(enemyName);
    }

    public static List<string> GetEnemiesToBattle()
    {
        return enemiesToBattle;
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
    /// Find and return a list All Heroes in the scene.
    /// </summary>
    public List<GameObject> FindHeroes()
    {
        HeroStats[] getHeroes = FindObjectsByType<HeroStats>(FindObjectsSortMode.None);
        heroes = new List<GameObject>(getHeroes.Length);
        for(int i = 0; i < getHeroes.Length; i++)
        {
            heroes.Add(getHeroes[i].gameObject);
        }
        return heroes;
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
