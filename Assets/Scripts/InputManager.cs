﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] ChargesManager charges;
    Vector2 axisMouseWheel = Vector2.zero;
    Vector2 mousePosition = Vector2.zero;
    bool isDoingForce;
    public static InputManager singletone;
    private void Awake()
    {
        if(singletone == null)
        {
            singletone = this;
        }
    }
    public void OnWheel(InputAction.CallbackContext ctx)
    {  
        if (ctx.performed)
        {
            axisMouseWheel = ctx.ReadValue<Vector2>();
            charges.SetSelector(axisMouseWheel.y);
        }
    }
    public void OnPutCharge(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        { 
            charges.ChargeSpawn(mousePosition);
            //charges.Edit(mousePosition);
        }
    }
    public void OnEditCharge(InputAction.CallbackContext ctx)
    {
        //charges.EditMode();
    }

    public void OnDoForce(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            isDoingForce = true;
            
        }
        if (ctx.canceled)
        {
            isDoingForce = false;
        }
    }
    public void OnMouseMovement(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            mousePosition = ctx.ReadValue<Vector2>();
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        }
    }
    public Vector2 GetMousePos()
    {
        return mousePosition;
    }
    public bool GetIsDoingForce()
    {
        return isDoingForce;
    }
}
