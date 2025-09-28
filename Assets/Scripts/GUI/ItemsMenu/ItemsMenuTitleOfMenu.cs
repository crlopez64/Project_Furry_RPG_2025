using TMPro;
using UnityEngine;

/// <summary>
/// Script in charge of updating the Title text of the Items Menu.
/// </summary>
public class ItemsMenuTitleOfMenu : MonoBehaviour
{
    private TextMeshProUGUI fountainPen;

    private void Awake()
    {
        fountainPen = GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// Set the text. 
    /// </summary>
    /// <param name="text"></param>
    public void SetText(string text)
    {
        fountainPen.text = text;
    }

    /// <summary>
    /// Clear out the text.
    /// </summary>
    public void ClearText()
    {
        fountainPen.text = "";
    }
}
