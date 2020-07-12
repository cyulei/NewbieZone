using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public static CameraShaker Instance { get; protected set; }

    float remainingShakeTime;
    float shakeStrength;
    Vector3 originalPosition;

    void Awake()
    {
        Instance = this;
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        if (remainingShakeTime > 0)
        {
            remainingShakeTime -= Time.deltaTime;

            if (remainingShakeTime <= 0)
            {
                transform.localPosition = originalPosition;
            }
            else
            {
                Vector3 randomDir = Random.insideUnitSphere;
                transform.localPosition = originalPosition + randomDir * shakeStrength;
            }
        }
    }

    public void Shake(float time, float strength)
    {
        shakeStrength = strength;
        remainingShakeTime = time;
    }
}
