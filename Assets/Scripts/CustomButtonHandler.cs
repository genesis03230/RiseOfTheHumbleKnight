using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomButtonHandler : MonoBehaviour
{
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && button != null || Input.GetKeyDown(KeyCode.JoystickButton0) && button != null)
        {
            button.onClick.Invoke();
        }
    }
}

