using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 阻止玩家退出Boss区域的墙
/// </summary>
public class BossFightWall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            FindObjectOfType<BossWall>().BossFightStart();
        }
    }
}
