using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerInputManager : MonoBehaviour
{
    public charecterController m_controler;
    private Controls m_controls;
    private Vector2 m_moveDirection;

    private void OnEnable()
    {
        m_controls = new Controls();
        m_controls.player.move.performed += HandleMove;
        m_controls.player.move.canceled += HandleMove;
        m_controls.player.move.Enable();
    }

    private void FixedUpdate()
    {
        m_controler.Move(m_moveDirection);
    }

    private void OnDisable()
    {
        m_controls.player.move.performed -= HandleMove;
        m_controls.player.move.canceled -= HandleMove;
        m_controls.player.move.Disable();
    }

    private void HandleMove(InputAction.CallbackContext context)
    {
        m_moveDirection = context.ReadValue<Vector2>();
        m_controler.Move(m_moveDirection);
    }
}
