﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChargesManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] GameObject positiveCharge;
    [SerializeField] GameObject negativeCharge;
    [SerializeField] float timeToDestroy;
    float count = 0;
    [Header("General Settings")]
    [SerializeField] int numCharges;
    [SerializeField] float mouseForce;
    [SerializeField] float mouseRadius;
    int maxCharges;
    float selector = 1;
    bool isEditMode;
    bool isDoingForce;
    bool playerInRadius;
    Vector2 dir = Vector2.zero;
    [Header("Text")]
    [SerializeField] TextController text;
    [TextArea]
    [SerializeField] string message;

    [Header("Mouse Visual Settings")]
    [SerializeField] Transform mouseTransform;
    [SerializeField] SpriteRenderer spr;
    [SerializeField] ParticleSystem [] particlesForce;
    [SerializeField] ParticleSystem switchParticle;
    [SerializeField] Color positiveColor;
    [SerializeField] Color negativeColor;
    [SerializeField] Color neutralColor;
    [Header("Mouse Audio Settings")]
    [SerializeField] AudioSource audioSwitch;
    [SerializeField] AudioSource audioForce;

    Rigidbody2D rb2dPlayer;
    Vector2 mousePos;
    private void Awake()
    {
        rb2dPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        maxCharges = numCharges;
        Cursor.visible = false;
        selector = 1;
        ChangeMouseColor();
    }
    private void Update()
    {
        mousePos = InputManager.singletone.GetMousePos();
        isDoingForce = InputManager.singletone.GetIsDoingForce();
        playerInRadius = Vector2.Distance(mousePos, rb2dPlayer.position) < mouseRadius;
        mouseTransform.position = mousePos;
        if (!PauseManager.singletone.GetPause())
        {
            ChangeMouseColor();
            DoForce();
            RotationWeapon();

            if (!isDoingForce)
            {
                PlayParticlesForce();
                audioForce.Stop();
            }
            else
            {
                if(!audioForce.isPlaying)
                audioForce.Play();
            }     
        }
    }
    public void ChargeSpawn(Vector3 posSpawn)
    {
        if(numCharges > 0 && numCharges <= maxCharges && !isDoingForce && !PauseManager.singletone.GetPause())
        {         
            SimpleCameraShakeInCinemachine.singletone.DoCameraShake();
            numCharges--;
            GameObject go = null;
            if (selector == 1)
            {
                go = Instantiate(positiveCharge, posSpawn, Quaternion.identity, null);              
            }
            else if (selector == -1)
            {
                go = Instantiate(negativeCharge, posSpawn, Quaternion.identity, null);             
            }
            Destroy(go, timeToDestroy);
            Invoke("AddCharge", timeToDestroy);
        }
        else
        {
            if(!isEditMode)
            text.SetText(message);
        }
       
    }
    void AddCharge()
    {
        numCharges++;
    }
    public void SetSelector(float _selector)
    {
        selector = _selector;
    }
    public void DoForce()
    {
        if (isDoingForce && playerInRadius)
        {
            dir = (rb2dPlayer.position - mousePos).normalized;
            rb2dPlayer.AddForceAtPosition(dir * mouseForce, rb2dPlayer.position, ForceMode2D.Force);
        }    
    }
    public void ChangeMouseColor()
    {
        if (isDoingForce)
        {
            spr.color = neutralColor;
        }
        else
        {
            if(selector == 1)
            {
                spr.color = positiveColor;
            }
            if(selector == -1)
            {
                spr.color = negativeColor;
            }
        }
    }
    public void RotationWeapon()
    {
        Vector2 dirRot = mousePos - rb2dPlayer.position;
        float rotationZ = Mathf.Atan2(dirRot.y, dirRot.x) * Mathf.Rad2Deg;
        mouseTransform.localRotation = Quaternion.Euler(0, 0, rotationZ + 90);
    }

    void PlayParticlesForce()
    {
        for(int i = 0; i< particlesForce.Length; i++)
        {
            particlesForce[i].Play();
        }
    }
    public void OnSwitch()
    {

        if (selector == 1)
        {
            switchParticle.startColor = negativeColor;
            audioSwitch.Play();
            switchParticle.Play();
        }
        if(selector == -1)
        {
            switchParticle.startColor = positiveColor;
            audioSwitch.Play();
            switchParticle.Play();
        }
       
       
    }
}
