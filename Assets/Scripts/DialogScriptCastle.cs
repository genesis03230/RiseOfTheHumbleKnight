using Doublsb.Dialog;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogScriptCastle : MonoBehaviour
{
    public static DialogScriptCastle Instance { get; private set; }

    public DialogManager dialogManager;
    public FadeController fadeController; // Referencia al FadeController

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject maidDialogue;
    [SerializeField] private GameObject printer;
    [SerializeField] private GameObject helmet;
    [SerializeField] private GameObject hair;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void StartDialogue()
    {
        //Activa el Canvas de dialogo
        printer.SetActive(true);
        player.SetActive(true);
        maidDialogue.SetActive(true);
        
        TextMaid();
    }

  

    public void EndDialogue()
    {
        //Desactiva el Canvas de dialogo
        helmet.SetActive(false);
        hair.SetActive(true);
        printer.SetActive(false);
        player.SetActive(false);

        maidDialogue.SetActive(false);

        StartCoroutine(WaitForNewAction());
    }

    private IEnumerator WaitForNewAction()
    {
        yield return new WaitForSeconds(1.5f);
        yield return StartCoroutine(fadeController.FadeOut()); // Ejecutar el fade out
        SceneManager.LoadScene("TheEnd");
    }


    private void TextMaid()
    {
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/emote:Normal//sound:Cofcof/Valiente caballero, no tengo palabras para agradecerte.", "MaidDialogue"));
        dialogTexts.Add(new DialogData("/emote:Normal/Me has salvado de un destino terrible. Pero debo regresar a la aldea.", "MaidDialogue"));
        dialogTexts.Add(new DialogData("/emote:Normal//sound:Sorry/Allí me espera el hombre al que amo.", "MaidDialogue"));

        dialogTexts.Add(new DialogData("/emote:CastleKnight//sound:Cofcof/¿Y si te dijera que tu amado nunca dejó de buscarte?", "PlayerDialogue"));
        dialogTexts.Add(new DialogData("/emote:CastleKnight/Que atravesó tormentas, montañas y sombras.", "PlayerDialogue"));
        dialogTexts.Add(new DialogData("/emote:CastleKnight/Todo para traerte de vuelta.", "PlayerDialogue"));

        dialogTexts.Add(new DialogData("/emote:Normal//speed:down//sound:Ha/. . .", "MaidDialogue"));

        dialogTexts.Add(new DialogData("/emote:Normal//speed:down//sound:Armour/. . .", "PlayerDialogue"));
        dialogTexts.Add(new DialogData("/emote:Castle//sound:Yeah/No importa cuánto haya cambiado por fuera.", "PlayerDialogue"));
        dialogTexts.Add(new DialogData("/emote:Castle/En mi corazón, sigo siendo el campesino que prometió protegerte /speed:down/SIEMPRE.", "PlayerDialogue"));

      
        dialogManager.Show(dialogTexts);

        //Llamamos a la Coroutine para esperar que el dialogo termine
        StartCoroutine(WaitForDialogueEnd());
    }

    private IEnumerator WaitForDialogueEnd()
    {
        //Mientras el dialogo este activo esperamos
        while (dialogManager.state != Doublsb.Dialog.State.Deactivate)
        {
            yield return null;
        }

        //Cuando el dialogo termine llamamos a EndDialogue()
        EndDialogue();
    }
}

