using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class CanvasController : MonoBehaviour
{
    [Header("Background Settings")]
    public float moveSpeedX = 0.5f; // Velocidad de movimiento en X (ajustable desde el Inspector)
    public RectTransform background; // RectTransform de la imagen de fondo

    [Header("Image Settings")]
    public GameObject[] images; // Imágenes a mostrar
    public TextMeshProUGUI[] texts; // Textos de las imágenes
    public float fadeDuration = 1f; // Duración del fade in y fade out
    public string nextSceneName; // Nombre de la siguiente escena

    private int currentImageIndex = 0;
    public float waitAfterText = 2f; // Tiempo de espera después de mostrar cada texto

    void Start()
    {
        if (background == null)
        {
            background = GameObject.Find("Background").GetComponent<RectTransform>(); // Buscar la imagen si no está asignada
        }
        StartCoroutine(ShowImages());
    }

    void Update()
    {
        // Movimiento en X del fondo
        if (background != null)
        {
            // Movemos la imagen cambiando su posición en el eje X
            Vector2 currentPos = background.anchoredPosition;
            background.anchoredPosition = new Vector2(currentPos.x - moveSpeedX * Time.deltaTime, currentPos.y); // Modificamos el valor de X
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

        // Después de mostrar todas las imágenes, espera un tiempo y carga la siguiente escena
        yield return new WaitForSeconds(waitAfterText); // Esperar después de la última imagen
        SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator TypewriterEffect(TextMeshProUGUI text, string fullText, float delayBetweenLetters)
    {
        // Asegurarse de que el texto esté invisible al principio
        CanvasGroup textCanvasGroup = text.GetComponent<CanvasGroup>();
        if (textCanvasGroup == null) textCanvasGroup = text.AddComponent<CanvasGroup>();
        textCanvasGroup.alpha = 0; // El texto empieza invisible

        // Comenzar el efecto Typewriter, pero solo cuando el fade-in de la imagen haya terminado
        string fullTextContent = text.text; // Guardamos el texto completo
        text.text = ""; // Limpiamos el texto inicial

        // Hacer que el texto sea visible antes de comenzar el efecto Typewriter
        textCanvasGroup.alpha = 1;

        foreach (char letter in fullText)
        {
            text.text += letter; // Agregar una letra al texto visible
            yield return new WaitForSeconds(delayBetweenLetters); // Esperar un tiempo entre cada letra
        }
    }

    private IEnumerator FadeInImage(GameObject image, TextMeshProUGUI text)
    {
        // Asegurarnos de que la imagen y el texto están invisibles al principio.
        CanvasGroup imageCanvasGroup = image.GetComponent<CanvasGroup>();
        if (imageCanvasGroup == null) imageCanvasGroup = image.AddComponent<CanvasGroup>();
        imageCanvasGroup.alpha = 0;

        CanvasGroup textCanvasGroup = text.GetComponent<CanvasGroup>();
        if (textCanvasGroup == null) textCanvasGroup = text.AddComponent<CanvasGroup>();
        textCanvasGroup.alpha = 0; // Asegurarnos de que el texto empieza invisible

        // Fade in de la imagen
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            imageCanvasGroup.alpha = alpha;
            yield return null;
        }

        // El texto permanece invisible hasta que comience el efecto Typewriter
    }

    private IEnumerator FadeOutImage(GameObject image, TextMeshProUGUI text)
    {
        // Fade out de la imagen
        CanvasGroup imageCanvasGroup = image.GetComponent<CanvasGroup>();
        if (imageCanvasGroup == null) imageCanvasGroup = image.AddComponent<CanvasGroup>();
        imageCanvasGroup.alpha = 1;

        // Fade out del texto
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

        image.SetActive(false); // Desactiva la imagen después del fade out
        text.gameObject.SetActive(false); // Desactiva el texto después del fade out
    }
}
