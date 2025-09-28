using TMPro;
using UnityEngine;

/// <summary>
/// Class intended to give a value to an Item Button (e.g. Mana Cost to an Attack or Count to an Item).
/// </summary>
public class ItemButtonValue : MonoBehaviour
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
