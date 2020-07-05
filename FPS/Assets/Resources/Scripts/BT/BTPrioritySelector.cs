using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 选择器 先有序地遍历子节点根据条件来选择一个子结点
/// </summary>
public class BTPrioritySelector : BTNode
{
    private BTNode activeChild;   // 当前激活的孩子节点

    public BTPrioritySelector(BTPrecondition precondition = null) : base(precondition) { }
    
    /// <summary>
    /// 判断能否执行该节点
    /// </summary>
    /// <returns></returns>
    protected override bool DoEvaluate()
    {
        // 孩子节点中至少有一个可以执行
        foreach (BTNode child in children)
        {
            if (child.Evaluate())
            {
                if (activeChild != null && activeChild != child)
                {
                    activeChild.Clear();
                }
                activeChild = child;
                return true;
            }
        }

        // 否则不进行选择
        if (activeChild != null)
        {
            activeChild.Clear();
            activeChild = null;
        }
        return false;
    }

    public override void Clear()
    {
        if (activeChild != null)
        {
            activeChild.Clear();
            activeChild = null;
        }
    }

    /// <summary>
    /// 执行该选择器的子节点 并返回结果
    /// </summary>
    /// <returns>执行结果</returns>
    public override BTResult Tick()
    {
        if (activeChild == null)
        {
            return BTResult.Ended;
        }

        BTResult result = activeChild.Tick();
        if (result != BTResult.Running)
        {
            activeChild.Clear();
            activeChild = null;
        }
        return result;
    }
}
