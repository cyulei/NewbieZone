using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
