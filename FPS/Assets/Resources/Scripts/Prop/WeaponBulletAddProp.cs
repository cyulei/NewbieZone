using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 增加武器子弹道具
/// </summary>
public class WeaponBulletAddProp : MonoBehaviour
{
    [Tooltip("子弹类型")]
    public WeaponType type;
    [Tooltip("增加的子弹数量")]
    public int addBulletNumer;
    [Tooltip("被拾取的音效")]
    public AudioClip bulletAddPropClip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // 增加子弹
            WeaponsManager weaponsManager = Director.GetInstance().CurrentWeaponsManager;
            for (int i = 0; i < weaponsManager.weaponTypes.Count; i++)
            {
                if (weaponsManager.weaponTypes[i] == type)
                {
                    weaponsManager.myWeapons[i].totalBulletNumber += addBulletNumer;
                    FindObjectOfType<WeaponBulletAdd>().ShowUI(i, addBulletNumer);
                }
            }
            // 播放音频
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.footAudioPlayer.PlayClip(bulletAddPropClip, 0.8f, 1.1f);

            // 销毁自身
            Destroy(this.gameObject);
        }
    }


}
