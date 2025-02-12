using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    [Header("Fade Settings")]
    public Image fadeImage; // Imagen del Canvas para el Fade
    public float fadeDuration = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantener el Manager en todas las escenas
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (fadeImage != null)
        {
            StartCoroutine(FadeIn());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void ChangeSceneWithFade(string sceneName)
    {
        if (fadeImage != null)
        {
            StartCoroutine(FadeOut(sceneName));
        }
        else
        {
            Debug.LogError("No se ha asignado la imagen de Fade.");
        }
    }

    private IEnumerator FadeIn()
    {
        fadeImage.gameObject.SetActive(true);
        Color color = fadeImage.color;
        color.a = 1f;

        float timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        fadeImage.gameObject.SetActive(false);
    }

    private IEnumerator FadeOut(string sceneName)
    {
        fadeImage.gameObject.SetActive(true);
        Color color = fadeImage.color;
        color.a = 0f;

        float timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
        yield return new WaitForSeconds(0.1f); // Espera a que la nueva escena cargue
        StartCoroutine(FadeIn());
    }
}
