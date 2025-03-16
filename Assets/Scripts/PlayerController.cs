using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform m_transform;
    [SerializeField] private GameObject playerKnightPrefab; //Prefab del nuevo player
    private Rigidbody2D m_rigidbody2D;
    private GatherInput m_gatherInput;
    private Animator m_animator;
    private ItemController itemController;
    private bool isSwitchingCharacter; //Evita multiples activaciones
    public static bool isMiniGameActive = false;


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

    [Header("Items Settings")]
    [SerializeField] private float checkItemsDistance;
    [SerializeField] private bool isItemsDetected;
    [SerializeField] private LayerMask itemsLayer;
    private GameObject detectedItem;
   

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
        HandleItems();
    }

    private void Move()
    {
        if (isDialogueActive) return;
        if (isMiniGameActive) return;
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
        if (isDialogueActive) return;

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

    private void HandleItems()
    {
        RaycastHit2D hit = Physics2D.Raycast(m_transform.position, Vector2.right * direction, checkItemsDistance, itemsLayer); //Crea otro rayo que detecta el itemsLayer
        if (hit.collider != null) //Si el hit con el collider no es nulo
        {
            isItemsDetected = true; //Detecta items
            detectedItem = hit.collider.gameObject; //Guarda el item detectado en la variable detectedItem
        }
        else
        {
            isItemsDetected = false; //Sino no detecta items
            detectedItem = null; //Limpia la referencia si no hay deteccion
        }
    }

    private void HandleInput()
    {
        if (m_gatherInput.IsAction && isNpcDetected && !isDialogueActive)
        {
            StartDialogue();
        }

        if (m_gatherInput.IsAction && isItemsDetected) //Si presiono la tecla correspondiente y puedo detectar items
        {
            if (detectedItem != null) //Verifica si hay un item guardado
            {
                ItemController itemController = detectedItem.GetComponent<ItemController>();
                if (itemController != null)
                {
                    itemController.OpenChest(); //Activa el metodo OpenChest dentro del ItemController

                    if (!isSwitchingCharacter) //Verifica que no haya otro cambio en proceso
                    {
                        StartCoroutine(SwapToKnight());
                    } 
                }
            }
        }
    }

    private IEnumerator SwapToKnight()
    {
        isSwitchingCharacter = true;

        // Desactiva el Player actual
        gameObject.SetActive(false);

        // Activa el PlayerKnight ya existente en la jerarquía
        if (playerKnightPrefab != null)
        {
            yield return new WaitForSeconds(2f);
            playerKnightPrefab.transform.position = transform.position; // Lo coloca en la misma posición
            playerKnightPrefab.SetActive(true);
        }
        else
        {
            Debug.LogError("No se ha asignado el PlayerKnight en el Inspector.");
        }

        isSwitchingCharacter = false;
    }

    private void StartDialogue()
    {
        if (currentNpc != null)
        {
            isDialogueActive = true;
            string npcName = currentNpc.name; // Obtiene el nombre del NPC
            string currentScene = SceneManager.GetActiveScene().name;

            // Verifica la escena actual y llama al script correspondiente
            if (currentScene == "VillageScene")
            {
                DialogScriptVillage.Instance?.StartDialogue(npcName);
            }
            else if (currentScene == "TavernScene")
            {
                DialogScriptTavern.Instance?.StartDialogue(npcName);
            }
            else if (currentScene == "DungeonScene")
            {
                DialogScriptDungeon.Instance?.StartDialogue(npcName);
            }
            else if (currentScene == "FinalScene")
            {
                DialogScriptDungeon.Instance?.StartDialogue(npcName);
            }
            else
            {
                Debug.LogWarning("No hay script de diálogo para esta escena.");
            }
         
        }
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        currentNpc = null;
    }

}
