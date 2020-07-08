using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackExtraEffectTool : MonoBehaviour
{
    private int ID = 0;
    public int EffectID { get { return ID++; } }
    private List<AttackEffect> attackEffects = new List<AttackEffect>();
    private void Start()
    {
        ID = 0;
        Director.GetInstance().CurrentAttackExtraEffectTool = this;
    }

    private void Update()
    {
        for (int i = 0; i < attackEffects.Count; i++)
        {
            // 判断效果是否开始
            if(attackEffects[i].currentTimes < attackEffects[i]._times)
            {
                if(attackEffects[i].isInterval)
                {
                    if(attackEffects[i].currentIntervalTime < attackEffects[i]._IntervalTime)
                    {
                        attackEffects[i].currentIntervalTime += Time.deltaTime;
                        continue;
                    }
                    else
                    {
                        attackEffects[i].StartEffect();
                        attackEffects[i].isInterval = false;
                        attackEffects[i].currentIntervalTime = 0;
                        continue;
                    }
                }
                else
                {
                    if(attackEffects[i].isFirstEnter)
                    {
                        attackEffects[i].StartEffect();
                        attackEffects[i].isFirstEnter = false;
                        continue;
                    }
                    if (attackEffects[i].currentContinueTime < attackEffects[i]._ContinueTime)
                    {
                        attackEffects[i].currentContinueTime += Time.deltaTime;
                        continue;
                    }
                    else
                    {
                        attackEffects[i].currentTimes++;
                        attackEffects[i].EndEffect();
                        attackEffects[i].isInterval = true;
                        attackEffects[i].currentContinueTime = 0;
                        continue;
                    }
                }
            }
            else
            {
                RemoveEffect(attackEffects[i]._ID);
            }
            // 判断效果是否该移除
        }
    }

    public void AddEffect(AttackEffect attackEffect)
    {
        attackEffects.Add(attackEffect);
    }

    private void RemoveEffect(int id)
    {
        for(int i=0;i<attackEffects.Count;i++)
        {
            if(attackEffects[i]._ID == id)
            {
                attackEffects.Remove(attackEffects[i]);
                break;
            }
        }
    }
}

public class AttackEffect
{
    public bool isFirstEnter = true;
    public int _ID;
    public int _times;           // 实行几次
    public float _IntervalTime;  // 每次的间隔时间
    public float _ContinueTime;  // 每次的持续时间

    public int currentTimes = 0;
    public float currentContinueTime = 0;
    public float currentIntervalTime = 0;

    public GameObject _other;

    public bool isInterval = false;
    public AttackEffect(int times,float IntervalTime,float ContinueTime,int ID, GameObject other)
    {
        _times = times;
        _IntervalTime = IntervalTime;
        _ContinueTime = ContinueTime;
        _ID = ID;
        _other = other;
    }

    public virtual void StartEffect() { }
    public virtual void EndEffect() { }
}

public class FireEffect : AttackEffect
{
    private int fireHurt;

    public FireEffect(int times, float IntervalTime, float ContinueTime, int ID, GameObject other,int FireHurt = 0):base(times, IntervalTime, ContinueTime,ID,other)
    {
        fireHurt = FireHurt;
    }
    public override void StartEffect()
    {
        //Debug.Log("让对象掉血");
        // 让对象掉血
        Director.GetInstance().CurrentHealthManagemer.AttackOtherObject(_other, fireHurt);
    }
}

public class FrozenEffect : AttackEffect
{
    float beforeMoveSpeed;
    float _slowRate;

    MonsterAI monster;
    public FrozenEffect(int times, float IntervalTime, float ContinueTime, int ID, GameObject other, float slowRate = 1.0f) : base(times, IntervalTime, ContinueTime, ID, other)
    {
        _slowRate = slowRate;
        monster = other.GetComponent<MonsterAI>();
        beforeMoveSpeed = monster.moveSpeed;
    }
    public override void StartEffect()
    {
        //Debug.Log("减速");
        monster.ChangeMoveSpeed(beforeMoveSpeed * _slowRate);
    }
    public override void EndEffect()
    {
        //Debug.Log("结束减速");
        monster.ChangeMoveSpeed(beforeMoveSpeed);
    }
}