using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRandomMoveDistance : BTAction
{
    private string destinationDataName;   // 在数据库中距离的string
    private float rangeX;
    private float rangeZ;

    private Transform trans;              // 当前怪物的位置

    private Vector3 oringinTrans;
    /// <summary>
    /// 随机选择目的地移动
    /// </summary>
    /// <param name="destinationName">距离的string值</param>
    /// <param name="rangeInX">坐标x值的范围</param>
    /// <param name="rangeInZ">坐标y值的范围</param>
    /// <param name="precondition">条件</param>
    public MonsterRandomMoveDistance(string destinationName, float rangeInX, float rangeInZ, BTPrecondition precondition = null) : base(precondition)
    {

        destinationDataName = destinationName;
        rangeX = rangeInX;
        rangeZ = rangeInZ;
    }

    public override void Activate(Database database)
    {
        base.Activate(database);
        trans = database.transform;
        oringinTrans = trans.position;
    }

    protected override BTResult Execute()
    {
        
        float x = Random.Range(-rangeX, rangeX);
        float z = Random.Range(-rangeZ, rangeZ);
        Vector3 destination = oringinTrans + new Vector3(x, 0, z); //dest world position
        database.SetData<Vector3>(destinationDataName, destination);
        //Debug.Log("随机移动:" + destination);
        return BTResult.Ended;
    }
}
