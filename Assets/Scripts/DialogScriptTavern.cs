using Doublsb.Dialog;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogScriptTavern : MonoBehaviour
{
    public static DialogScriptTavern Instance { get; private set; }

    public DialogManager dialogManager;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject npcBanditDialogue;
    [SerializeField] private GameObject printer;
    [SerializeField] private GameObject trigger;
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
            case "NpcBandit":
                trigger.SetActive(true);
                npcBanditDialogue.SetActive(true);
                TextNpcBandit();
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

        npcBanditDialogue.SetActive(false);

        isDialogueActive = false;

        StartCoroutine(WaitForNewAction());
    }

    private IEnumerator WaitForNewAction()
    {
        yield return new WaitForSeconds(0.2f); //Esperar para que el Raycast no detecte enseguida el dialogo nuevamente

        //Notifica al PlayerController que ya puede interactuar de nuevo
        FindObjectOfType<PlayerController>().EndDialogue();
    }


    private void TextNpcBandit()
    {
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/emote:Tavern//sound:Hi/Hola, necesito tu ayuda... Dicen que sabes algo acerca de la bestia que ronda por estos lugares.", "PlayerDialogue"));

        dialogTexts.Add(new DialogData("/emote:Serious//sound:Haa/¿Ayudarte?", "NpcBanditDialogue"));

        dialogTexts.Add(new DialogData("/emote:Normal//sound:Naaa/¿Y por qué debería?", "NpcBanditDialogue"));

        dialogTexts.Add(new DialogData("/emote:Happy//sound:Jajaja/Primero invítame un trago, campesino. Quizá después te cuente algo... si estoy de humor.", "NpcBanditDialogue"));

        dialogTexts.Add(new DialogData("/emote:Tavern//sound:Onmyway/Esta bien, ire a buscarlo,", "PlayerDialogue"));

        dialogTexts.Add(new DialogData("/emote:Normal//speed:down//sound:Barman/. . . . . ."));

        dialogTexts.Add(new DialogData("/emote:Tavern//sound:Onet/Aquí tienes tu trago. Ahora... ¿Me dirás algo?", "PlayerDialogue"));

        dialogTexts.Add(new DialogData("/emote:Normal//sound:Mmm/Bien, bien... La bestia vive en el camino hacia las montañas.", "NpcBanditDialogue"));

        dialogTexts.Add(new DialogData("/emote:Serious//sound:Grunting/Nadie en su sano juicio se atreve a ir allí. /emote:Normal//sound:Haa/¿Por qué quieres saber sobre la bestia?", "NpcBanditDialogue"));

        dialogTexts.Add(new DialogData("/emote:Tavern/Tiene cautiva a mi amada... Voy a enfrentarla.", "PlayerDialogue"));

        dialogTexts.Add(new DialogData("/emote:Happy//sound:Jajaja/¡Ja! Estás loco.", "NpcBanditDialogue"));

        dialogTexts.Add(new DialogData("/emote:Serious//sound:Grunting2/Además, ni siquiera llevas armadura ni espada. /emote:Normal//sound:Grunting/La bestia te hará pedazos.", "NpcBanditDialogue"));

        dialogTexts.Add(new DialogData("/emote:Happy/Aunque... si de verdad tienes tantas ganas de morir, quizá pueda ayudarte.", "NpcBanditDialogue"));

        dialogTexts.Add(new DialogData("/emote:Normal//sound:Mmm/Conozco a alguien. Un mago excéntrico que vive en las mazmorras, más allá del bosque.", "NpcBanditDialogue"));

        dialogTexts.Add(new DialogData("/emote:Tavern//sound:What/¿Y ese mago cómo podría ayudarme?", "PlayerDialogue"));

        dialogTexts.Add(new DialogData("/emote:Serious//sound:Grunting/Se dice que tiene un orbe misterioso que otorga un poder inigualable a quien lo posee.", "NpcBanditDialogue"));

        dialogTexts.Add(new DialogData("/emote:Happy/Pero nadie ha regresado tras enfrentarse a sus pruebas... Quizá tú seas el primero.", "NpcBanditDialogue"));

        dialogTexts.Add(new DialogData("/emote:Tavern/Estoy dispuesto a correr el riesgo. /sound:Great/Gracias por tu ayuda.", "PlayerDialogue"));

        dialogTexts.Add(new DialogData("/emote:Surprised//sound:Hey/ESPERA!!!", "NpcBanditDialogue"));

        dialogTexts.Add(new DialogData("/emote:Serious//sound:Grunting2/Si quieres llegar rápido a las mazmorras, /emote:Normal//sound:Grunting/conozco un camino secreto.", "NpcBanditDialogue"));

        dialogTexts.Add(new DialogData("/emote:Happy/Sigue las escaleras detrás de mí. Llegarás mucho antes.", "NpcBanditDialogue"));

        dialogTexts.Add(new DialogData("/emote:Tavern//sound:Okey/Has sido de gran ayuda. Por cierto, ¿cómo te llamas?", "PlayerDialogue"));

        dialogTexts.Add(new DialogData("/emote:Serious//sound:Grunting2/Me llaman el bandido de las colinas, pero muchos me conocen como...", "NpcBanditDialogue"));

        dialogTexts.Add(new DialogData("/emote:Happy//color:green/\"KAUFMAN\"", "NpcBanditDialogue"));

        dialogTexts.Add(new DialogData("/emote:Tavern//sound:Bye/Hasta luego, Kaufman.", "PlayerDialogue"));

        dialogTexts.Add(new DialogData("/emote:Happy//sound:Adios/¡Suerte en tu travesía, loco valiente!", "NpcBanditDialogue"));

        dialogTexts.Add(new DialogData("/emote:Surprised//sound:Jajaja/HA HA HA!", "NpcBanditDialogue"));

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

