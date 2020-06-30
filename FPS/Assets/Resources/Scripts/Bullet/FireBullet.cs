using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : Bullet
{
    private void Start()
    {
        bulletType = WeaponType.Fire;
    }
    protected override void HurtMonster(GameObject monster)
    {
        base.HurtMonster(monster);
        Instantiate(Effect, this.transform.position, Quaternion.identity);
     //   Debug.Log("持续燃烧");
    }
}
