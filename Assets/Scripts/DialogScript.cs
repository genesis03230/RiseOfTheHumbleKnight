using Doublsb.Dialog;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogScript : MonoBehaviour
{
    public static DialogScript Instance { get; private set; }

    public DialogManager dialogManager;

    public PatrolCharacter patrolCharacter;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject npcBoyDialogue;
    //[SerializeField] private GameObject npcWomenDialogue;
    //[SerializeField] private GameObject npcBanditDialogue;
    //[SerializeField] private GameObject npcMagicianDialogue;
    [SerializeField] private GameObject printer;
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
        npcBoyDialogue.SetActive(true);

        switch (npcName)
        {
            case "NpcBoy":
                patrolCharacter.PausePatrol();
                TextNpcBoy();
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

        isDialogueActive = false;

        StartCoroutine(WaitForNewAction());
    }

    private IEnumerator WaitForNewAction()
    {
        yield return new WaitForSeconds(0.5f); //Esperar para que el Raycast no detecte enseguida el dialogo nuevamente

        patrolCharacter.ResumePatrol();

        //Notifica al PlayerController que ya puede interactuar de nuevo
        FindObjectOfType<PlayerController>().EndDialogue();
    }

    private void TextNpcBoy()
    {
        

        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/emote:Village/Hola, pequeño. ¿Sabes algo sobre la bestia que ronda por aquí?", "PlayerDialogue"));

        dialogTexts.Add(new DialogData("Lo siento, señor... no sé nada sobre eso.", "NpcBoyDialogue"));

        dialogTexts.Add(new DialogData("/emote:Village/No te preocupes. Gracias de todos modos.", "PlayerDialogue"));

        dialogTexts.Add(new DialogData("Ojalá encuentre lo que busca...", "NpcBoyDialogue"));

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

