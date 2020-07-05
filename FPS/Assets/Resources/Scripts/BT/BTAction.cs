using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 行为节点 具体的游戏逻辑
/// </summary>
public class BTAction : BTNode
{
    private enum BTActionStatus
    {
        Ready = 1,              // 默认的行为状态 
        Running = 2,            // 行为正在运行
    }

    private BTActionStatus status = BTActionStatus.Ready;

    public BTAction(BTPrecondition precondition = null) : base(precondition) { }

    /// <summary>
    /// 行为开始实行前
    /// </summary>
    protected virtual void Enter()
    {
        // 用于Debug
    }

    /// <summary>
    /// 行为刚推出实行
    /// </summary>
    protected virtual void Exit()
    {
        // 用于Debug
    }

    /// <summary>
    /// 行为执行逻辑
    /// </summary>
    /// <returns></returns>
    protected virtual BTResult Execute()
    {
        return BTResult.Running;
    }

    public override void Clear()
    {
        if (status != BTActionStatus.Ready)
        {   
            // 还未清除
            Exit();
            status = BTActionStatus.Ready;
        }
    }

    public override BTResult Tick()
    {
        BTResult result = BTResult.Ended;
        if (status == BTActionStatus.Ready)
        {
            Enter();   
            status = BTActionStatus.Running;
        }

        if (status == BTActionStatus.Running)
        {      
            result = Execute();
            if (result != BTResult.Running)
            {
                Exit();
                // 返回默认状态
                status = BTActionStatus.Ready;
            }
        }
        return result;
    }

    // 行为节点不能拥有子节点
    public override void AddChild(BTNode aNode)
    {
        Debug.LogError("BTAction: Cannot add a node into BTAction.");
    }
    // 行为节点不能拥有子节点
    public override void RemoveChild(BTNode aNode)
    {
        Debug.LogError("BTAction: Cannot remove a node into BTAction.");
    }
}
