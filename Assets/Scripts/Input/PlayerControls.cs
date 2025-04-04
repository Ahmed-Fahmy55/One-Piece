using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour, Controls.IPlayerActions
{
    public event Action OnAbility1Use;
    public event Action OnAbility2Use;
    public event Action OnAbility3Use;

    public bool JumpDown { get; set; }
    public bool JumpHeld { get; private set; }
    public float HorizontalMovement { get; private set; }


    private Controls inputs;

    private void Start()
    {
        inputs = new Controls();
        inputs.Player.SetCallbacks(this);
        inputs.Player.Enable();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            JumpDown = true;
        }
        else if(context.canceled)
        {
            JumpDown= false;
        }
    }

    public void OnJumpHold(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            JumpHeld = true;
        }
        else if (context.canceled)
        {
            JumpHeld = false;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        HorizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void OnAbility1(InputAction.CallbackContext context)
    {
        OnAbility1Use?.Invoke();
    }

    public void OnAbility2(InputAction.CallbackContext context)
    {
        OnAbility2Use?.Invoke();
    }

    public void OnAbility3(InputAction.CallbackContext context)
    {
        OnAbility3Use?.Invoke();
    }
}
