using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform m_transform;
    private Rigidbody2D m_rigidbody2D;
    private GatherInput m_gatherInput;
    private Animator m_animator;

    //ANIMATOR IDS
    private int idSpeed;

    [Header("Move Settings")]
    [SerializeField] private float speed;
    private int direction = -1;

    [Header("NPC Settings")]
    [SerializeField] private float checkNpcDistance;
    [SerializeField] private bool isNpcDetected;
    [SerializeField] private LayerMask npcLayer;
    [SerializeField] private bool isDialogueActive = false;
    private GameObject currentNpc;


    private void Awake()
    {
        m_gatherInput = GetComponent<GatherInput>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        idSpeed = Animator.StringToHash("speed");
    }

    private void Update()
    {
        SetAnimatorValues();
    }

    private void SetAnimatorValues()
    {
        // Se considera tanto la velocidad en el eje X como en el eje Y
        float velocityMagnitude = new Vector2(Mathf.Abs(m_rigidbody2D.velocity.x), Mathf.Abs(m_rigidbody2D.velocity.y)).magnitude;
        m_animator.SetFloat(idSpeed, velocityMagnitude);
    }

    void FixedUpdate()
    {
        Move();
        HandleNpc();
        HandleInput();
    }

    private void Move()
    {
        if (isDialogueActive) return;
        Vector2 input = m_gatherInput.Value;
        m_rigidbody2D.velocity = input * speed;

        // Solo realizar Flip si el movimiento es horizontal
        if (input.x != 0)
        {
            Flip(input.x);
        }
    }

    private void Flip(float horizontalInput)
    {
        if (horizontalInput * direction < 0)
        {
            HandleDirection();
        }
    }

    private void HandleDirection()
    {
        m_transform.localScale = new Vector3(-m_transform.localScale.x, m_transform.localScale.y, 1);
        direction *= -1;
    }

    private void HandleNpc()
    {
        RaycastHit2D hit = Physics2D.Raycast(m_transform.position, Vector2.right * direction, checkNpcDistance, npcLayer);
        if (hit.collider != null)
        {
            isNpcDetected = true;
            currentNpc = hit.collider.gameObject; // Guarda el NPC detectado
        }
        else
        {
            isNpcDetected = false;
            currentNpc = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(m_transform.position, new Vector2(m_transform.position.x + (checkNpcDistance * direction), m_transform.position.y));
    }

    private void HandleInput()
    {
        if (m_gatherInput.IsAction && isNpcDetected && !isDialogueActive)
        {
            StartDialogue();
        }
    }

    private void StartDialogue()
    {
        if (currentNpc != null)
        {
            isDialogueActive = true;
            string npcName = currentNpc.name; // Obtiene el nombre del NPC
            DialogScript.Instance.StartDialogue(npcName); // Llama al sistema de diálogo con el NPC específico
        }
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        currentNpc = null;
    }

}
