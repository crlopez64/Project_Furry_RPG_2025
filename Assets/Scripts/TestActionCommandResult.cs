using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestActionCommandResult : MonoBehaviour
{
    private float timer;
    private TextMeshProUGUI fountainPen;
    private Vector3 position;

    private void Awake()
    {
        fountainPen = GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
        timer = 2f;
    }

    void Update()
    {
        if (timer > 0.0f)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (timer > 0.0f)
        {
            // Slowly scroll up
            position = GetComponent<RectTransform>().position;
            Vector3 newPosition = new Vector3(position.x, position.y + 0.6f, position.z);
            GetComponent<RectTransform>().position = newPosition;
        }
    }

    private void OnEnable()
    {
        timer = 2f;
    }

    /// <summary>
    /// Set text of success.
    /// </summary>
    /// <param name="text"></param>
    public void SetText(string text)
    {
        if (fountainPen.text == null)
        {
            fountainPen = GetComponent<TextMeshProUGUI>();
        }
        fountainPen.text = text;
        timer = 2f;
    }

    /// <summary>
    /// Clear the text.
    /// </summary>
    public void ClearText()
    {
        if (fountainPen.text == null)
        {
            fountainPen = GetComponent<TextMeshProUGUI>();
        }
        fountainPen.text = "";
        timer = 0f;
        gameObject.SetActive(false);
    }
}
