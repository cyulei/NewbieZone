using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackExtraEffectTool : MonoBehaviour
{
    // 用于设置额外效果的ID
    private int ID = 0;
    public int EffectID { get { return ID++; } }

    // 需要执行的效果的容器
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
            // 判断效果是否到达次数
            if(attackEffects[i].currentTimes < attackEffects[i]._times)
            {
                // 如果效果在间隔期间
                if(attackEffects[i].isInterval)
                {
                    if(attackEffects[i].currentIntervalTime < attackEffects[i]._intervalTime)
                    {
                        // 间隔时间未结束继续计时
                        attackEffects[i].currentIntervalTime += Time.deltaTime;
                        continue;
                    }
                    else
                    {
                        // 间隔时间结束开始执行效果
                        attackEffects[i].StartEffect();
                        attackEffects[i].isInterval = false;
                        attackEffects[i].currentIntervalTime = 0;
                        continue;
                    }
                }
                else
                {
                    // 如果是第一次执行效果 一开始不是间隔时间
                    if(attackEffects[i].isFirstEnter)
                    {
                        attackEffects[i].StartEffect();       
                        attackEffects[i].isFirstEnter = false;
                        continue;
                    }
                    // 如果持续时间未结束
                    if (attackEffects[i].currentContinueTime < attackEffects[i]._continueTime)
                    {
                        // 继续增加持续时间
                        attackEffects[i].currentContinueTime += Time.deltaTime;
                        continue;
                    }
                    else
                    {
                        // 到达持续时间
                        attackEffects[i].currentTimes++;                 // 增加当前执行的次数
                        attackEffects[i].EndEffect();                    // 结束效果
                        attackEffects[i].isInterval = true;              // 进入间隔期
                        attackEffects[i].currentContinueTime = 0;
                        continue;
                    }
                }
            }
            else
            {
                // 如果到达该执行的次数 移除效果
                RemoveEffect(attackEffects[i]._ID);       
            }
        }
    }

    /// <summary>
    /// 向容器中添加效果
    /// </summary>
    /// <param name="attackEffect">添加对象</param>
    public void AddEffect(AttackEffect attackEffect)
    {
        attackEffects.Add(attackEffect);
    }

    /// <summary>
    /// 容器中移除效果
    /// </summary>
    /// <param name="id">效果的id</param>
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