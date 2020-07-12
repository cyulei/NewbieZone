using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    // CameraShaker单例
    public static CameraShaker Instance { get; protected set; }

    float remainingShakeTime;    // 震动时间
    float shakeStrength;         // 震动强度
    Vector3 originalPosition;    // 初始位置

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
                // 获取一个随机的圆形范围的向量
                Vector3 randomDir = Random.insideUnitSphere;
                transform.localPosition = originalPosition + randomDir * shakeStrength;
            }
        }
    }

    /// <summary>
    /// 相机震动
    /// </summary>
    /// <param name="time">震动时长</param>
    /// <param name="strength">震动强度</param>
    public void Shake(float time, float strength)
    {
        shakeStrength = strength;
        remainingShakeTime = time;
    }
}
