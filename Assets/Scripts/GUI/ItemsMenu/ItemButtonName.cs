using TMPro;
using UnityEngine;

/// <summary>
/// Class intended to give the name to an Item Button.
/// </summary>
public class ItemButtonName : MonoBehaviour
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
}
