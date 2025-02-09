using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GatherInput : MonoBehaviour
{
    private Controls controls;
    [SerializeField] private Vector2 _value;
    public Vector2 Value { get => _value; }
    
    [SerializeField] private bool _isAction;
    public bool IsAction { get => _isAction; set => _isAction = value; }

    private void Awake()
    {
        controls = new Controls();
    }

    private void OnEnable()
    {
        controls.Player.Move.performed += StartMove;
        controls.Player.Move.canceled += StopMove;
        controls.Player.Action.performed += StartAction;
        controls.Player.Action.canceled += StopAction;
        controls.Player.Enable();
    }

    private void StartMove(InputAction.CallbackContext context)
    {
        _value = context.ReadValue<Vector2>().normalized;
    }

    private void StopMove(InputAction.CallbackContext context)
    {
        _value = Vector2.zero;
    }

    private void StartAction(InputAction.CallbackContext context)
    {
        _isAction = true;
    }

    private void StopAction(InputAction.CallbackContext context)
    {
        _isAction = false;
    }

    private void OnDisable()
    {
        controls.Player.Move.performed -= StartMove;
        controls.Player.Move.canceled -= StopMove;
        controls.Player.Action.performed -= StartAction;
        controls.Player.Action.canceled -= StopAction;
        controls.Player.Disable();
    }
}
