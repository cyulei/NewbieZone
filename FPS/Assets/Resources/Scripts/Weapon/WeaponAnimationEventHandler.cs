using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器动画触发的事件
/// </summary>
public class WeaponAnimationEventHandler : MonoBehaviour
{
    Weapon weapon;

    void Awake()
    {
        weapon = GetComponentInParent<Weapon>();
    }

    /// <summary>
    /// 走路动画 触发声音
    /// </summary>
    public void PlayFootstep()
    {
        weapon.player.PlayFootstep();
    }

    /// <summary>
    /// 所有动画播放完毕回到默认状态
    /// </summary>
    public void ReturnToIdle()
    {
        weapon.ChangeState(WeaponState.Idle);
    }
}
