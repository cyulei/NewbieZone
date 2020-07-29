using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 
/// 同时执行各个子结点 每当任一子结点的准入条件失败就不会执行。
/// </summary>
public class BTParallel : BTNode
{
    protected List<BTResult> results;
    protected ParallelFunction func;

    public BTParallel(ParallelFunction func) : this(func, null) { }

    public BTParallel(ParallelFunction func, BTPrecondition precondition) : base(precondition)
    {
        results = new List<BTResult>();
        this.func = func;
    }

    /// <summary>
    /// 检测所有子节点能不能执行
    /// </summary>
    /// <returns></returns>
    protected override bool DoEvaluate()
    {
        foreach (BTNode child in children)
        {
            if (!child.Evaluate())
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 所有子节点都开始执行
    /// </summary>
    /// <returns></returns>
    public override BTResult Tick()
    {
        int endingResultCount = 0;

        for (int i = 0; i < children.Count; i++)
        {

            if (func == ParallelFunction.And)
            {
                if (results[i] == BTResult.Running)
                {
                    results[i] = children[i].Tick();
                }
                if (results[i] != BTResult.Running)
                {
                    endingResultCount++;
                }
            }
            else
            {
                if (results[i] == BTResult.Running)
                {
                    results[i] = children[i].Tick();
                }
                if (results[i] != BTResult.Running)
                {
                    ResetResults();
                    return BTResult.Ended;
                }
            }
        }
        if (endingResultCount == children.Count)
        {   
            ResetResults();
            return BTResult.Ended;
        }
        return BTResult.Running;
    }

    public override void Clear()
    {
        ResetResults();

        foreach (BTNode child in children)
        {
            child.Clear();
        }
    }

    /// <summary>
    /// 添加一个孩子节点
    /// </summary>
    /// <param name="aNode"></param>
    public override void AddChild(BTNode aNode)
    {
        base.AddChild(aNode);
        results.Add(BTResult.Running);
    }

    /// <summary>
    /// 删除一个孩子节点
    /// </summary>
    /// <param name="aNode"></param>
    public override void RemoveChild(BTNode aNode)
    {
        int index = children.IndexOf(aNode);
        results.RemoveAt(index);
        base.RemoveChild(aNode);
    }

    
    /// <summary>
    /// 将所有节点都设置为在运行
    /// </summary>
    private void ResetResults()
    {
        for (int i = 0; i < results.Count; i++)
        {
            results[i] = BTResult.Running;
        }
    }

    public enum ParallelFunction
    {
        And = 1,    // 当所有结果都不运行返回Ended
        Or = 2,     // 当任意一个结果不运行返回Ended
    }
}
