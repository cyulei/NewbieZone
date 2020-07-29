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
    public BulletOwner bulletOwner;  // 子弹所属阵营

    [Header("子弹属性")]
    [Tooltip("子弹击中物体的特效")]
    public Transform Effect;
    [Tooltip("子弹移动的速度")]
    public float bulletMoveSpeed = 5f;
    [HideInInspector]
    public float startTime = 0f;
    [Tooltip("子弹存活时间")]
    public float endTime = 5f;
    [Tooltip("子弹的普通伤害")]
    public int hurt = 10;

    [HideInInspector]
    public Vector3 DirectionTowards { get; set; }  // 子弹移动到的目的地

    protected bool isHitGameobject = false;         // 是击中物体

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
            //Debug.Log("回收:" + endTime + "isHitGameobject:" + isHitGameobject);
            startTime = 0f;
            isHitGameobject = false;
            Director.GetInstance().CurrentBulletFactory.FreeBullet(this.gameObject);
        }
    }

    /// <summary>
    /// 子弹移动
    /// </summary>
    protected virtual void Move()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, DirectionTowards, Time.deltaTime * bulletMoveSpeed);
    }

    /// <summary>
    /// 击中了对象
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if(healthManagemer == null)
        {
            healthManagemer = Director.GetInstance().CurrentHealthManagemer;
        }
        // 如果是怪物并且是player的子弹
        if (bulletOwner == BulletOwner.Player && (other.gameObject.tag == "Monster" || other.gameObject.tag == "Boss"))
        {
            AttackOther(other.gameObject, hurt);         // 伤害
            HurtExtraEffect(other.gameObject);           // 除了伤害以外的其他效果
            isHitGameobject = true;
        }
        else if(bulletOwner == BulletOwner.Monster && other.gameObject.tag == "Player")
        {
            // 如果击中玩家，并且是怪物的子弹
            AttackOther(other.gameObject, hurt);         // 伤害
            Singleton<PlayerHurtUI>.Instance.MonsterAttack();
            HurtExtraEffect(other.gameObject);           // 除了伤害以外的其他效果
            isHitGameobject = true;
        }
    }

    /// <summary>
    /// 除了掉血以外的其他效果
    /// </summary>
    /// <param name="other">受击对象</param>
    protected virtual void HurtExtraEffect(GameObject other)
    {
        if(bulletType == WeaponType.Normal)
        {
            // 生成粒子效果
            Instantiate(Effect, this.transform.position, Quaternion.identity);
        }
    }

    /// <summary>
    /// 对对象造成伤害
    /// </summary>
    /// <param name="gameObject">受到伤害的对象</param>
    /// <param name="hurt">伤害</param>
    protected void AttackOther(GameObject gameObject,int hurt)
    {
        healthManagemer.AttackOtherObject(gameObject, hurt);
    }

}
