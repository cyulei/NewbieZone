using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器类型
/// </summary>
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
    public List<WeaponType> weaponTypes = new List<WeaponType>();

    [HideInInspector]
    public WeaponType CurrentWeaponType { get; private set; }

    [Tooltip("切换武器的冷却时间")]
    public float switchWeaponCooldown;
    private bool canChangeWeapon = false;
    private int currentWeaponCounter = 0;

    [Header("玩家可以拥有的武器")]
    public List<Weapon> myWeapons = new List<Weapon>();
    [HideInInspector]
    public Weapon currentWeapon;

    void Start()
    {
        Director.GetInstance().CurrentWeaponsManager = this;
        CurrentWeaponType = weaponTypes[currentWeaponCounter];
        currentWeapon = myWeapons[currentWeaponCounter];
        switchWeaponCooldown = 0;
    }

    private void Update()
    {
        switchWeaponCooldown += 1 * Time.deltaTime;
        if (switchWeaponCooldown > 0.8f)
        {
            canChangeWeapon = true;
        }
        else
        {
            canChangeWeapon = false;
        }
    }

    /// <summary>
    /// 改变武器
    /// </summary>
    /// <param name="typeIndex">改变的武器索引</param>
    private void WeanponChange(int typeIndex)
    {
        CurrentWeaponType = weaponTypes[typeIndex];
        currentWeapon = myWeapons[typeIndex];

        // 显示当前武器
        myWeapons[typeIndex].gameObject.SetActive(true);
        myWeapons[typeIndex].Selected();

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
                // 还原冷却时间
                switchWeaponCooldown = 0;

                // 当前武器状态重置 隐藏
                myWeapons[currentWeaponCounter].ResetWeapon();
                myWeapons[currentWeaponCounter].gameObject.SetActive(false);

                currentWeaponCounter++;
                if (currentWeaponCounter > weaponTypes.Count - 1)
                {
                    currentWeaponCounter = 0;
                }
                // 改变武器
                WeanponChange(currentWeaponCounter);
            }
            if (axis > 0)
            {
                switchWeaponCooldown = 0;

                myWeapons[currentWeaponCounter].ResetWeapon();
                myWeapons[currentWeaponCounter].gameObject.SetActive(false);

                currentWeaponCounter--;
                if (currentWeaponCounter < 0)
                {
                    currentWeaponCounter = weaponTypes.Count - 1;
                }
                WeanponChange(currentWeaponCounter);
            }
        }
    }

    /// <summary>
    /// 触发更换弹夹事件
    /// </summary>
    /// <param name="currentBulletClipNumer">现在的子弹数量</param>
    /// <param name="singleBulletClipNumer">弹夹子弹数量</param>
    public void ChangeWeaponBulletClip(int currentBulletClipNumer, int singleBulletClipNumer)
    {
        WeaponBulletClipChange?.Invoke(currentBulletClipNumer, singleBulletClipNumer);
    }

    /// <summary>
    /// 切换武器委托
    /// </summary>
    /// <param name="typeIndex">武器序号</param>
    public delegate void ChangeWeaponTypeEvent(int typeIndex);
    public event ChangeWeaponTypeEvent WeaponTypeChange;

    /// <summary>
    /// 更换弹夹委托
    /// </summary>
    /// <param name="currentBulletClipNumer">现在的子弹数量</param>
    /// <param name="singleBulletClipNumer">弹夹子弹数量</param>
    public delegate void ChangeWeaponBulletClipEvent(int currentBulletClipNumer,int singleBulletClipNumer);
    public event ChangeWeaponBulletClipEvent WeaponBulletClipChange;
}
