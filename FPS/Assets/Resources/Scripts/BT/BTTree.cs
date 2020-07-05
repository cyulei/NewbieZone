using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 行为树
public abstract class BTTree : MonoBehaviour
{
    protected BTNode root = null;      // 行为树根节点
     
    [HideInInspector]
    public Database database;         // 结点之间交流存储数据库

    [HideInInspector]
    public bool isRunning = true;     // 行为树是否正在运行

    public const string RESET = "Reset";  // 在数据库中重置的键值

    //private static int _resetId;

    void Awake()
    {
        Init();
        // 对根节点也设置同样的数据库
        root.Activate(database);
    }
    void Update()
    {
        if (!isRunning) return;

        if (database.GetData<bool>(RESET))
        {
            Reset();
            database.SetData<bool>(RESET, false);
        }

        // 对整个树从根节点开始检查和运行
        if (root.Evaluate())
        {
            root.Tick();
        }
    }

    /// <summary>
    /// 初始化，重置设置为false，检查是否有Database组件
    /// </summary>
    protected virtual void Init()
    {
        database = GetComponent<Database>();
        if (database == null)
        {
            database = gameObject.AddComponent<Database>();
        }

        database.SetData<bool>(RESET, false);
    }

    /// <summary>
    /// 重置根节点
    /// </summary>
    protected void Reset()
    {
        if (root != null)
        {
            root.Clear();
        }
    }
}
