using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSearchPlayer : BTAction
{
    private float searchRadius;           // 搜寻玩家的范围
    private Transform trans;

    /// <summary>
    /// 怪物范围内搜索玩家
    /// </summary>
    /// <param name="radius">搜寻玩家的范围</param>
    /// <param name="precondition">进行该行为的条件</param>
    public MonsterSearchPlayer(float radius, BTPrecondition precondition = null) : base(precondition)
    {
        searchRadius = radius;
    }

    public override void Activate(Database database)
    {
        base.Activate(database);
        trans = database.transform;
    }

    protected override BTResult Execute()
    {
        Collider[] hits = Physics.OverlapSphere(trans.position, searchRadius);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.CompareTag("Player"))
            {
                //mTarget = hits[i].transform; 有目标
                Debug.Log("发现目标");
            }
            break;
        }

        return BTResult.Ended;
    }

}
