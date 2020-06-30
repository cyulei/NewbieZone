using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Normal,
    Frozen,
    Fire
}

public class WeaponsManager : MonoBehaviour
{
    [Tooltip("武器UI图片")]
    public Sprite NormalBulletSprite;
    public Sprite FrozenBulletSprite;
    public Sprite FireBulletSprite;

    [HideInInspector]
    public WeaponType CurrentWeaponType { get; private set; }

    void Start()
    {
        Director.GetInstance().CurrentWeaponsManager = this;
        CurrentWeaponType = WeaponType.Normal;
    }

    public void ChangeWeaponType(WeaponType newWeaponType)
    {
        CurrentWeaponType = newWeaponType;
        WeaponTypeChange?.Invoke(newWeaponType);
    }

    public delegate void ChangeWeaponTypeEvent(WeaponType newWeaponType);
    public event ChangeWeaponTypeEvent WeaponTypeChange;
}
