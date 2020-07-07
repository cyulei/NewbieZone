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

    private int index;
    private void Start()
    {
        bulletType = WeaponType.Fire;
        index = 0;
    }
    protected override void HurtExtraEffect(GameObject other)
    {
        base.HurtExtraEffect(other);
        Instantiate(Effect, this.transform.position, Quaternion.identity);
        //   Debug.Log("持续燃烧");
        StartCoroutine(Fire(other));
    }
    private IEnumerator Fire(GameObject other)
    {
        if (index < fireTimes)
        {
            Debug.Log("index:" + index + "fireIntervalTime:" + fireIntervalTime);
            if (other == null)
            {
                base.isHitGameobject = true;
                base.endTime = 5f;
                base.isNeedToMove = true;
                yield return null;
            }
            base.AttackOther(other, fireHurt);
            index++;
            yield return new WaitForSeconds(fireIntervalTime);
            StartCoroutine(Fire(other));
        }
        else
        {
            base.isHitGameobject = true;
            base.endTime = 5f;
            base.isNeedToMove = true;
            yield return null;
        }
    }

    protected override void UpdateHitStatus()
    {
     //   base.UpdateHitStatus();
        base.isHitGameobject = false;
        base.endTime = 999f;
        base.isNeedToMove = false;
    }
}
