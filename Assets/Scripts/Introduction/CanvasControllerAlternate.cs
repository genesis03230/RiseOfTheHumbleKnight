using System.Collections;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CanvasControllerAlternate : MonoBehaviour
{
    [Header("Background Settings")]
    public float moveSpeedX = 0.5f; // Velocidad de movimiento en X (ajustable desde el Inspector)
    public RectTransform background; // RectTransform de la imagen de fondo

    [Header("Image Settings")]
    public GameObject[] images; // Imágenes a mostrar
    public TextMeshProUGUI[] texts; // Textos de las imágenes
    public float fadeDuration = 1f; // Duración del fade in y fade out

    [Header("Final Text")]
    public TextMeshProUGUI finalText; // Texto final a mostrar después del fade out

    [Header("Black Screen Panel")]
    public GameObject blackScreenPanel; // Panel negro para el fade out completo

    private int currentImageIndex = 0;
    public float waitAfterText = 2f; // Tiempo de espera después de mostrar cada texto

    public VideoPlayer videoPlayer;
    public GameObject videoCanvas;

    void Start()
    {
        videoCanvas.SetActive(false);

        if (background == null)
        {
            background = GameObject.Find("Background").GetComponent<RectTransform>(); // Buscar la imagen si no está asignada
        }
        if (blackScreenPanel == null)
        {
            blackScreenPanel = GameObject.Find("BlackScreen"); // Buscar el panel negro si no está asignado
        }
        StartCoroutine(ShowImages());
    }

    void Update()
    {
        // Movimiento en X del fondo
        if (background != null)
        {
            Vector2 currentPos = background.anchoredPosition;
            background.anchoredPosition = new Vector2(currentPos.x - moveSpeedX * Time.deltaTime, currentPos.y);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private IEnumerator ShowImages()
    {
        // Mostrar imágenes y textos en orden
        for (int i = 0; i < images.Length; i++)
        {
            images[i].SetActive(true);
            yield return StartCoroutine(FadeInImage(images[i], texts[i])); // Fade in imagen
            yield return StartCoroutine(TypewriterEffect(texts[i], texts[i].text, 0.05f)); // Efecto Typewriter
            yield return new WaitForSeconds(waitAfterText); // Esperar después de que el texto ha terminado de escribirse
            yield return StartCoroutine(FadeOutImage(images[i], texts[i])); // Fade out imagen y texto
        }

        // Una vez que terminan todas las imágenes, procedemos a hacer fade out al Canvas completo
        yield return StartCoroutine(FadeToBlack()); // Oscurecer la pantalla

        // Logica para después de la pantalla negra
        PlayCredits();
    }

    private IEnumerator TypewriterEffect(TextMeshProUGUI text, string fullText, float delayBetweenLetters)
    {
        CanvasGroup textCanvasGroup = text.GetComponent<CanvasGroup>();
        if (textCanvasGroup == null) textCanvasGroup = text.AddComponent<CanvasGroup>();
        textCanvasGroup.alpha = 0;

        string fullTextContent = text.text;
        text.text = "";

        textCanvasGroup.alpha = 1;

        foreach (char letter in fullText)
        {
            text.text += letter;
            yield return new WaitForSeconds(delayBetweenLetters);
        }
    }

    private IEnumerator FadeInImage(GameObject image, TextMeshProUGUI text)
    {
        CanvasGroup imageCanvasGroup = image.GetComponent<CanvasGroup>();
        if (imageCanvasGroup == null) imageCanvasGroup = image.AddComponent<CanvasGroup>();
        imageCanvasGroup.alpha = 0;

        CanvasGroup textCanvasGroup = text.GetComponent<CanvasGroup>();
        if (textCanvasGroup == null) textCanvasGroup = text.AddComponent<CanvasGroup>();
        textCanvasGroup.alpha = 0;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            imageCanvasGroup.alpha = alpha;
            yield return null;
        }
    }

    private IEnumerator FadeOutImage(GameObject image, TextMeshProUGUI text)
    {
        CanvasGroup imageCanvasGroup = image.GetComponent<CanvasGroup>();
        if (imageCanvasGroup == null) imageCanvasGroup = image.AddComponent<CanvasGroup>();
        imageCanvasGroup.alpha = 1;

        CanvasGroup textCanvasGroup = text.GetComponent<CanvasGroup>();
        if (textCanvasGroup == null) textCanvasGroup = text.AddComponent<CanvasGroup>();
        textCanvasGroup.alpha = 1;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));
            imageCanvasGroup.alpha = alpha;
            textCanvasGroup.alpha = alpha;
            yield return null;
        }

        image.SetActive(false); // Desactivar la imagen
        text.gameObject.SetActive(false); // Desactivar el texto
    }

    private IEnumerator FadeToBlack()
    {
        // Asegurarse de que el panel negro sea invisible inicialmente
        CanvasGroup blackScreenCanvasGroup = blackScreenPanel.GetComponent<CanvasGroup>();
        if (blackScreenCanvasGroup == null) blackScreenCanvasGroup = blackScreenPanel.AddComponent<CanvasGroup>();
        blackScreenCanvasGroup.alpha = 0;

        // Asegurarse de que el panel negro sea visible al final
        blackScreenPanel.SetActive(true);

        // Fade to black (oscurecer la pantalla)
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            blackScreenCanvasGroup.alpha = alpha;
            yield return null;
        }
    }

    private IEnumerator ShowFinalText(string text)
    {
        // Asignamos el texto final
        finalText.text = text;

        // Asegurarnos de que el texto esté invisible al principio
        CanvasGroup textCanvasGroup = finalText.GetComponent<CanvasGroup>();
        if (textCanvasGroup == null) textCanvasGroup = finalText.AddComponent<CanvasGroup>();
        textCanvasGroup.alpha = 0;

        // Hacemos fade in del texto
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            textCanvasGroup.alpha = alpha;
            yield return null;
        }

        // Activar el texto final una vez se complete el fade in
        finalText.gameObject.SetActive(true);
    }

    public void PlayCredits()
    {
        videoCanvas.SetActive(true); // Mostrar el Canvas
        videoPlayer.Play(); // Reproducir el video

        // Suscribirse al evento cuando el video termine
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene("PostCredits");
    }
}
