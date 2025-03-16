using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    public DialogScriptCastle dialogScriptCastle;
    public float moveSpeed = 2f;
    public float moveDistance = 5f;
    public Animator animator;

    private bool shouldMove = false;
    private bool hasStopped = false;
    private Vector3 targetPosition;

    [SerializeField] private GameObject buttonASprite;
    private bool isPlayerInRange;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        targetPosition = transform.position + Vector3.left * moveDistance;
    }

    void Update()
    {
        if (shouldMove && !hasStopped)
        {
            MoveLeft();
        }
    }

    public void StartMoving()
    {
        shouldMove = true;
        animator.SetBool("isWalking", true);
    }

    private void MoveLeft()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            StopMoving();
        }
    }

    private void StopMoving()
    {
        shouldMove = false;
        hasStopped = true;
        animator.SetBool("isWalking", false);
        StartCoroutine(FinalDialogue());
    }

    private IEnumerator FinalDialogue()
    {
        yield return new WaitForSeconds(1.5f);
        DialogScriptCastle.Instance.StartDialogue();
    }

    public bool HasStopped()
    {
        return hasStopped;
    }

    //Logica para activar o desactivar boton para interactuar

    private void OnTriggerEnter2D(Collider2D collision)
    {
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
