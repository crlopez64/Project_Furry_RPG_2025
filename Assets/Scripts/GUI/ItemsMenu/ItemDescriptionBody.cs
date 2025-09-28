using UnityEngine;
using TMPro;

/// <summary>
/// Script in charge of saying what an Items does.
/// </summary>
public class ItemDescriptionBody : MonoBehaviour
{
    private TextMeshProUGUI fountainPen;

    private void Awake()
    {
        fountainPen = GetComponentInChildren<TextMeshProUGUI>();
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
