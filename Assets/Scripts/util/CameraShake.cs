﻿using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    public Transform camTransform;
    
    // How long the object should shake for.
    public float shake = 0f;
    
    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;
    
    Vector3 originalPos;
    
    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }
    
    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    public void play(float amount, float time)
    {
        if (shakeAmount > 0) {
            shakeAmount += amount / 2;
            shake += time/4;
        } else {
            shakeAmount = amount;
            shake = time;
        }
    }
    
    void Update()
    {
        if (shake > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
            
            shake -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shake = 0f;
            shakeAmount = 0f;
            camTransform.localPosition = originalPos;
        }
    }
}