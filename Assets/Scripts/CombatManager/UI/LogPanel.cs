using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogPanel : MonoBehaviour
{
    /*protected static LogPanel current;

    public Text logLabel;

    public Text logText;
    private Queue<string> messages = new Queue<string>();
    private bool isDisplayingMessage = false;

    void Awake()
    {
        current = this;
    }

    public static void Write(string message)
    {
        current.logLabel.text = message;
    }*/

    protected static LogPanel current;

    public Text logLabel;
    public Text logText;
    private Queue<(string, float)> messages = new Queue<(string, float)>();
    private bool isDisplayingMessage = false;
    private float defaultDuration = 2f; // Duración predeterminada

    void Awake()
    {
        current = this;
    }

    public static void Write(string message, float duration = -1f)
    {
        if (duration < 0)
        {
            duration = current.defaultDuration;
        }
        current.messages.Enqueue((message, duration));
        if (!current.isDisplayingMessage)
        {
            current.StartCoroutine(current.DisplayMessages());
        }
    }

    private IEnumerator DisplayMessages()
    {
        isDisplayingMessage = true;
        while (messages.Count > 0)
        {
            var (message, duration) = messages.Dequeue();
            logLabel.text = message;
            yield return new WaitForSeconds(duration);
        }
        isDisplayingMessage = false;
    }
}
