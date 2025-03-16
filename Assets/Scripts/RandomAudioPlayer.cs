using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudioPlayer : MonoBehaviour
{
    public AudioSource audioSource; // Referencia al AudioSource
    public AudioClip[] audioClips;  // Array con los audios (debe contener 2 clips)

    void OnEnable()
    {
        if (audioClips.Length > 0)
        {
            int randomIndex = Random.Range(0, audioClips.Length); // Elegir aleatoriamente un índice
            audioSource.clip = audioClips[randomIndex]; // Asignar el audio al AudioSource
            audioSource.Play(); // Reproducir el audio
        }
    }
}
