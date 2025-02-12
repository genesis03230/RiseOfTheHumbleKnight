using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    private bool isPlayerInRange;
    public GameObject gameObjectCanvas;
    public GameObject miniGame;

    private SimonSaysGame simonsSaysGame;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = true;
            gameObjectCanvas.SetActive(true);
            simonsSaysGame = miniGame.GetComponent<SimonSaysGame>();
            simonsSaysGame.SimonsSaysPlay();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Destroy(this);
        }
    }
}
