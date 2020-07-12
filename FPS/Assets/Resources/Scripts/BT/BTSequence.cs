using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 依此执行每一个子节点
/// </summary>
public class BTSequence : BTNode
{
    private BTNode activeChild;   // 当前正在执行的节点
    private int activeIndex = -1; // 当前正在执行的节点的索引


    public BTSequence(BTPrecondition precondition = null) : base(precondition) { }

    protected override bool DoEvaluate()
    {
        if (activeChild != null)
        {
            bool result = activeChild.Evaluate();
            // 节点之间与关系 一个不能执行则后面不执行
            if (!result)
            {
                activeChild.Clear();
                activeChild = null;
                activeIndex = -1;
            }
            return result;
        }
        else
        {
            //Debug.Log(children[0]);
            return children[0].Evaluate();
        }
    }

    public override BTResult Tick()
    {
        // 第一次选择第一个节点执行
        if (activeChild == null)
        {
            activeChild = children[0];
            activeIndex = 0;
        }

        BTResult result = activeChild.Tick();
        if (result == BTResult.Ended)
        {  
            // 如果当前节点执行完毕执行下一个节点
            activeIndex++;
            if (activeIndex >= children.Count)
            {   
                // 整个序列执行完毕
                activeChild.Clear();
                activeChild = null;
                activeIndex = -1;
            }
            else
            {   
                // 执行下一个节点
                activeChild.Clear();
                activeChild = children[activeIndex];
                result = BTResult.Running;
            }
        }
        return result;
    }

    public override void Clear()
    {
        if (activeChild != null)
        {
            activeChild = null;
            activeIndex = -1;
        }

        foreach (BTNode child in children)
        {
            child.Clear();
        }
    }
}
