using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 行为树节点的进入条件
public abstract class BTPrecondition : BTNode
{
    public BTPrecondition() : base() { }

    public abstract bool Check();

    public override BTResult Tick()
    {
        bool success = Check();
        if (success)
        {
            return BTResult.Ended;
        }
        else
        {
            return BTResult.Running;
        }
    }
}
