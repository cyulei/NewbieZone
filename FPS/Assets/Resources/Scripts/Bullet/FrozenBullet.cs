using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenBullet : Bullet
{
    private void Start()
    {
        bulletType = WeaponType.Frozen;
    }
    protected override void HurtMonster(GameObject monster)
    {
        base.HurtMonster(monster);
        Instantiate(Effect, this.transform.position, Quaternion.identity);
     //   Debug.Log("减缓速度");
    }
}
