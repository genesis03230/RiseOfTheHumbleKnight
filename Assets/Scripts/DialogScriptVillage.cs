using Doublsb.Dialog;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogScriptVillage : MonoBehaviour
{
    public static DialogScriptVillage Instance { get; private set; }

    public DialogManager dialogManager;

    public PatrolCharacter patrolCharacter;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject npcBoyDialogue;
    [SerializeField] private GameObject npcWomenDialogue;
    [SerializeField] private GameObject printer;
    [SerializeField] private GameObject trigger;
    [SerializeField] private GameObject door_Close;
    [SerializeField] private GameObject door_Open;
    private bool isDialogueActive;

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

    public void StartDialogue(string npcName)
    {
        if (isDialogueActive) return;

        isDialogueActive = true;

        //Activa el Canvas de dialogo
        printer.SetActive(true);
        player.SetActive(true);
        

        switch (npcName)
        {
            case "NpcBoy":
                npcBoyDialogue.SetActive(true);
                patrolCharacter.PausePatrol();
                TextNpcBoy();
                break;

            case "NpcWomen":
                trigger.SetActive(true);
                door_Close.SetActive(false);
                door_Open.SetActive(true);
                npcWomenDialogue.SetActive(true);
                TextNpcWomen();
                break;

            // Agregar mas dialogos de NPC
            // case "NpcWoman":
            //     TextNpcWoman();
            //     break;

            default:
                Debug.LogWarning("NPC no reconocido: " + npcName);
                EndDialogue();
                break;
        }
    }

  

    public void EndDialogue()
    {
        //Desactiva el Canvas de dialogo
        printer.SetActive(false);
        player.SetActive(false);

        npcBoyDialogue.SetActive(false);
        npcWomenDialogue.SetActive(false);

        isDialogueActive = false;

        StartCoroutine(WaitForNewAction());
    }

    private IEnumerator WaitForNewAction()
    {
        yield return new WaitForSeconds(0.2f); //Esperar para que el Raycast no detecte enseguida el dialogo nuevamente

        patrolCharacter.ResumePatrol();

        //Notifica al PlayerController que ya puede interactuar de nuevo
        FindObjectOfType<PlayerController>().EndDialogue();
    }

    private void TextNpcBoy()
    {
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/emote:Village//sound:Hi/Hola, peque�o. �Sabes algo sobre la bestia que ronda por aqu�?", "PlayerDialogue"));

        dialogTexts.Add(new DialogData("/sound:Sorry/Lo siento, se�or... no s� nada sobre eso.", "NpcBoyDialogue"));

        dialogTexts.Add(new DialogData("/emote:Village/No te preocupes. Gracias de todos modos.", "PlayerDialogue"));

        dialogTexts.Add(new DialogData("Ojal� encuentre lo que busca...", "NpcBoyDialogue"));

        dialogManager.Show(dialogTexts);

        //Llamamos a la Coroutine para esperar que el dialogo termine
        StartCoroutine(WaitForDialogueEnd());
    }

    private void TextNpcWomen()
    {
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/sound:Hey/�Oh! Qu� sorpresa ver a un forastero por aqu�. �Qu� te trae a nuestra humilde aldea?", "NpcWomenDialogue"));

        dialogTexts.Add(new DialogData("/emote:Village//sound:Hey/Hola! He escuchado rumores sobre una bestia que ronda esta zona. Necesito informaci�n para encontrarla y ponerle fin a su amenaza.", "PlayerDialogue"));

        dialogTexts.Add(new DialogData("/sound:Hum/La bestia... �Nadie se atreve a hablar de ella! Ya ha destruido campos y ahuyentado a nuestros animales.", "NpcWomenDialogue"));

        dialogTexts.Add(new DialogData("/emote:Village/Entonces necesito saber d�nde encontrar a alguien que hable sin miedo. �Alg�n cazador en la aldea, quiz�s?", "PlayerDialogue"));

        dialogTexts.Add(new DialogData("/sound:sorry/No exactamente, pero si te diriges a la taberna, aqu� cerca, encontrar�s a los bandidos que siempre merodean por all�.", "NpcWomenDialogue"));

        dialogTexts.Add(new DialogData("/emote:Village//sound:What/�Bandidos?", "PlayerDialogue"));

        dialogTexts.Add(new DialogData("/sound:Yes/�S�! No siempre son amables, pero saben cosas que nadie m�s se atreve a contar. Puede que tengan la informaci�n que buscas... si tienes algo con qu� negociar, claro.", "NpcWomenDialogue"));

        dialogTexts.Add(new DialogData("/emote:Village//sound:Okey/Gracias, ha sido muy amable. Eso me basta para empezar.", "PlayerDialogue"));

        dialogTexts.Add(new DialogData("/sound:GoodLuck/Buena suerte, viajero. Y cuidado con los bandidos, a veces la bestia no es lo m�s peligroso en esta aldea.", "NpcWomenDialogue"));

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

