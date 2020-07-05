using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCheckPlayerInRange : BTPrecondition
{
    private float searchRadius;
    private string targetName;

    private Transform trans;
    private bool isReverse;

    /// <summary>
    /// 条件：检查范围内是否有玩家
    /// </summary>
    /// <param name="radius">搜寻半径</param>
    /// <param name="target">搜寻目标</param>
    /// <param name="reverseSign">是否逆置条件</param>
    public MonsterCheckPlayerInRange(float radius, string target,bool reverseSign = false)
    {
        searchRadius = radius;
        targetName = target;
        isReverse = reverseSign;
    }

    public override void Activate(Database database)
    {
        base.Activate(database);

        trans = database.transform;
    }

    public override bool Check()
    {
        GameObject target = GameObject.Find(targetName) as GameObject;
        if (target == null) return false;

        Vector3 offset = target.transform.position - trans.position;
        if (!isReverse)
            return offset.sqrMagnitude <= searchRadius * searchRadius;
        else
            return !(offset.sqrMagnitude <= searchRadius * searchRadius);
    }
}
