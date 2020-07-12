using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBulletAddProp : MonoBehaviour
{
    public WeaponType type;
    public int addBulletNumer;
    public AudioClip bulletAddPropClip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            WeaponsManager weaponsManager = Director.GetInstance().CurrentWeaponsManager;
            for (int i = 0; i < weaponsManager.weaponTypes.Count; i++)
            {
                if (weaponsManager.weaponTypes[i] == type)
                {
                    weaponsManager.myWeapons[i].totalBulletNumber += addBulletNumer;
                }
            }
            // 播放音频
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.footAudioPlayer.PlayClip(bulletAddPropClip, 0.8f, 1.1f);
            Destroy(this.gameObject);
        }
    }
}
