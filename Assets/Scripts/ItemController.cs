using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    [SerializeField] private DialogScriptDungeon dialogScriptDungeon;
    [SerializeField] private GameObject buttonASprite;
    [SerializeField] public GameObject particleEffect1;
    [SerializeField] public GameObject particleEffect2;
    [SerializeField] public AudioSource audioSourceDungeon;
    [SerializeField] public AudioSource audioSourceKnight;
    private bool isPlayerInRange;
    private Animator animator;
    private bool openChest;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (dialogScriptDungeon == null)
        {
            dialogScriptDungeon = GetComponent<DialogScriptDungeon>();
        }

        if (dialogScriptDungeon == null)
        {
            Debug.LogError("dialogScriptDungeon no encontrado.");
        }
    }

    public void OpenChest()
    {
        gameObject.layer = LayerMask.NameToLayer("Ground");
        animator.SetBool("open", true);
        buttonASprite.SetActive(false);
        openChest = true;
        audioSourceDungeon.Stop();
        particleEffect1.SetActive(true);
        particleEffect2.SetActive(true);
        dialogScriptDungeon.isSecondDialogue = false;
        dialogScriptDungeon.isThirdDialogue = true;
        StartCoroutine(Music());
    }

    private IEnumerator Music()
    {
        yield return new WaitForSeconds(4f);
        audioSourceKnight.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (openChest) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = true;
            buttonASprite.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = false;
            buttonASprite.SetActive(false);
        }
    }

}
