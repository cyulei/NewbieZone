using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCheckPlayerInRangeAndRandomAttack : MonsterCheckPlayerInRange
{
    private bool isLongDistance;

    int random;
    float nowTime;
    float randomTime;
    public MonsterCheckPlayerInRangeAndRandomAttack(float radius, string target, bool isLongdistance, bool reverseSign = false):base(radius, target,reverseSign)
    {
        isLongDistance = isLongdistance;
        random = Random.Range(0, 10);
        randomTime = 10f;
        nowTime = Time.time;
    }
    public override bool Check()
    {
        if(Time.time - nowTime > randomTime)
        {
            random = Random.Range(0, 10);
            nowTime = Time.time;
        }
        GameObject target = GameObject.Find(targetName) as GameObject;
        if (target == null) return false;

        Vector3 offset = target.transform.position - trans.position;

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
