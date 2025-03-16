using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSkipper : MonoBehaviour
{
    [SerializeField] private GameObject imageToShow; // Imagen en el Canvas (asignar en el Inspector)
    [SerializeField] private float delayTime = 5f; // Tiempo antes de mostrar la imagen
    [SerializeField] private string skipButton = "b"; // Tecla para omitir la escena
    [SerializeField] private string gamepadButton = "joystick button 1"; // Botón del Gamepad (B en Xbox)

    private bool canSkip = false;

    private void Start()
    {
        if (imageToShow != null)
            imageToShow.SetActive(false); // Oculta la imagen al inicio

        Invoke(nameof(ShowImage), delayTime); // Espera el tiempo y muestra la imagen
    }

    private void Update()
    {
        if (canSkip && (Input.GetKeyDown(skipButton) || Input.GetKeyDown(gamepadButton)))
        {
            SkipScene();
        }
    }

    private void ShowImage()
    {
        if (imageToShow != null)
            imageToShow.SetActive(true);

        canSkip = true; // Ahora la escena se puede omitir
    }

    private void SkipScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex); // Carga la siguiente escena
        }
        else
        {
            Debug.Log("No hay más escenas en el Build Settings.");
        }
    }
}
