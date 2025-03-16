using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Doublsb.Dialog;

public class SimonSaysGame : MonoBehaviour
{
    public static PlayerController playerController;

    [SerializeField] private DialogScriptDungeon dialogScriptDungeon;
    public DialogManager dialogManager;
    public GameObject npcMagicianDialogue;
    public GameObject printer;
    public GameObject canvas;
   

    public List<GameObject> botones; // Lista de botones/runas
    public int maxAciertos = 5;
    public AudioClip[] sonidosRunas; // Clips de audio para cada runa
    public float tiempoEntreRunas = 0.5f;
    public float tiempoEntreSecuencias = 1.5f;

    private AudioSource audioSource;
    private List<int> secuencia = new List<int>();
    private int indexJugador = 0;


    private void Awake()
    {
        // Obtener el componente si no se ha asignado desde el Inspector
        if (dialogScriptDungeon == null)
        {
            dialogScriptDungeon = GetComponent<DialogScriptDungeon>();
        }

        if (dialogScriptDungeon == null)
        {
            Debug.LogError("dialogScriptDungeon no encontrado.");
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ShowTextRandomDefeat()
    {
        var dialogTexts = new List<DialogData>
        {
        new DialogData("/emote:Normal//sound:Oh/HAS FALLADO!!", "NpcMagicianDialogue"),
        new DialogData("/emote:Happy//sound:Jajaja/¡Jejeje! Si esto fuera un juego de memoria, estarías perdiendo... Ah, espera, lo es.", "NpcMagicianDialogue"),
        new DialogData("/emote:Serious//sound:Cofcof/Parece que no tienes suerte, ¿eh?", "NpcMagicianDialogue"),
        new DialogData("/emote:Surprised//sound:Hemhem/Esto es doloroso... ¿No aprenderás nunca?", "NpcMagicianDialogue")
        };

        // Selección aleatoria del diálogo
        int index = Random.Range(0, dialogTexts.Count);

        // Mostrar el diálogo aleatorio
        dialogManager.Show(new List<DialogData> { dialogTexts[index] });
    }

    public void ShowTextRandomVictory()
    {
        var dialogTexts = new List<DialogData>
        {
        new DialogData("/emote:Surprised//sound:Ouu/¡Increíble! ¡Parece que tienes más talento de lo que pensaba!", "NpcMagicianDialogue"),
        new DialogData("/emote:Happy//sound:JAJA/¡Lo has hecho! ¡Impresionante! Me has dejado sin palabras.", "NpcMagicianDialogue"),
        new DialogData("/emote:Surprised//sound:Ouu/¡Felicidades! Parece que eres un genio, o simplemente muy afortunado.", "NpcMagicianDialogue"),
        new DialogData("/emote:Happy//sound:JAJA/¡Qué bien jugado! Te felicito, has superado mi desafío. ¡Estás a otro nivel!", "NpcMagicianDialogue")
        };

        // Selección aleatoria del diálogo
        int index = Random.Range(0, dialogTexts.Count);

        // Mostrar el diálogo aleatorio
        dialogManager.Show(new List<DialogData> { dialogTexts[index] });
    }

    public void SimonsSaysPlay()
    {
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
                        printer.SetActive(true);
                        npcMagicianDialogue.SetActive(true);
                        ShowTextRandomVictory();
                        StartCoroutine(VictoryText());
                        return;
                    }

                    Debug.Log("Secuencia correcta. Nueva ronda.");
                    Invoke(nameof(GenerarNuevaSecuencia), tiempoEntreSecuencias); // Espera antes de la nueva secuencia
                }
            }
            else
            {
                Debug.Log("Secuencia incorrecta. Reiniciando...");
                printer.SetActive(true);
                npcMagicianDialogue.SetActive(true);

                ShowTextRandomDefeat();
                StartCoroutine(DefeatText());
            }
        }
    }

    private IEnumerator VictoryText()
    {
        while (dialogManager.state != State.Deactivate)
        {
            yield return null;
        }

        printer.SetActive(false);
        npcMagicianDialogue.SetActive(false);
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        dialogScriptDungeon.isFirstDialogue = false;
        dialogScriptDungeon.isSecondDialogue = true;
      
        dialogScriptDungeon.isSecondDialogue = true;
        PlayerController.isMiniGameActive = false;
        canvas.SetActive(false);
    }

    private IEnumerator DefeatText()
    {
        while (dialogManager.state != State.Deactivate)
        {
            yield return null;
        }

        printer.SetActive(false);
        npcMagicianDialogue.SetActive(false);
        ReiniciarJuego();
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
