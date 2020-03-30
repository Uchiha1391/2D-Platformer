using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Camera mainCamera;
    private float shakeAmount = 0;

    void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    public void Shake(float amount,float length)
    {
        shakeAmount = amount;
        InvokeRepeating("DoShake", 0, 0.01f);
        Invoke("StopShake",length);
    }



    void DoShake()
    {
        if(shakeAmount>0)
        {
            Vector3 camPosition = mainCamera.transform.position;

            float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
            float offsetY = Random.value * shakeAmount * 2 - shakeAmount;

            camPosition.x += offsetX;
            camPosition.y += offsetY;

            mainCamera.transform.position = camPosition;

        }
    }

    void StopShake()
    {
        CancelInvoke("DoShake");
        mainCamera.transform.localPosition = Vector3.zero;
    }
}
