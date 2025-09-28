using UnityEngine;
using TMPro;

/// <summary>
/// Script of adding additional detail for Item.
/// </summary>
public class ItemDescriptionShortRight : MonoBehaviour
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
