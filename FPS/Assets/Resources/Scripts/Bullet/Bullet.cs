using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletOwner
{
    Monster,
    Player
}

public class Bullet : MonoBehaviour
{
    [HideInInspector]
    public BulletOwner bulletOwner;

    [Tooltip("子弹击中物体的特效")]
    public Transform Effect;
    [Tooltip("子弹移动的速度")]
    public float bulletMoveSpeed = 5f;
    [HideInInspector]
    public float startTime = 0f;
    [Tooltip("子弹存活时间")]
    public float endTime = 5f;

    [HideInInspector]
    public Vector3 DirectionTowards { get; set; }  // 子弹移动到的目的地

    bool isHitGameobject = false;                  // 是击中物体

    [HideInInspector]
    public WeaponType bulletType;                  // 子弹类型

    HealthManagemer healthManagemer;
    private void Start()
    {
        bulletType = WeaponType.Normal;
        healthManagemer = Director.GetInstance().CurrentHealthManagemer;
    }
    private void Update()
    {
        startTime += Time.deltaTime;
        Move();
        if(endTime - startTime < 0.001f || isHitGameobject)
        {
            startTime = 0f;
            isHitGameobject = false;
            Director.GetInstance().CurrentBulletFactory.FreeBullet(this.gameObject);
        }
    }

    protected virtual void Move()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, DirectionTowards, Time.deltaTime * bulletMoveSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(healthManagemer == null)
        {
            healthManagemer = Director.GetInstance().CurrentHealthManagemer;
        }
        if (bulletOwner == BulletOwner.Player && other.gameObject.tag == "Monster")
        {
            healthManagemer.AttackOtherObject(other.gameObject, 20);
         //   Debug.Log("打中怪兽了");
         //   HurtMonster(other.gameObject);
            isHitGameobject = true;
        }
        else if(bulletOwner == BulletOwner.Monster && other.gameObject.tag == "Player")
        {
            //Debug.Log("打中玩家啦");
            healthManagemer.AttackOtherObject(other.gameObject, 20);
            isHitGameobject = true;
        }
    }

    protected virtual void HurtMonster(GameObject monster)
    {
        Destroy(monster);
    }
}
