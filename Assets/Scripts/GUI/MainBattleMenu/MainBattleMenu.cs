using UnityEngine;

public class MainBattleMenu : MonoBehaviour
{
    private MainBattleMenuEast eastButton;
    private MainBattleMenuSouth southButton;
    private MainBattleMenuWest westButton;
    private MainBattleMenuNorth northButton;

    private void Awake()
    {
        AllocateButtons();
    }

    /// <summary>
    /// Open the main battle menu.
    /// </summary>
    public void OpenMainMenu()
    {
        eastButton.SetText("BLOCK");
        southButton.SetText("ATTACK");
        westButton.SetText("SKILLS");
        northButton.SetText("ITEMS");
    }

    /// <summary>
    /// Open Attack Menu; confirm Attack with the same button as the original button.
    /// </summary>
    public void OpenAttackMenuDoubleTap()
    {
        eastButton.SetText("CANCEL");
        southButton.SetText("CONFIRM");
        westButton.SetText("CANCEL");
        northButton.SetText("CANCEL");
    }

    /// <summary>
    /// Open Skills Menu; confirm Skill with the same button as the original button.
    /// </summary>
    public void OpenSkillsMenuDoubleTap()
    {
        eastButton.SetText("CANCEL");
        southButton.SetText("CANCEL");
        westButton.SetText("CONFIRM");
        northButton.SetText("CANCEL");
    }

    /// <summary>
    /// Open Items Menu; confirm Item with the same button as the original button.
    /// </summary>
    public void OpenItemsMenuDoubleTap()
    {
        eastButton.SetText("CANCEL");
        southButton.SetText("CANCEL");
        westButton.SetText("CANCEL");
        northButton.SetText("CONFIRM");
    }

    /// <summary>
    /// Open any sub menu with the boring options.
    /// </summary>
    public void OpenSubMenu()
    {
        eastButton.SetText("CANCEL");
        southButton.SetText("CONFIRM");
        westButton.SetText("");
        northButton.SetText("");
    }

    public void AllocateButtons()
    {
        eastButton = GetComponentInChildren<MainBattleMenuEast>();
        southButton = GetComponentInChildren<MainBattleMenuSouth>();
        westButton = GetComponentInChildren<MainBattleMenuWest>();
        northButton = GetComponentInChildren<MainBattleMenuNorth>();
    }
}
