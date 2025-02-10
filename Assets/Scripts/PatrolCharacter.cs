using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolCharacter : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float waitTime = 1.5f;
    public Transform leftPoint, rightPoint;

    private Animator animator;
    private Vector3 targetPosition;
    private bool movingRight = true;
    private bool isWaiting = false;
    private bool isPaused = false; // Nuevo flag para pausar el movimiento

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        targetPosition = rightPoint.position;
    }

    void Update()
    {
        if (isPaused)
        {
            animator.SetBool("isWalking", false); // Asegurar animacion Idle al pausar
            return;
        }

        if (isWaiting)
        {
            animator.SetBool("isWalking", false); 
            return;
        }

        // Movimiento del personaje
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Comprobar si llegó al punto de destino
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            StartCoroutine(WaitAndSwitchDirection());
        }

        // Activar animación de caminar
        animator.SetBool("isWalking", true);

        // Controlar la rotación (flip)
        if (movingRight && transform.localScale.x < 0)
        {
            Flip();
        }
        else if (!movingRight && transform.localScale.x > 0)
        {
            Flip();
        }
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private IEnumerator WaitAndSwitchDirection()
    {
        animator.SetBool("isWalking", false); // Cambiar a Idle
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);

        // Cambiar de dirección
        movingRight = !movingRight;
        targetPosition = movingRight ? rightPoint.position : leftPoint.position;

        isWaiting = false;
    }

    // Método público para pausar el personaje
    public void PausePatrol()
    {
        isPaused = true;
    }

    // Método público para reanudar el patrullaje
    public void ResumePatrol()
    {
        isPaused = false;
    }
}
