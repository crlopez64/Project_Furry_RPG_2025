using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class in charge of keeping text and button to West Button in the Main battle Menu.
/// </summary>
public class MainBattleMenuWest : MonoBehaviour
{
    private TextMeshProUGUI fountainPen;
    //private Image image; 

    private void Awake()
    {
        fountainPen = GetComponentInChildren<TextMeshProUGUI>();
        //image = GetComponentInChildren<Image>();
    }

    /// <summary>
    /// Set text.
    /// </summary>
    /// <param name="text"></param>
    public void SetText(string text)
    {
        fountainPen.text = text;
    }
}
