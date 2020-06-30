using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentWeaponUI : MonoBehaviour
{
    [Tooltip("当前武器UI图片")]
    public Image weaponImage;
    WeaponsManager weaponsManager;
    // Start is called before the first frame update
    void Start()
    {
        // 监听武器改变事件
        weaponsManager = Director.GetInstance().CurrentWeaponsManager;
        weaponsManager.WeaponTypeChange += WeaponChange;
    }

    // 改变当前武器UI图片
    void WeaponChange(WeaponType weaponType)
    {
        switch(weaponType)
        {
            case WeaponType.Fire:
                weaponImage.sprite = weaponsManager.FireBulletSprite;
                break;
            case WeaponType.Frozen:
                weaponImage.sprite = weaponsManager.FrozenBulletSprite;
                break;
            default:
                weaponImage.sprite = weaponsManager.NormalBulletSprite;
                break;
        }
    }
}
