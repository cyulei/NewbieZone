using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 行为树的结果
public enum BTResult
{
    Ended = 1,
    Running = 2,
}

public abstract class BTNode
{
    protected List<BTNode> children;   // 节点上的子节点们
    public List<BTNode> Children { get { return children; } }

    public BTPrecondition precondition;  // 进入该节点的条件

    public Database database;            // 公用的存储数据的数据库
    public bool activated;               // 数据库已激活

    // 行为的执行的冷却时间
    public float interval = 0.05f;
    private float lastTimeEvaluated = 0;  // 上次执行时间点

    public BTNode() : this(null) { }

    public BTNode(BTPrecondition precondition)
    {
        this.precondition = precondition;
    }

    /// <summary>
    /// 设置结点的共有数据库
    /// </summary>
    /// <param name="database"></param>
    public virtual void Activate(Database database)
    {
        if (activated) return;

        this.database = database;

        if (precondition != null)
        {
            precondition.Activate(database);
        }
        if (children != null)
        {
            foreach (BTNode child in children)
            {
                child.Activate(database);
            }
        }

        activated = true;
    }

    /// <summary>
    /// 判断节点行为是否执行
    /// </summary>
    /// <returns></returns>
    public bool Evaluate()
    {
        bool coolDownOK = CheckTimer();

        return activated && coolDownOK && (precondition == null || precondition.Check()) && DoEvaluate();
    }

    // 检查冷却时间是否到了
    private bool CheckTimer()
    {
        if (Time.time - lastTimeEvaluated > interval)
        {
            lastTimeEvaluated = Time.time;
            return true;
        }
        return false;
    }

    protected virtual bool DoEvaluate() { return true; }

    public virtual BTResult Tick() { return BTResult.Ended; }

    public virtual void Clear() { }

    public virtual void AddChild(BTNode aNode)
    {
        if (children == null)
        {
            children = new List<BTNode>();
        }
        if (aNode != null)
        {
            children.Add(aNode);
        }
    }

    public virtual void RemoveChild(BTNode aNode)
    {
        if (children != null && aNode != null)
        {
            children.Remove(aNode);
        }
    }
}
