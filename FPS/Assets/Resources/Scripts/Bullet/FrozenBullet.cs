using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenBullet : Bullet
{
    private AttackExtraEffectTool effectTool;

    private void Start()
    {
        bulletType = WeaponType.Frozen;
        effectTool = Director.GetInstance().CurrentAttackExtraEffectTool;
    }
    protected override void HurtExtraEffect(GameObject monster)
    {
        base.HurtExtraEffect(monster);
        Instantiate(Effect, this.transform.position, Quaternion.identity);
        //   Debug.Log("减缓速度");
        effectTool.AddEffect(new FrozenEffect(1, 0, 5, effectTool.EffectID, monster, 0.1f));
    }
}
