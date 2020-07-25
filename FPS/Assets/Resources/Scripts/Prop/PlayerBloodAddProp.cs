using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 为玩家加血的物件
/// </summary>
public class PlayerBloodAddProp : MonoBehaviour
{
    [Tooltip("加血的数量")]
    public int AddBloodNumber;
    [Tooltip("被拾取音效")]
    public AudioClip bloodPropClip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // 加血
            Health health = other.gameObject.GetComponent<Health>();
            health.SetHealth(health.MyHealth + AddBloodNumber);

            FindObjectOfType<HPAdd>().ShowUI(AddBloodNumber);
            // 播放音频
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.footAudioPlayer.PlayClip(bloodPropClip, 0.8f, 1.1f);
            // 销毁自身
            Destroy(this.gameObject);
        }
    }
}
