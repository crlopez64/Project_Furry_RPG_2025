using UnityEngine;
using TMPro;

/// <summary>
/// Keeps track of the "NOW" on turn taking.
/// </summary>
public class HudHeroCurrentTurn : MonoBehaviour
{
    private void Awake()
    {
        TextMeshProUGUI fountainPen = GetComponent<TextMeshProUGUI>();
        fountainPen.text = "NOW";
    }
}
