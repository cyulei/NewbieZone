using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRotateToTarget : BTAction
{
    protected Transform trans;               // 怪物的Transform
    protected string locationName;
    private float rotateTime = 1.5f;
    private float lastTimeEvaluated = 0;  // 上次执行时间点

    /// <summary>
    /// 怪物朝目的地转向
    /// </summary>
    /// <param name="targetName">目的地的string值</param>
    /// <param name="precondition">执行条件</param>
    public MonsterRotateToTarget( string targetName, BTPrecondition precondition = null) : base(precondition)
    {
        locationName = targetName;
    }
    public override void Activate(Database database)
    {
        base.Activate(database);
        trans = database.transform;
        lastTimeEvaluated = Time.time;
    }

    protected override BTResult Execute()
    {
        Vector3 playerPosition = database.GetData<Vector3>(locationName);
        Vector3 offset = playerPosition - trans.position;
        Quaternion rot = Quaternion.LookRotation(offset);
        //trans.localEulerAngles = new Vector3(rot.eulerAngles.x, rot.eulerAngles.y, rot.eulerAngles.z);
        //return BTResult.Ended;
        trans.localEulerAngles = Vector3.Lerp(trans.localEulerAngles, new Vector3(rot.eulerAngles.x, rot.eulerAngles.y, rot.eulerAngles.z), (Time.time - lastTimeEvaluated) /rotateTime);
        if (Time.time - lastTimeEvaluated > rotateTime)
        {
            Debug.Log("旋转结束");
            lastTimeEvaluated = Time.time;
            return BTResult.Ended;
        }
        return BTResult.Running;
    }

}
