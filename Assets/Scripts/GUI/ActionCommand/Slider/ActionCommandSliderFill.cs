using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Change the color of the slider fill.
/// </summary>
public class ActionCommandSliderFill : MonoBehaviour
{
    private Image image;

    private void Awake()
    {
        AllocateGuiComponents();
    }

    /// <summary>
    /// Set the color fill.
    /// </summary>
    /// <param name="color"></param>
    public void SetColor(Color color)
    {
        if (image == null)
        {
            AllocateGuiComponents();
        }
        image.color = color;
    }

    private void AllocateGuiComponents()
    {
        image = GetComponent<Image>();
    }
}
