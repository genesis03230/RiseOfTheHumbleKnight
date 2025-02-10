using Doublsb.Dialog;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TEST : MonoBehaviour
{
    public DialogManager dialogManager;

    private void Awake()
    {
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("Hola, necesito que me ayudes... Me dijeron que conoces a un mago misterioso", "Jona"));

        dialogTexts.Add(new DialogData("/emote:Serious/¿Ayudarte?", "Kaufman"));

        dialogTexts.Add(new DialogData("/emote:Happy/¡Ja! Primero invítame un trago, campesino.", "Kaufman"));

        dialogTexts.Add(new DialogData("Esta bien ire a buscarlo", "Jona"));

        dialogTexts.Add(new DialogData("/speed:down/. . . . . .", "Tavern"));

        dialogTexts.Add(new DialogData("Toma aqui tienes tu trago, ahora... Me ayudaras?", "Jona"));

        dialogTexts.Add(new DialogData("/emote:Normal/Bien, bien... Conozco a alguien. Un mago excéntrico, vive en una cueva en el bosque.", "Kaufman"));

        dialogTexts.Add(new DialogData("/emote:Surprised/Pero cuidado: sus juegos son peligrosos!!!", "Kaufman"));

        dialogTexts.Add(new DialogData("El bandido Kaufman me habia proporcionado informacion valiosa", "Empty"));

        dialogTexts.Add(new DialogData("Asi que me dirigi a la cueva /color:green/gamer/color:white/, donde habitaba el mago Bellbrack", "Empty"));

        dialogTexts.Add(new DialogData("/emote:Happy/Tu eres el mago capaz de hacer poderosos milagros?", "Jona"));

        dialogTexts.Add(new DialogData("/emote:Happy/Ah, un campesino valiente!", "Bellbrack"));

        dialogTexts.Add(new DialogData("/emote:Serious/¿Quieres poder?", "Bellbrack"));

        dialogTexts.Add(new DialogData("/emote:Happy/Claro que si, necesito convertirme en un caballero para rescatar a mi amada de la bestia", "Jona"));

        dialogTexts.Add(new DialogData("/emote:Surprised/Primero, juega conmigo. Te dire una pregunta!", "Bellbrack"));

        dialogTexts.Add(new DialogData("/emote:Happy/Si fallas...", "Bellbrack"));

        dialogTexts.Add(new DialogData("/color:red//speed:down//emote:Normal/MORIRAS!!!", "Bellbrack"));
       
        dialogTexts.Add(new DialogData("/emote:Happy/Bueno lo intentare", "Jona"));

        dialogTexts.Add(new DialogData("/emote:Happy/¿Cuántos juegos existen de Pokémon y su universo?", "Bellbrack"));

        dialogTexts.Add(new DialogData("/emote:Happy/QUE CULIAO....", "Jona"));



        dialogManager.Show(dialogTexts);
    }
}

