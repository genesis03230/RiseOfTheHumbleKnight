using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1.0f;

    private void Awake()
    {
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = 0;
            fadeImage.color = color;
        }
    }

    public IEnumerator FadeOut()
    {
        if (fadeImage != null)
        {
            float elapsedTime = 0f;
            Color color = fadeImage.color;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
                fadeImage.color = color;
                yield return null;
            }
        }
    }

    public IEnumerator FadeIn()
    {
        if (fadeImage != null)
        {
            float elapsedTime = 0f;
            Color color = fadeImage.color;
            color.a = 1; // Inicia en negro

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                color.a = Mathf.Clamp01(1 - (elapsedTime / fadeDuration)); // Va de negro a transparente
                fadeImage.color = color;
                yield return null;
            }
        }
    }
}
