using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextFitter : MonoBehaviour
{
    [SerializeField] GameObject messageObject;
    [SerializeField] Image backgrounImage;
    [SerializeField] TextMeshProUGUI text;

    [SerializeField] RectTransform rectTransform;
    [SerializeField] RectTransform textRectTransform;
    float preferredHeight;

    void SetHeight()
    {
        if (text == null) { return; }

        preferredHeight = text.preferredHeight;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, preferredHeight);
    }

    private void OnEnable()
    {
        SetHeight();
    }

    private void Start()
    {
        SetHeight();
    }

    public void DisplayMessage(string message, bool isGood, float displayTime)
    {
        // Turn on the message object
        messageObject.SetActive(true);

        // Set the background image color
        if (isGood) { backgrounImage.color = Color.green; }
        else { backgrounImage.color = Color.red; }

        // Set the message
        text.text = message;

        // Set the height
        SetHeight();

        StartCoroutine(WaitEndClose(displayTime));
    }

    private IEnumerator WaitEndClose(float displayTime)
    {
        yield return new WaitForSeconds(displayTime);

        // Delete the text
        text.text = "";

        // Turn off the message object
        messageObject.SetActive(false);
    }
}
