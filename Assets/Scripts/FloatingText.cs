using UnityEngine;
using System.Collections;

public class FloatingText : MonoBehaviour
{
    public GameObject criticalHit; // Objeto para golpes críticos
    public GameObject miss; // Objeto para fallos
    public Transform target; // El GameObject sobre el que aparecerá
    public float randomRange = 1.5f; // Rango de posición aleatoria
    public float moveSpeed = 1f; // Velocidad de movimiento hacia arriba
    public float duration = 2f; // Duración antes de desactivar

    private Vector3 initialCriticalPos;
    private Vector3 initialMissPos;

    void Start()
    {
        if (criticalHit != null)
        {
            criticalHit.SetActive(false);
            initialCriticalPos = criticalHit.transform.position;
        }

        if (miss != null)
        {
            miss.SetActive(false);
            initialMissPos = miss.transform.position;
        }
    }

    public void ActivateObject(bool isCritical)
    {
        GameObject floatingObject = isCritical ? criticalHit : miss;
        if (floatingObject == null || target == null) return;

        // Generar posición aleatoria cerca del objeto
        Vector3 randomOffset = new Vector3(Random.Range(-randomRange, randomRange), 0, Random.Range(-randomRange, randomRange));
        Vector3 startPosition = target.position + randomOffset;

        // Posicionar el objeto en la nueva ubicación
        floatingObject.transform.position = startPosition;
        floatingObject.SetActive(true);

        // Iniciar animación de movimiento y desactivación
        StartCoroutine(MoveAndDisable(floatingObject, isCritical));
    }

    private IEnumerator MoveAndDisable(GameObject floatingObject, bool isCritical)
    {
        float elapsedTime = 0f;
        Vector3 startPos = floatingObject.transform.position;
        Vector3 endPos = startPos + new Vector3(0, 2f, 0); // Mover 2 unidades hacia arriba

        while (elapsedTime < duration)
        {
            floatingObject.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        floatingObject.SetActive(false);
        floatingObject.transform.position = isCritical ? initialCriticalPos : initialMissPos;
    }
}
