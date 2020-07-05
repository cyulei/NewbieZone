using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : BTAction
{
    protected float ATK;                     // 攻击力
    protected Transform trans;               // 怪物的Transform
    public MonsterAttack(int atk,BTPrecondition precondition = null) : base(precondition)
    {
        ATK = atk;
    }
    public override void Activate(Database database)
    {
        base.Activate(database);
        // database由MonsterAI挂在到物体上与MonsterAI同一transform都是怪兽的
        trans = database.transform;
    }
}

public class MonsterMeleeAttack : MonsterAttack
{
    public MonsterMeleeAttack(int atk, BTPrecondition precondition = null) : base(atk , precondition)
    {

    }

    // TODO:
    protected override BTResult Execute()
    {
        // 玩家掉血 TODO:
        // 播放动画
        Debug.Log("正在攻击玩家");
        return BTResult.Ended;
    }
}

public class MonsterLongDistanceAttacks : MonsterAttack
{
    // 再次攻击冷却时间
    private float colddown = 2f;
    private float lastTimeEvaluated = 0;  // 上次执行时间点


    public MonsterLongDistanceAttacks(int atk, BTPrecondition precondition = null) : base(atk, precondition)
    {

    }

    public override void Activate(Database database)
    {
        base.Activate(database);
        lastTimeEvaluated = Time.time;
    }

    protected override BTResult Execute()
    {
        if(Time.time - lastTimeEvaluated > colddown)
        {
            // 发射子弹
            GameObject bullet = Director.GetInstance().CurrentBulletFactory.GetBullet(trans, BulletOwner.Monster);
            lastTimeEvaluated = Time.time;
            return BTResult.Ended;
        }
        return BTResult.Running;
    }
}