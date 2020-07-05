﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : MonoBehaviour
{
    private Bullet bullet = null;                        
    private List<Bullet> used = new List<Bullet>(10);            //正在被使用的子弹列表
    private List<Bullet> free = new List<Bullet>(10);            //空闲的子弹列表

    private void Start()
    {
        Director.GetInstance().CurrentBulletFactory = this;
    }

    public GameObject GetBullet(Transform shootingTransform, BulletOwner bulletOwner, WeaponType weaponType = WeaponType.Normal)
    {
        bullet = null;

     //   Debug.Log("free的数量" + free.Count);
        for (int i = 0; i < free.Count; i++)
        {
            if (weaponType == free[i].bulletType)
            {
                bullet = free[i];
                free.Remove(free[i]);
                break;
            }
        }

        if (bullet == null)
        {
            if (weaponType == WeaponType.Normal)
                bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Bullet/NomalBullet"), shootingTransform.position, Quaternion.identity).GetComponent<Bullet>();
            else if(weaponType == WeaponType.Fire)
                bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Bullet/FireBullet"), shootingTransform.position, Quaternion.identity).GetComponent<FireBullet>() as Bullet;
            else if(weaponType == WeaponType.Frozen)
                bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Bullet/FrozenBullet"), shootingTransform.position, Quaternion.identity).GetComponent<FrozenBullet>() as Bullet;
        }

        bullet.transform.position = shootingTransform.position;
        bullet.bulletOwner = bulletOwner;
        bullet.GetComponent<Bullet>().DirectionTowards = shootingTransform.forward * 1000 - shootingTransform.position;
        //添加到使用列表中
        used.Add(bullet.GetComponent<Bullet>());

        bullet.gameObject.SetActive(true);
        return bullet.gameObject;
    }

    public void FreeBullet(GameObject bullet)
    {
  //      Debug.Log("used数量:" + used.Count);
        for (int i = 0; i < used.Count; i++)
        {
            if (bullet.GetInstanceID() == used[i].gameObject.GetInstanceID())
            {
                //  Debug.Log("设置失活");
                used[i].gameObject.SetActive(false);
                free.Add(used[i]);
                used.Remove(used[i]);
                break;
            }
        }
    }
}
