using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour
{
    // 一个List模拟的字典序容器 database中 使用dataNames对应索引的string值作为key值(需转换为int型)，可以获取对应位置的value
    // database和dataNames中值一一对应，索引相同
    private List<object> database = new List<object>();
    private List<string> dataNames = new List<string>();

    /// <summary>
    /// 查找数据通过string
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="dataName">数据名称</param>
    /// <returns></returns>
    public T GetData<T>(string dataName)
    {
        int dataId = IndexOfDataId(dataName);
        if (dataId == -1) Debug.LogError("Database: Data for " + dataName + " does not exist!");

        return (T)database[dataId];
    }

    /// <summary>
    /// 设置数据的值
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="dataName">所设置的string类型key</param>
    /// <param name="data">所设置的值</param>
    public void SetData<T>(string dataName, T data)
    {
        int dataId = GetDataId(dataName);
        database[dataId] = (object)data;
    }

    /// <summary>
    /// 获得数据ID
    /// </summary>
    /// <param name="dataName">所设置的string类型key</param>
    /// <returns></returns>
    public int GetDataId(string dataName)
    {
        int dataId = IndexOfDataId(dataName);
        // 未查找到则在两个List中加入新的
        if (dataId == -1)
        {
            dataNames.Add(dataName);
            database.Add(null);
            dataId = dataNames.Count - 1;
        }

        return dataId;
    }

    /// <summary>
    /// 查找对应string
    /// </summary>
    /// <param name="dataName">所设置的string类型key</param>
    /// <returns></returns>
    private int IndexOfDataId(string dataName)
    {
        for (int i = 0; i < dataNames.Count; i++)
        {
            if (dataNames[i].Equals(dataName)) return i;
        }

        return -1;
    }
}
