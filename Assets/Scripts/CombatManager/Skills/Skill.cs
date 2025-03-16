using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public abstract class Skill : MonoBehaviour
{
    [Header("Base Skill")]
    public string skillName;
    public float animationDuration;

    public SkillTargeting targeting;

    public GameObject effectPrfb;

    [Header("Animations")]
    public string emitterAnimationName; // Nombre de la animación para el emisor
    public string receiverAnimationName; // Nombre de la animación para el receptor

    [Header("Audio Settings")]
    public AudioClip emitterAudioClip; // Audio para el emisor
    public float emitterAudioDelay = 0f; // Delay para el audio del emisor
    public AudioClip receiverAudioClip; // Audio para el receptor
    public float receiverAudioDelay = 0f; // Delay para el audio del receptor

    protected Fighter emitter;
    protected List<Fighter> receivers;

    protected Queue<string> messages;

    public bool needsManualTargeting
    {
        get
        {
            switch (this.targeting)
            {
                case SkillTargeting.SINGLE_ALLY:
                case SkillTargeting.SINGLE_OPPONENT:
                    return true;

                default:
                    return false;
            }
        }
    }

    void Awake()
    {
        this.messages = new Queue<string>();
        this.receivers = new List<Fighter>();
    }

    protected void Animate(Fighter fighter, string animationName, bool instantiateEffect)
    {
        Animator animator = fighter.GetComponentInParent<Animator>();
        if (animator != null)
        {
            animator.Play(animationName);
        }

        if (instantiateEffect && this.effectPrfb != null)
        {
            var go = Instantiate(this.effectPrfb, fighter.transform.position, Quaternion.identity);
            Destroy(go, this.animationDuration);
        }
    }

    /*private void Animate(Fighter receiver)
    {
        var go = Instantiate(this.effectPrfb, receiver.transform.position, Quaternion.identity);
        Destroy(go, this.animationDuration);
    }*/

    private IEnumerator PlayAudioWithDelay(AudioSource audioSource, AudioClip clip, float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.PlayOneShot(clip);
    }

    public void Run()
    {
        // Animar al emisor de la habilidad sin instanciar el effectPrfb
        this.Animate(this.emitter, this.emitterAnimationName, false);

        // Reproducir el audio del emisor con delay
        if (this.emitterAudioClip != null)
        {
            AudioSource emitterAudioSource = this.emitter.GetComponentInParent<AudioSource>();
            if (emitterAudioSource != null)
            {
                StartCoroutine(PlayAudioWithDelay(emitterAudioSource, this.emitterAudioClip, this.emitterAudioDelay));
            }
        }

        foreach (var receiver in this.receivers)
        {
            this.OnRun(receiver);

            // Verificar si el golpe falló
            if (this.messages.Count > 0 && this.messages.Peek() == "MISS")
            {
                this.messages.Dequeue(); // Eliminar el mensaje de fallo
                continue; // Saltar la animación del receptor
            }

            // Animar al receptor de la habilidad e instanciar el effectPrfb
            this.Animate(receiver, this.receiverAnimationName, true); //Se agrega linea

            // Reproducir el audio del receptor con delay
            if (this.receiverAudioClip != null)
            {
                AudioSource receiverAudioSource = receiver.GetComponentInParent<AudioSource>();
                if (receiverAudioSource != null)
                {
                    StartCoroutine(PlayAudioWithDelay(receiverAudioSource, this.receiverAudioClip, this.receiverAudioDelay));
                }
            }
        }

        this.receivers.Clear();
    }

    public void SetEmitter(Fighter _emitter)
    {
        this.emitter = _emitter;
    }

    public void AddReceiver(Fighter _receiver)
    {
        this.receivers.Add(_receiver);
    }

    public string GetNextMessage()
    {
        if (this.messages.Count != 0)
            return this.messages.Dequeue();
        else
            return null;
    }

    protected abstract void OnRun(Fighter receiver);
}