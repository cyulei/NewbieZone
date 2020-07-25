using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 触发boss移动和血条显示
/// </summary>
public class BossWall : MonoBehaviour
{
    [Tooltip("boss物体")]
    public GameObject Boss;
    [Tooltip("boss血条")]
    public GameObject BossHealth;

    private BossAI _bossAI;
    private BoxCollider _myCollider;
    private Health _bossHealth;
    private void Start()
    {
        _bossAI = Boss.GetComponent<BossAI>();
        _bossAI.enabled = false;

        _bossHealth = Boss.GetComponent<Health>();
        _bossHealth.disable = true;

        _myCollider = GetComponent<BoxCollider>();

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            // boss 开始动
            _bossAI.enabled = true;
            // boss 血条显示
            BossHealth.SetActive(true);
        }
    }

    public void BossFightStart()
    {
        // 防止玩家退出区域
         _myCollider.isTrigger = false;
        _bossHealth.disable = false;
    }
}
