using UnityEngine;
using TMPro;

/// <summary>
/// Script in charge of saying what an Items does.
/// </summary>
public class ItemDescriptionMain : MonoBehaviour
{
    private ItemDescriptionBody descriptionBody;
    private ItemDescriptionShortLeft descriptionShortLeft;
    private ItemDescriptionShortRight descriptionShortRight;

    private void Awake()
    {
        AllocateDescriptionParts();
    }

    /// <summary>
    /// Set the main body into the Description only. 
    /// </summary>
    /// <param name="mainBody"></param>
    public void SetText(string mainBody)
    {
        descriptionBody.SetText(mainBody);
        descriptionShortLeft.SetText("");
        descriptionShortRight.SetText("");
    }

    /// <summary>
    /// Set the main body and two smaller descriptors into the Description body.
    /// </summary>
    /// <param name="mainBody"></param>
    /// <param name="shortLeft"></param>
    /// <param name="shortRight"></param>
    public void SetText(string mainBody, string shortLeft, string shortRight)
    {
        descriptionBody.SetText(mainBody);
        descriptionShortLeft.SetText(shortLeft);
        descriptionShortRight.SetText(shortRight);
    }

    /// <summary>
    /// Clear out the Description.
    /// </summary>
    public void ClearText()
    {
        descriptionBody.SetText("");
        descriptionShortLeft.SetText("");
        descriptionShortRight.SetText("");
    }

    /// <summary>
    /// Get the components.
    /// </summary>
    public void AllocateDescriptionParts()
    {
        descriptionBody = GetComponentInChildren<ItemDescriptionBody>(true);
        descriptionShortLeft = GetComponentInChildren<ItemDescriptionShortLeft>(true);
        descriptionShortRight = GetComponentInChildren<ItemDescriptionShortRight>(true);
    }
}
