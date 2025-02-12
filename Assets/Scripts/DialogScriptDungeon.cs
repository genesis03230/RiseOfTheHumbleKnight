using Doublsb.Dialog;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogScriptDungeon : MonoBehaviour
{
    public static DialogScriptDungeon Instance { get; set; }

    public DialogManager dialogManager;

    [SerializeField] private GameObject playerDialogue;
    [SerializeField] private GameObject playerKnightDialogue;
    [SerializeField] private GameObject npcMagicianDialogue;
    [SerializeField] private GameObject printer;
    [SerializeField] private GameObject triggerMiniGame;
    [SerializeField] private GameObject triggerChest;
    [SerializeField] private GameObject trigger;
    private bool isDialogueActive;
    public bool isFirstDialogue = true;
    public bool isSecondDialogue = false;
    public bool isThirdDialogue = false;

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
        playerDialogue.SetActive(true);
        

        if (isFirstDialogue)
        {
            switch (npcName)
            {
                case "NpcMagician":
                    triggerMiniGame.SetActive(true);
                    npcMagicianDialogue.SetActive(true);
                    FirstTextNpcMagician();
                    break;

                default:
                    Debug.LogWarning("NPC no reconocido: " + npcName);
                    EndDialogue();
                    break;
            }
        }

        if (isSecondDialogue)
        {
            switch (npcName)
            {
                case "NpcMagician":
                    triggerChest.SetActive(true);
                    npcMagicianDialogue.SetActive(true);
                    SecondTextNpcMagician();
                    break;

                default:
                    Debug.LogWarning("NPC no reconocido: " + npcName);
                    EndDialogue();
                    break;
            }
        }

        if (isThirdDialogue)
        {
            switch (npcName)
            {
                case "NpcMagician":
                    trigger.SetActive(true);
                    npcMagicianDialogue.SetActive(true);
                    ThirdTextNpcMagician();
                    break;

                default:
                    Debug.LogWarning("NPC no reconocido: " + npcName);
                    EndDialogue();
                    break;
            }
        }
    }

   

    public void EndDialogue()
    {
        //Desactiva el Canvas de dialogo
        printer.SetActive(false);
        playerDialogue.SetActive(false);
        npcMagicianDialogue.SetActive(false);
        isDialogueActive = false;

        StartCoroutine(WaitForNewAction());
    }

    private IEnumerator WaitForNewAction()
    {
        yield return new WaitForSeconds(0.2f); //Esperar para que el Raycast no detecte enseguida el dialogo nuevamente

        //Notifica al PlayerController que ya puede interactuar de nuevo
        FindObjectOfType<PlayerController>().EndDialogue();
    }


    private void FirstTextNpcMagician()
    {
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/emote:Surprised/Ah, un intr�pido aventurero que ha ca�do en mi trampa... O tal vez no tan intr�pido, despu�s de todo.", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Dungeon/Tu eres el mago de las Mazmorras?.", "PlayerDialogue"));

        dialogTexts.Add(new DialogData("/emote:Happy/Asi es! Soy el mago /color:green/BELLBRACK", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Surprised/�Has venido por el poder del orbe?", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Dungeon/He escuchado sobre tu orbe m�gico... ", "PlayerDialogue"));

        dialogTexts.Add(new DialogData("/emote:Dungeon/y s� que s�lo t� puedes darme el poder que necesito para salvar a mi amada de la bestia.", "PlayerDialogue"));

        dialogTexts.Add(new DialogData("/emote:Serious/La �BESTIA?, seguro te refieres al...", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Normal//color:red/DRAGON NEGRO DE LA MONTA�AS", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Happy/Muy bien, te dar� el orbe, pero primero deber�s demostrar tu val�a.", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Dungeon/Estoy listo. No importa lo que deba hacer, lo lograr�.", "PlayerDialogue"));

        dialogTexts.Add(new DialogData("/emote:Serious/Mira esas runas en la pared. All� donde la luz roja brilla, ese es el comienzo de nuestro juego.", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Dungeon/Las runas... �Qu� tengo que hacer exactamente?", "PlayerDialogue"));

        dialogTexts.Add(new DialogData("/emote:Serious/Si logras completar mi desaf�o, el poder ser� tuyo.", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Normal/Si fallas...", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Happy/Bueno, no ser� tan f�cil. HA HA HA!!", "NpcMagicianDialogue"));

        dialogManager.Show(dialogTexts);

        //Llamamos a la Coroutine para esperar que el dialogo termine
        StartCoroutine(WaitForDialogueEnd());
    }


    private void SecondTextNpcMagician()
    {
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/emote:Surprised/No esperaba que alguien tan audaz pudiese superar mi prueba.", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Serious/Ahora, como promet�, el poder est� a tu alcance.", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Serious/El orbe est� dentro de ese cofre.", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Surprised/�brelo y sentir�s c�mo la magia fluye a trav�s de ti.", "NpcMagicianDialogue"));

        dialogManager.Show(dialogTexts);

        //Llamamos a la Coroutine para esperar que el dialogo termine
        StartCoroutine(WaitForDialogueEnd());
    }

    private void ThirdTextNpcMagician()
    {
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/emote:Normal/Ahora que has obtenido el poder del orbe, hay algo m�s que debo revelarte", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Happy/Joven caballero!", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Serious/Aqu�, en lo profundo de estas mazmorras, hay un camino secreto.", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Dungeon/Esta mazmorras est� llena de sorpresas.", "PlayerKnightDialogue"));

        dialogTexts.Add(new DialogData("/emote:Surprised/Este atajo te llevar� directamente a las monta�as.", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Surprised/Donde aguarda el /color:red/DRAGON NEGRO.", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Dungeon/Si este pasaje me lleva m�s cerca de las monta�as, lo tomar�.", "PlayerKnightDialogue"));

        dialogTexts.Add(new DialogData("/emote:Normal/No dejes que la oscuridad te detenga.", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Normal/Tu misi�n es clara.", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Serious/El destino de tu amada est� en tus manos.", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Dungeon/Gracias, mago. Ahora s� que mi destino me espera.", "PlayerKnightDialogue"));

        dialogTexts.Add(new DialogData("/emote:Dungeon/La oscuridad no me detendr�, ni el drag�n, ni nada.", "PlayerKnightDialogue"));

        dialogTexts.Add(new DialogData("/emote:Happy/Adelante, humilde caballero!!!", "NpcMagicianDialogue"));


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

