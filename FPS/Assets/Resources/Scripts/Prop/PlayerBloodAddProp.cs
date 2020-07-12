using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBloodAddProp : MonoBehaviour
{
    public int AddBloodNumber;
    public AudioClip bloodPropClip;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Health health = other.gameObject.GetComponent<Health>();
            health.SetHealth(health.MyHealth + AddBloodNumber);
            // 播放音频
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.footAudioPlayer.PlayClip(bloodPropClip, 0.8f, 1.1f);
            Destroy(this.gameObject);
        }
    }
}
