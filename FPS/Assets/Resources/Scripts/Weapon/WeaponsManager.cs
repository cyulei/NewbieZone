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
    public List<Sprite> bulletSprites = new List<Sprite>();
    [Tooltip("玩家可以拥有的武器类型")]
    public List<WeaponType> weapons = new List<WeaponType>();

    [HideInInspector]
    public WeaponType CurrentWeaponType { get; private set; }

    [Tooltip("切换武器的冷却时间")]
    public float switchWeaponCooldown;
    private bool canChangeWeapon = false;
    private int currentWeaponCounter = 0;
    void Start()
    {
        Director.GetInstance().CurrentWeaponsManager = this;
        CurrentWeaponType = weapons[currentWeaponCounter];
        switchWeaponCooldown = 0;
    }

    private void Update()
    {
        switchWeaponCooldown += 1 * Time.deltaTime;
        if (switchWeaponCooldown > 1.2f)
        {
            canChangeWeapon = true;
        }
        else
        {
            canChangeWeapon = false;
        }
    }

    private void WeanponChange(int typeIndex)
    {
        CurrentWeaponType = weapons[typeIndex];
        WeaponTypeChange?.Invoke(typeIndex);
    }

    /// <summary>
    /// 通过键盘切换武器
    /// </summary>
    /// <param name="keyborad">键盘数字键</param>
    public void ChangeWeaponTypeByKeyborad(int keyborad)
    {
        if(canChangeWeapon)
        {
            switchWeaponCooldown = 0;
            WeanponChange(keyborad - 1);
        }
    }

    /// <summary>
    /// 通过鼠标切换武器
    /// </summary>
    /// <param name="axis"></param>
    public void ChangeWeaponTypeByMouse(float axis)
    {
        if (canChangeWeapon)
        {
            if (axis < 0)
            {
                switchWeaponCooldown = 0;

                currentWeaponCounter++;
                if (currentWeaponCounter > weapons.Count - 1)
                {
                    currentWeaponCounter = 0;
                }
                WeanponChange(currentWeaponCounter);
            }
            if (axis > 0)
            {
                switchWeaponCooldown = 0;

                currentWeaponCounter--;
                if (currentWeaponCounter < 0)
                {
                    currentWeaponCounter = weapons.Count - 1;
                }
                WeanponChange(currentWeaponCounter);
            }
        }
    }

    /// <summary>
    /// 切换武器委托
    /// </summary>
    /// <param name="typeIndex">武器序号</param>
    public delegate void ChangeWeaponTypeEvent(int typeIndex);
    public event ChangeWeaponTypeEvent WeaponTypeChange;
}
