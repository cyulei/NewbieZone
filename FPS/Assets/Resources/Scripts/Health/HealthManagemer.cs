using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManagemer : MonoBehaviour
{
    private void Start()
    {
        Director.GetInstance().CurrentHealthManagemer = this;
    }

    /// <summary>
    /// 攻击其他物体
    /// </summary>
    /// <param name="other">攻击的物体</param>
    /// <param name="ATK">攻击力</param>
    public void AttackOtherObject(GameObject other,int ATK)
    {
        if(other == null)
        {
            return;
        }
        Health health = other.GetComponent<Health>();
        if(health == null)
        {
            Debug.LogError("该对象上没有血量组件");
        }
        health.SetHealth(health.MyHealth - ATK);
    }
}
