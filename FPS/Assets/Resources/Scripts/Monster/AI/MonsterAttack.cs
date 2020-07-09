﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : BTAction
{
    protected float ATK;                     // 攻击力
    protected Transform trans;               // 怪物的Transform
    protected bool isFirstEnter;             // 是否是第一次攻击

    public MonsterAttack(int atk,BTPrecondition precondition = null) : base(precondition)
    {
        ATK = atk;
        isFirstEnter = true;
    }
    public override void Activate(Database database)
    {
        base.Activate(database);
        // database由MonsterAI挂在到物体上与MonsterAI同一transform都是怪兽的
        trans = database.transform;
    }
}

/// <summary>
/// 怪兽进程攻击
/// </summary>
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

/// <summary>
/// 怪兽远程攻击
/// </summary>
public class MonsterLongDistanceAttacks : MonsterAttack
{
    // 再次发射子弹冷却时间
    private float colddown = 2f;
    private float lastTimeEvaluated;  // 上次执行时间点


    public MonsterLongDistanceAttacks(int atk, BTPrecondition precondition = null) : base(atk, precondition)
    {

    }

    public override void Activate(Database database)
    {
        base.Activate(database);
    }

    protected override BTResult Execute()
    {
        if(isFirstEnter)
        {
            // 发射子弹
            GameObject bullet = Director.GetInstance().CurrentBulletFactory.GetBullet(trans, BulletOwner.Monster);
            lastTimeEvaluated = Time.time;
            isFirstEnter = false;
        }

        // 间隔几秒后再次发射子弹
        if(Time.time - lastTimeEvaluated > colddown && !isFirstEnter)
        {
            // 发射子弹
            GameObject bullet = Director.GetInstance().CurrentBulletFactory.GetBullet(trans, BulletOwner.Monster);
            lastTimeEvaluated = Time.time;
            return BTResult.Ended;
        }
        return BTResult.Running;
    }
}