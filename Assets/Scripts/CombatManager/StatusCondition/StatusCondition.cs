using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public abstract class StatusCondition : MonoBehaviour
{
    /*[Header("Base Status Condition")]
    public GameObject effectPrfb;
    public float animationDuration;

    public string receptionMessage;
    public string applyMessage;
    public string expireMessage;

    public int turnDuration;

    public bool hasExpired { get { return this.turnDuration <= 0; } }

    protected Queue<string> messages;
    protected Fighter receiver;

    public void Awake()
    {
        this.messages = new Queue<string>();
    }

    public void SetReceiver(Fighter recv)
    {
        this.receiver = recv;
    }

    private void Animate()
    {
        var go = Instantiate(this.effectPrfb, this.receiver.transform.position, Quaternion.identity);
        Destroy(go, this.animationDuration);
    }

    public void Apply()
    {
        if (this.receiver == null)
        {
            throw new System.InvalidOperationException("StatusCondition needs a receiver");
        }

        if (this.OnApply())
        {
            this.Animate();
        }

        turnDuration--;

        if (this.hasExpired)
        {
            this.messages.Enqueue(this.expireMessage.Replace("{receiver}", this.receiver.idName));
        }
    }

    public string GetNextMessage()
    {
        if (this.messages.Count != 0)
            return this.messages.Dequeue();
        else
            return null;
    }

    public string GetReceptionMessage()
    {
        return this.receptionMessage.Replace("{receiver}", this.receiver.idName);
    }

    public abstract bool OnApply();
    public abstract bool BlocksTurn();*/





    [Header("Base Status Condition")]
    public GameObject effectPrfb;
    public float animationDuration;
    public string receptionMessage;
    public string applyMessage;
    public string expireMessage;
    public int turnDuration;
    public bool hasExpired { get; private set; }
    protected Queue<string> messages;
    protected Fighter receiver;

    [Header("Animations")]
    public string emitterAnimationName; // Nombre de la animación para el emisor
    public string receiverAnimationName; // Nombre de la animación para el receptor

    [Header("Audio Settings")]
    public AudioClip emitterAudioClip; // Audio para el emisor
    public float emitterAudioDelay = 0f; // Delay para el audio del emisor
    public AudioClip receiverAudioClip; // Audio para el receptor
    public float receiverAudioDelay = 0f; // Delay para el audio del receptor

    public void Awake()
    {
        this.messages = new Queue<string>();
    }

    public void SetReceiver(Fighter recv)
    {
        this.receiver = recv;
    }

    private void Animate(Fighter fighter, string animationName, bool instantiateEffect, AudioClip audioClip, float audioDelay)
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

        if (audioClip != null)
        {
            AudioSource audioSource = fighter.GetComponentInParent<AudioSource>();
            if (audioSource != null)
            {
                StartCoroutine(PlayAudioWithDelay(audioSource, audioClip, audioDelay));
            }
        }
    }

    private IEnumerator PlayAudioWithDelay(AudioSource audioSource, AudioClip clip, float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.PlayOneShot(clip);
    }

    /*private void Animate()
    {
        var go = Instantiate(this.effectPrfb, this.receiver.transform.position, Quaternion.identity);
        Destroy(go, this.animationDuration);
    }*/

    public void Apply()
    {
        if (this.turnDuration > 0)
        {
            this.OnApply();
            this.Animate(this.receiver, this.receiverAnimationName, true, this.receiverAudioClip, this.receiverAudioDelay);
            this.turnDuration--;
            if (this.turnDuration == 0)
            {
                this.Expire();
            }
        }
    }

    public string GetNextMessage()
    {
        if (this.messages.Count != 0)
            return this.messages.Dequeue();
        else
            return null;
    }

    public string GetReceptionMessage()
    {
        return this.receptionMessage;
    }

    public abstract bool OnApply();

    public abstract bool BlocksTurn();

    private void Expire()
    {
        this.hasExpired = true;
        this.messages.Enqueue(this.expireMessage);
        Destroy(this.gameObject);
    }
}