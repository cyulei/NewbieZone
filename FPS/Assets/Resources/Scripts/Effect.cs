using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [Tooltip("粒子效果摧毁时间")]
    public float endTime = 1f;
    float startTime = 0f;

    void Update()
    {
        startTime += Time.deltaTime;
        if(endTime - startTime < 0.001)
        {
            //Debug.Log("摧毁特效");
            Destroy(this.gameObject);
        }
    }
}
