using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFindToTargetDestination : BTAction
{
    private string playerName;            // 玩家的gameobject名称
    private string destinationDataName;   // 在数据库中距离的string
    private string playerLocationName;
    private Transform trans;              // 当前怪物的位置
     
    private float attackDistance;          // 攻击距离


    /// <summary>
    /// 到玩家位置应该走的目的地
    /// </summary>
    /// <param name="targetName">玩家的gameobject名称</param>
    /// <param name="playerLocation">在数据库中玩家当前位置的string</param>
    /// <param name="destinationName">在数据库中距离的string</param>
    /// <param name="minDistance">攻击距离</param>
    /// <param name="precondition">进入该状态的条件</param>
    public MonsterFindToTargetDestination(string targetName,string playerLocation ,string destinationName, float minDistance, BTPrecondition precondition = null) : base(precondition)
    {
        playerName = targetName;
        destinationDataName = destinationName;
        attackDistance = minDistance;
        playerLocationName = playerLocation;
    }

    public override void Activate(Database database)
    {
        base.Activate(database);
        trans = database.transform;
    }

    private Vector3 GetToTargetOffset()
    {
        GameObject target = GameObject.Find(playerName) as GameObject;
        if (target == null)
        {
            return Vector3.zero;
        }

        database.SetData<Vector3>(playerLocationName, target.transform.position);
        Debug.Log("玩家位置:" + target.transform.position);
        return target.transform.position - trans.position;
    }

    protected override BTResult Execute()
    {
        if (GameObject.Find(playerName) as GameObject == null)
        {
            return BTResult.Ended;
        }

        Vector3 offset = GetToTargetOffset();

        if (offset.sqrMagnitude >= attackDistance * attackDistance)
        {
            Vector3 direction = offset.normalized;
            Vector3 destination = trans.position + offset - attackDistance * direction;  // 走到攻击范围圈上即可
            database.SetData<Vector3>(destinationDataName, destination);
            Debug.Log("去攻击:" + destination);
            return BTResult.Running;
        }
        return BTResult.Ended;
    }
}
