using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : Bullet
{
    [Tooltip("燃烧附加伤害")]
    public int fireHurt;
    [Tooltip("持续燃烧几次")]
    public int fireTimes;
    [Tooltip("燃烧间隔")]
    public float fireIntervalTime;

    private AttackExtraEffectTool effectTool;

    private void Start()
    {
        bulletType = WeaponType.Fire;
        effectTool = Director.GetInstance().CurrentAttackExtraEffectTool;
    }
    protected override void HurtExtraEffect(GameObject other)
    {
        base.HurtExtraEffect(other);
        Instantiate(Effect, this.transform.position, Quaternion.identity);
        //   Debug.Log("持续燃烧");
        //StartCoroutine(Fire(other));
        // 生成持续效果
        effectTool.AddEffect(new FireEffect(3, 2, 0, effectTool.EffectID, other, 10));
    }


}
