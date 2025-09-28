using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Script in charge of text in the Narrator during battle.
/// </summary>
public class HudNarrator : MonoBehaviour
{
    private bool activateTimer;
    private float timer;
    private TextMeshProUGUI fountainPen;
    private Image backboard;

    private void Awake()
    {
        timer = 0f;
        fountainPen = GetComponentInChildren<TextMeshProUGUI>(true);
        backboard = GetComponentInChildren<Image>(true);
    }

    private void Update()
    {
        if (activateTimer)
        {
            if (timer > 0f)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                HidePanel();
            }
        }
    }

    /// <summary>
    /// Set the text and activate backboard. Deactivate timer.
    /// </summary>
    /// <param name="text"></param>
    public void SetText(string text)
    {
        fountainPen.text = text;
        backboard.gameObject.SetActive(true);
        activateTimer = false;
        timer = 0f;
    }

    /// <summary>
    /// Set text and activate backboard with timer.
    /// </summary>
    /// <param name="text"></param>
    public void SetTextWithTimer(string text)
    {
        fountainPen.text = text;
        backboard.gameObject.SetActive(true);
        activateTimer = true;
        timer = 3f;
    }

    /// <summary>
    /// Hide Narrator panel.
    /// </summary>
    public void HidePanel()
    {
        if (fountainPen == null)
        {
            fountainPen = GetComponentInChildren<TextMeshProUGUI>(true);
        }
        if (backboard == null)
        {
            backboard = GetComponentInChildren<Image>(true);
        }
        activateTimer = false;
        fountainPen.text = "";
        backboard.gameObject.SetActive(false);
    }
}
