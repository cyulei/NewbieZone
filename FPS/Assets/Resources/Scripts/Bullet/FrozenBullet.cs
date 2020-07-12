using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 冰冻子弹:可以让敌人减速
/// </summary>
public class FrozenBullet : Bullet
{
    private AttackExtraEffectTool effectTool;        // 攻击造成附加效果的工具
    public float continueTime;
    public float slowRate;
    private void Start()
    {
        // 设置子弹类型
        bulletType = WeaponType.Frozen;
        effectTool = Director.GetInstance().CurrentAttackExtraEffectTool;
    }
    protected override void HurtExtraEffect(GameObject monster)
    {
        base.HurtExtraEffect(monster);

        // 产生粒子效果
        Instantiate(Effect, this.transform.position, Quaternion.identity);
        //   Debug.Log("减缓速度");
        // 产生附加效果
        effectTool.AddEffect(new FrozenEffect(1, 0, continueTime, effectTool.EffectID, monster, slowRate));
    }
}
