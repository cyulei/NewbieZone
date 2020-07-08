using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : BTAction
{
    private string destinationDataName;    // 移动到的目的地在数据库中的key值

    private float speed;                   // 移动速度
    private Vector3 destination;           // 移动到的目的地位置
    private float tolerance = 0.2f;       // 移动到目标地址的容错率

    private Transform trans;               // 怪物的Transform

    /// <summary>
    /// 构造器
    /// </summary>
    /// <param name="destinationName">移动到的目的地在数据库中的key值</param>
    /// <param name="moveSpeed">移动速度</param>
    public MonsterMove(string destinationName, float moveSpeed)
    {
        destinationDataName = destinationName;
        speed = moveSpeed;
    }

    public override void Activate(Database database)
    {
        base.Activate(database);
        // database由MonsterAI挂在到物体上与MonsterAI同一transform都是怪兽的
        trans = database.transform;
    }

    /// <summary>
    /// 执行移动
    /// </summary>
    /// <returns></returns>
    protected override BTResult Execute()
    {
        UpdateDestination();
     //   UpdateFaceDirection();

        if (CheckArrived())
        {
            //Debug.Log("到达目的地");
            return BTResult.Ended;
        }
        MoveToDestination();
        return BTResult.Running;
    }

    /// <summary>
    /// 更新需要到达的目的地
    /// </summary>
    private void UpdateDestination()
    {
        destination = database.GetData<Vector3>(destinationDataName);
    }

    /// <summary>
    /// 检测是否到达目的地
    /// </summary>
    /// <returns></returns>
    private bool CheckArrived()
    {
        Vector3 offset = destination - trans.position;

        return offset.sqrMagnitude < tolerance * tolerance;
    }

    /// <summary>
    /// 移动到目的地
    /// </summary>
    private void MoveToDestination()
    {
        Vector3 direction = (destination - trans.position).normalized;
        trans.position += direction * speed;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}
