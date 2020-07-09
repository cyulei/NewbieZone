using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : Bullet
{
    [Header("燃烧子弹属性")]
    [Tooltip("燃烧附加伤害")]
    public int fireHurt;
    [Tooltip("持续燃烧几次")]
    public int fireTimes;
    [Tooltip("燃烧间隔")]
    public float fireIntervalTime;

    // 额外效果的管理器
    private AttackExtraEffectTool effectTool;

    private void Start()
    {
        bulletType = WeaponType.Fire;
        effectTool = Director.GetInstance().CurrentAttackExtraEffectTool;
    }
    protected override void HurtExtraEffect(GameObject other)
    {
        base.HurtExtraEffect(other);
        // 生成粒子效果
        Instantiate(Effect, this.transform.position, Quaternion.identity);
        // 配置额外效果
        effectTool.AddEffect(new FireEffect(3, 2, 0, effectTool.EffectID, other, 10));
    }


}
