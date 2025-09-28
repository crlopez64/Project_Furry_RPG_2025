using UnityEngine;
using TMPro;

/// <summary>
/// The GUI for running basic text.
/// </summary>
public class HudHeroTextBase : MonoBehaviour
{
    protected TextMeshProUGUI fountainPen;

    private void Awake()
    {
        fountainPen = GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// Set the text to this gameObject.
    /// </summary>
    /// <param name="text"></param>
    public void SetText(string text)
    {
        fountainPen.text = text;
    }

}
