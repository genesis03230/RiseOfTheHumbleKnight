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

        dialogTexts.Add(new DialogData("/emote:Surprised//sound:Greetings/Ah, un intrépido aventurero que ha caído en mi trampa... O tal vez no tan intrépido, después de todo.", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Dungeon//sound:Hey/Tu eres el mago de las Mazmorras?.", "PlayerDialogue"));

        dialogTexts.Add(new DialogData("/emote:Happy//sound:Yes/Asi es! Soy el mago /color:green/BELLBRACK", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Surprised//sound:Oh/¿Has venido por el poder del orbe?", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Dungeon//sound:Yeah/He escuchado sobre tu orbe mágico... ", "PlayerDialogue"));

        dialogTexts.Add(new DialogData("/emote:Dungeon/y sé que sólo tú puedes darme el poder que necesito para salvar a mi amada de la bestia.", "PlayerDialogue"));

        dialogTexts.Add(new DialogData("/emote:Serious//sound:Haaa/La ¿BESTIA?, seguro te refieres al...", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Normal//color:red/DRAGON NEGRO DE LAS MONTAÑAS", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Happy//sound:Great/Muy bien, te daré el orbe, pero primero deberás demostrar tu valía.", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Dungeon//sound:Ready/Estoy listo. No importa lo que deba hacer, lo lograré.", "PlayerDialogue"));

        dialogTexts.Add(new DialogData("/emote:Serious//sound:Oh/Mira esas runas en la pared. Allí donde la luz roja brilla, ese es el comienzo de nuestro juego.", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Dungeon//sound:What/Las runas... ¿Qué tengo que hacer exactamente?", "PlayerDialogue"));

        dialogTexts.Add(new DialogData("/emote:Serious/Si logras completar mi desafío, el poder será tuyo.", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Normal//sound:Haaa/Si fallas...", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Happy/Bueno, no será tan fácil. /sound:Jajaja/HA HA HA!!", "NpcMagicianDialogue"));

        dialogManager.Show(dialogTexts);

        //Llamamos a la Coroutine para esperar que el dialogo termine
        StartCoroutine(WaitForDialogueEnd());
    }


    private void SecondTextNpcMagician()
    {
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/emote:Surprised//sound:Great/No esperaba que alguien tan audaz pudiese superar mi prueba.", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Serious//sound:Haaa/Ahora, como prometí, el poder está a tu alcance.", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Serious/El orbe está dentro de ese cofre.", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Surprised//sound:Oh/Ábrelo y sentirás cómo la magia fluye a través de ti.", "NpcMagicianDialogue"));

        dialogManager.Show(dialogTexts);

        //Llamamos a la Coroutine para esperar que el dialogo termine
        StartCoroutine(WaitForDialogueEnd());
    }

    private void ThirdTextNpcMagician()
    {
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/emote:Surprised//sound:Ohhh/Ahora que has obtenido el poder del orbe, hay algo más que debo revelarte", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Happy//sound:JAJA/Joven caballero!", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Serious/Aquí, en lo profundo de estas mazmorras, hay un camino secreto.", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Dungeon//sound:What/Esta mazmorra está llena de sorpresas.", "PlayerKnightDialogue"));

        dialogTexts.Add(new DialogData("/emote:Surprised//sound:Oh/Este atajo te llevará directamente a las montañas.", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Surprised/Donde aguarda el /color:red//emote:Serious/DRAGON NEGRO.", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Dungeon//sound:Okey/Si este pasaje me lleva más cerca de las montañas, lo tomaré.", "PlayerKnightDialogue"));

        dialogTexts.Add(new DialogData("/emote:Surprised//sound:Ohhh/No dejes que la oscuridad te detenga.", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Normal/Tu misión es clara.", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Serious//sound:Haaa/El destino de tu amada está en tus manos.", "NpcMagicianDialogue"));

        dialogTexts.Add(new DialogData("/emote:Dungeon//sound:Yes/Gracias, mago. Ahora sé que mi destino me espera.", "PlayerKnightDialogue"));

        dialogTexts.Add(new DialogData("/emote:Dungeon//sound:Onmyway/La oscuridad no me detendrá, ni el dragón, ni nada.", "PlayerKnightDialogue"));

        dialogTexts.Add(new DialogData("/emote:Happy//sound:JAJA/Adelante, humilde caballero!!!", "NpcMagicianDialogue"));


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

