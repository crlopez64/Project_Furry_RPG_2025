using UnityEngine;

/// <summary>
/// Manager in charge of all 4 Hud Hero Masters on screen.
/// </summary>
public class HudHeroManager : MonoBehaviour
{
    private HudHeroMaster[] masters;

    private void Awake()
    {
        masters = GetComponentsInChildren<HudHeroMaster>();
        FindFirstObjectByType<GameManager>().SetHudHeroManager(this);
        AllocateGuiComponents();
    }

    /// <summary>
    /// Assign a Hud to a specific hero. Index 0 = left-most Hud.
    /// </summary>
    /// <param name="hero"></param>
    /// <param name="index"></param>
    public void AssignHudToHero(HeroStats hero, int index)
    {
        if (index < 0 || index >= masters.Length)
        {
            return;
        }
        hero.SetHudHeroMaster(masters[index]);
        masters[index].gameObject.SetActive(true);
    }

    /// <summary>
    /// Turn off a specific Hud Hero master.
    /// </summary>
    /// <param name="index"></param>
    public void TurnOffHud(int index)
    {
        masters[index].gameObject.SetActive(false);
    }

    /// <summary>
    /// Turn off all the hud
    /// </summary>
    public void TurnOffHud()
    {
        foreach(HudHeroMaster master in masters)
        {
            master.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Turn on all the Hud.
    /// </summary>
    public void TurnOnHud()
    {
        foreach (HudHeroMaster master in masters)
        {
            master.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Allocate the GUI components for all the Masters.
    /// </summary>
    private void AllocateGuiComponents()
    {
        if (masters == null)
        {
            masters = GetComponentsInChildren<HudHeroMaster>();
        }
        foreach (HudHeroMaster master in masters)
        {
            master.AllocateGuiCompoenents();
        }
    }

    /// <summary>
    /// Return the HUD Count.
    /// </summary>
    /// <returns></returns>
    public int GetHudCount()
    {
        return masters.Length;
    }
}
