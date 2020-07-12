using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private int _health;
    public int MyHealth { get { return _health; } }   // 物体的血量
    [Tooltip("物体最小血量")]
    public int minHealth = 0;
    [Tooltip("物体最大血量")]
    public int maxHealth = 100;


    private void Start()
    {
        _health = maxHealth;
    }

    public void SetHealth(int health)
    {
        //Debug.Log("血量变化");
        if (health > maxHealth) _health = maxHealth;
        else if (health < minHealth) _health = minHealth;
        else _health = health;

        if(_health == minHealth)
        {
            NeedToDeath?.Invoke();
        }
        MyHealthChange?.Invoke(_health);
    }

    /// <summary>
    /// 血量变化事件
    /// </summary>
    /// <param name="health">改变的血量</param>
    public delegate void HealthChange(int health);
    public event HealthChange MyHealthChange;

    /// <summary>
    /// 血量达到最少
    /// </summary>
    public delegate void Death();
    public event Death NeedToDeath;
}
