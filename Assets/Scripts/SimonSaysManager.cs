using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimonSaysGame : MonoBehaviour
{
    public List<GameObject> botones; // Lista de botones/runas
    public int maxAciertos = 5;
    public AudioClip[] sonidosRunas; // Clips de audio para cada runa
    public float tiempoEntreRunas = 0.5f;
    public float tiempoEntreSecuencias = 1.5f;

    private AudioSource audioSource;
    private List<int> secuencia = new List<int>();
    private int indexJugador = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        GenerarNuevaSecuencia();
    }

    public void BotonPresionado(int index)
    {
        if (indexJugador < secuencia.Count)
        {
            ReproducirSonido(index);

            if (secuencia[indexJugador] == index)
            {
                indexJugador++;
                if (indexJugador == secuencia.Count)
                {
                    if (secuencia.Count >= maxAciertos)
                    {
                        Debug.Log("¡Ganaste el juego!");
                        return;
                    }

                    Debug.Log("Secuencia correcta. Nueva ronda.");
                    Invoke(nameof(GenerarNuevaSecuencia), tiempoEntreSecuencias); // Espera antes de la nueva secuencia
                }
            }
            else
            {
                Debug.Log("Secuencia incorrecta. Reiniciando...");
                ReiniciarJuego();
            }
        }
    }

    void GenerarNuevaSecuencia()
    {
        indexJugador = 0;
        int nuevoIndex = Random.Range(0, botones.Count);
        secuencia.Add(nuevoIndex);

        StartCoroutine(MostrarSecuencia());
    }

    System.Collections.IEnumerator MostrarSecuencia()
    {
        foreach (int index in secuencia)
        {
            var buttonImage = botones[index].GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.color = Color.yellow; // Cambia el color del botón
            }

            ReproducirSonido(index);
            yield return new WaitForSeconds(tiempoEntreRunas);

            if (buttonImage != null)
            {
                buttonImage.color = Color.white; // Restaura el color del botón
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    void ReproducirSonido(int index)
    {
        if (sonidosRunas.Length > index)
        {
            audioSource.PlayOneShot(sonidosRunas[index]);
        }
    }

    void ReiniciarJuego()
    {
        secuencia.Clear();
        Invoke(nameof(GenerarNuevaSecuencia), tiempoEntreSecuencias);
    }
}
