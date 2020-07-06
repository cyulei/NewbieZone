using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private int _health;
    public int MyHealth { get { return _health; } }
    public int minHealth = 0;
    public int maxHealth = 100;


    private void Start()
    {
        _health = maxHealth;
    }

    public void SetHealth(int health)
    {
        if (health > maxHealth) _health = minHealth;
        else if (health < minHealth) _health = minHealth;
        else _health = health;

        if(_health == minHealth)
        {
            NeedToDeath?.Invoke();
        }
        MyHealthChange?.Invoke(_health);
    }

    public delegate void HealthChange(int health);
    public event HealthChange MyHealthChange;

    public delegate void Death();
    public event Death NeedToDeath;
}
