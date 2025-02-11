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
    }
  

    private void TextNpcBandit()
    {
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/emote:Tavern/Hola, necesito tu ayuda... Dicen que sabes algo acerca de la bestia que ronda por estos lugares.", "PlayerDialogue"));

        dialogTexts.Add(new DialogData("/emote:Serious/¿Ayudarte?", "NpcBanditDialogue"));

        dialogTexts.Add(new DialogData("/emote:Normal/¿Y por qué debería?", "NpcBanditDialogue"));

        dialogTexts.Add(new DialogData("/emote:Happy/¡Ja! Primero invítame un trago, campesino. Quizá después te cuente algo... si estoy de humor.", "NpcBanditDialogue"));

        dialogTexts.Add(new DialogData("/emote:Tavern/Esta bien, ire a buscarlo,", "PlayerDialogue"));

        dialogTexts.Add(new DialogData("/emote:Normal//speed:down/. . . . . ."));

        dialogTexts.Add(new DialogData("/emote:Tavern/Aquí tienes tu trago. Ahora... ¿Me dirás algo?", "PlayerDialogue"));

        dialogTexts.Add(new DialogData("/emote:Normal/Bien, bien... La bestia vive en el camino hacia las montañas.", "NpcBanditDialogue"));

        dialogTexts.Add(new DialogData("/emote:Serious/Nadie en su sano juicio se atreve a ir allí. ¿Por qué quieres saber sobre la bestia?", "NpcBanditDialogue"));

        dialogTexts.Add(new DialogData("/emote:Tavern/Tiene cautiva a mi amada... Voy a enfrentarla.", "PlayerDialogue"));

        dialogTexts.Add(new DialogData("/emote:Happy/¡Ja! Estás loco.", "NpcBanditDialogue"));

        dialogTexts.Add(new DialogData("/emote:Normal/Además, ni siquiera llevas armadura ni espada. La bestia te hará pedazos.", "NpcBanditDialogue"));

        dialogTexts.Add(new DialogData("/emote:Happy/Aunque... si de verdad tienes tantas ganas de morir, quizá pueda ayudarte.", "NpcBanditDialogue"));

        dialogTexts.Add(new DialogData("/emote:Normal/Conozco a alguien. Un mago excéntrico que vive en las mazmorras, más allá del bosque.", "NpcBanditDialogue"));

        dialogTexts.Add(new DialogData("/emote:Tavern/¿Y ese mago cómo podría ayudarme?", "PlayerDialogue"));

        dialogTexts.Add(new DialogData("/emote:Serious/Se dice que tiene un orbe misterioso que otorga un poder inigualable a quien lo posee.", "NpcBanditDialogue"));

        dialogTexts.Add(new DialogData("/emote:Happy/Pero nadie ha regresado tras enfrentarse a sus pruebas... Quizá tú seas el primero.", "NpcBanditDialogue"));

        dialogTexts.Add(new DialogData("/emote:Tavern/Estoy dispuesto a correr el riesgo. Gracias por tu ayuda.", "PlayerDialogue"));

        dialogTexts.Add(new DialogData("/emote:Surprised/ESPERA!!!", "NpcBanditDialogue"));

        dialogTexts.Add(new DialogData("/emote:Serious/Si quieres llegar rápido a las mazmorras, conozco un camino secreto.", "NpcBanditDialogue"));

        dialogTexts.Add(new DialogData("/emote:Happy/Sigue las escaleras detrás de mí. Llegarás mucho antes.", "NpcBanditDialogue"));

        dialogTexts.Add(new DialogData("/emote:Tavern/Has sido de gran ayuda. Por cierto, ¿cómo te llamas?", "PlayerDialogue"));

        dialogTexts.Add(new DialogData("/emote:Serious/Me llaman el bandido de las colinas, pero muchos me conocen como...", "NpcBanditDialogue"));

        dialogTexts.Add(new DialogData("/emote:Happy/ \"KAUFMAN\"", "NpcBanditDialogue"));

        dialogTexts.Add(new DialogData("/emote:Tavern/Hasta luego, Kaufman.", "PlayerDialogue"));

        dialogTexts.Add(new DialogData("/emote:Happy/¡Suerte en tu travesía, loco valiente!", "NpcBanditDialogue"));

        dialogTexts.Add(new DialogData("/emote:Surprised/HA HA HA!", "NpcBanditDialogue"));

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

