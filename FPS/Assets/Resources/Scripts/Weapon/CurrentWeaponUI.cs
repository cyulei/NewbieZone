using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentWeaponUI : MonoBehaviour
{
    [Tooltip("当前武器UI图片")]
    public Image weaponImage;
    WeaponsManager weaponsManager;

    public Text BulletClipText;
    // Start is called before the first frame update
    void Start()
    {
        // 监听武器改变事件
        weaponsManager = Director.GetInstance().CurrentWeaponsManager;
        weaponsManager.WeaponTypeChange += WeaponChange;
        weaponsManager.WeaponBulletClipChange += WeaponClipChange;
        weaponImage.sprite = weaponsManager.bulletSprites[0];
    }

    // 改变当前武器UI图片
    void WeaponChange(int typeIndex)
    {
        weaponImage.sprite = weaponsManager.bulletSprites[typeIndex];
    }

    void WeaponClipChange(int currentBulletClipNumer, int singleBulletClipNumer)
    {
        BulletClipText.text = currentBulletClipNumer + " | " + singleBulletClipNumer;
    }
}
