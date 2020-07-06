using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManagemer : MonoBehaviour
{
    private void Start()
    {
        Director.GetInstance().CurrentHealthManagemer = this;
    }
    public void AttackOtherObject(GameObject other,int ATK)
    {
        Health health = other.GetComponent<Health>();
        if(health == null)
        {
            Debug.LogError("该对象上没有血量组件");
        }
        health.SetHealth(health.MyHealth - ATK);
    }
}
