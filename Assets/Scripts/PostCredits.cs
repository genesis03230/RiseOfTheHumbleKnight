using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PostCredits : MonoBehaviour
{
    public FadeController fadeController;

    [Header("Referencias")]
    public Transform cameraTransform; // Cámara principal
    public Vector3 targetPosition; // Posición final de la cámara
    public float moveDuration = 3f; // Duración del movimiento
    public Animator dragonAnimator; // Animator del DragonBoss
    public GameObject blackScreen; // Pantalla negra del Canvas
    public GameObject creditText; // Texto de créditos
    public AudioSource audioSource; // Audio de la escena

    [Header("Temporizadores")]
    public float dragonAwakeTime = 6f; // Tiempo para despertar al dragón
    public float blackScreenTime = 10f; // Tiempo para mostrar pantalla negra
    public float creditTextTime = 12f; // Tiempo para mostrar texto
    public float restartTime = 20f; // Tiempo para reiniciar el juego

    private void Start()
    {
        StartCoroutine(fadeController.FadeIn()); // Iniciar la animación de FadeIn
        // Iniciar el proceso cuando comienza la escena
        StartCoroutine(HandlePostCreditSequence());
    }

    private IEnumerator HandlePostCreditSequence()
    {
        // Mover la cámara al objetivo
        yield return StartCoroutine(MoveCamera());

        // Esperar 6 segundos y activar la animación del Dragón
        yield return new WaitForSeconds(dragonAwakeTime);
        if (dragonAnimator != null)
        {
            dragonAnimator.SetBool("awakeDragon", true);
        }

        // Esperar más tiempo y activar la pantalla negra
        yield return new WaitForSeconds(blackScreenTime - dragonAwakeTime);
        if (blackScreen != null)
        {
            blackScreen.SetActive(true);
        }

        // Esperar más tiempo y activar el texto
        yield return new WaitForSeconds(creditTextTime - blackScreenTime);
        if (creditText != null)
        {
            creditText.SetActive(true);
        }

        // Esperar más tiempo y reiniciar la escena o cargar la primera
        yield return new WaitForSeconds(restartTime - creditTextTime);
        SceneManager.LoadScene(0); // Cargar la primera escena (ajústalo según el nombre de tu escena inicial)
    }

    private IEnumerator MoveCamera()
    {
        Vector3 startPosition = cameraTransform.position;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            cameraTransform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegurar que la cámara se coloque exactamente en la posición final
        cameraTransform.position = targetPosition;
    }
}
