using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWait : BTAction
{
    float waitTime = 2f;
    float lastTimeEvaluated = 0f;

    public MonsterWait(BTPrecondition precondition = null) : base(precondition){ }

    protected override void Enter()
    {
        //Debug.Log("静止");
        lastTimeEvaluated = Time.time;
    }
    /// <summary>
    /// 进行等待 等待时间过后结束
    /// </summary>
    /// <returns></returns>
    protected override BTResult Execute()
    {
        if (Time.time - lastTimeEvaluated > waitTime)
        {
            return BTResult.Ended;
        }
        return BTResult.Running;
    }
}
