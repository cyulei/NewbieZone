using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect
{
    public bool isFirstEnter = true;       // 是第一次使用该特效
    public int _ID;                        // 额外效果的ID
    public int _times;                     // 实行几次
    public float _intervalTime;            // 每次的间隔时间
    public float _continueTime;            // 每次的持续时间

    public int currentTimes = 0;               // 当前执行的次数
    public float currentContinueTime = 0;      // 当前效果持续的时间
    public float currentIntervalTime = 0;      // 当前的间隔时间
     
    public GameObject _other;                  // 效果作用的对象

    public bool isInterval = false;            // 是否在间隔时间

    public AttackEffect(int times, float IntervalTime, float ContinueTime, int ID, GameObject other)
    {
        _times = times;
        _intervalTime = IntervalTime;
        _continueTime = ContinueTime;
        _ID = ID;
        _other = other;
    }

    /// <summary>
    /// 开始效果
    /// </summary>
    public virtual void StartEffect() { }

    /// <summary>
    /// 效果结束
    /// </summary>
    public virtual void EndEffect() { }
}

public class FireEffect : AttackEffect
{
    private int fireHurt;               // 火焰效果每次的伤害

    /// <summary>
    /// 火焰效果构造器
    /// </summary>
    /// <param name="times">执行次数</param>
    /// <param name="IntervalTime">每次执行的间隔时间</param>
    /// <param name="ContinueTime">每次效果的持续时间</param>
    /// <param name="ID">火焰效果的ID</param>
    /// <param name="other">效果作用对象</param>
    /// <param name="FireHurt">火焰效果每次的伤害</param>
    public FireEffect(int times, float IntervalTime, float ContinueTime, int ID, GameObject other, int FireHurt = 0) : base(times, IntervalTime, ContinueTime, ID, other)
    {
        fireHurt = FireHurt;
    }

    public override void StartEffect()
    {
        // 让对象掉血
        Director.GetInstance().CurrentHealthManagemer.AttackOtherObject(_other, fireHurt);
    }
}

public class FrozenEffect : AttackEffect
{
    float beforeMoveSpeed;          // 没有作用效果之前的移动速度
    float _slowRate;                // 让对象减慢速度的百分比

    AIForMonster monster;              // 只作用在怪兽身上

    public FrozenEffect(int times, float IntervalTime, float ContinueTime, int ID, GameObject other, float slowRate = 1.0f) : base(times, IntervalTime, ContinueTime, ID, other)
    {
        _slowRate = slowRate;
        if(other.gameObject.tag == "Monster")
            monster = other.GetComponent<MonsterAI>();
        else if(other.gameObject.tag == "Boss")
            monster = other.GetComponent<BossAI>();
        beforeMoveSpeed = monster.moveSpeed;
    }
    public override void StartEffect()
    {
        // 让怪物减速
        monster.ChangeMoveSpeed(beforeMoveSpeed * _slowRate);
    }
    public override void EndEffect()
    {
        // 结束减速的时候还原移动速度
        monster.ChangeMoveSpeed(beforeMoveSpeed);
    }
}