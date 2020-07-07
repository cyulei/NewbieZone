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

    protected bool isHitGameobject = false;                  // 是击中物体
    protected bool isNeedToMove = true;
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
        if(isNeedToMove)
            Move();

        if(endTime - startTime < 0.001f || isHitGameobject)
        {
            Debug.Log("回收:" + endTime + "isHitGameobject:" + isHitGameobject);
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
            AttackOther(other.gameObject, 0);
         //   Debug.Log("打中怪兽了");
            HurtExtraEffect(other.gameObject);
            UpdateHitStatus();
        }
        else if(bulletOwner == BulletOwner.Monster && other.gameObject.tag == "Player")
        {
            //Debug.Log("打中玩家啦");
            AttackOther(other.gameObject, 0);
            HurtExtraEffect(other.gameObject);
            UpdateHitStatus();
        }
    }

    protected virtual void HurtExtraEffect(GameObject monster) { }

    public void AttackOther(GameObject gameObject,int hurt)
    {
        healthManagemer.AttackOtherObject(gameObject, hurt);
    }

    protected virtual void UpdateHitStatus() { isHitGameobject = true; }
}
