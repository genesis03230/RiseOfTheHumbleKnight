using System.Collections;
using UnityEngine;

public class SlowMotionController : MonoBehaviour
{
    public float slowMotionScale = 0.2f; // Velocidad reducida (20% del tiempo normal)
    public float slowMotionDuration = 2f; // Duración en segundos
    public float restoreDuration = 1f; // Tiempo para restaurar a la normalidad

    private bool isSlowMotionActive = false;

    public void ActivateSlowMotion()
    {
        if (!isSlowMotionActive)
        {
            StartCoroutine(SlowMotionEffect());
        }
    }

    private IEnumerator SlowMotionEffect()
    {
        isSlowMotionActive = true;

        // Ralentizar el tiempo
        Time.timeScale = slowMotionScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // Ajustar físicas

        // Esperar el tiempo de slow motion
        yield return new WaitForSecondsRealtime(slowMotionDuration);

        // Restaurar el tiempo gradualmente
        float elapsedTime = 0f;
        while (elapsedTime < restoreDuration)
        {
            Time.timeScale = Mathf.Lerp(slowMotionScale, 1f, elapsedTime / restoreDuration);
            Time.fixedDeltaTime = 0.02f * Time.timeScale; // Ajustar físicas
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        // Asegurar que el tiempo vuelve a la normalidad
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        isSlowMotionActive = false;
    }
}

