using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCheckPlayerInRangeAndRandomAttack : MonsterCheckPlayerInRange
{
    private bool isLongDistance;  // 是不是远程攻击

    private int random;           // 随机数
    private float nowTime;        // 当前时间
    private float randomTime;     // 下一次产生随机数的时间

    /// <summary>
    /// 判断攻击方式
    /// </summary>
    /// <param name="radius">怪兽攻击半径</param>
    /// <param name="target">目标名称</param>
    /// <param name="isLongdistance">是不是远程攻击</param>
    /// <param name="reverseSign">是否逆置</param>
    public MonsterCheckPlayerInRangeAndRandomAttack(float radius, string target, bool isLongdistance, bool reverseSign = false):base(radius, target,reverseSign)
    {
        isLongDistance = isLongdistance;
        random = Random.Range(0, 10);
        randomTime = 5f;
        nowTime = Time.time;
    }

    public override bool Check()
    {
        // 判断是否到随机时间
        if(Time.time - nowTime > randomTime)
        {
            random = Random.Range(0, 10);
            nowTime = Time.time;
        }
        GameObject target = GameObject.Find(targetName) as GameObject;
        if (target == null) return false;

        Vector3 offset = target.transform.position - trans.position;

        // 30%进行进程攻击 70%进行远程攻击
        if(random >= 3 && offset.sqrMagnitude <= searchRadius * searchRadius && isLongDistance)
        {
            //Debug.Log("远程攻击");
            return true;
        }
        if(random < 3 && offset.sqrMagnitude <= searchRadius * searchRadius && !isLongDistance)
        {
            //Debug.Log("近程攻击");
            return true;
        }
        return false;
    }
}
