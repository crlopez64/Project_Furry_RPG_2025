using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemsDescription : MonoBehaviour
{
    private TextMeshProUGUI fountainPen;
    private Image backPanel;

    private void Awake()
    {
        fountainPen = GetComponentInChildren<TextMeshProUGUI>();
        backPanel = GetComponentInChildren<Image>();
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
