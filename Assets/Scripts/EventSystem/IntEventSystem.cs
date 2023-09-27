using System;
using System.Collections.Generic;
using UnityEngine;

public static class IntEventSystem
{
    private static readonly Dictionary<int, Action<object>> mAllListenerMap = new Dictionary<int, Action<object>>(50);

    public static void Register(int key, Action<object> fun)
    {
        if (mAllListenerMap.ContainsKey(key))
        {
            mAllListenerMap[key] += fun;
        }
        else
        {
            mAllListenerMap[key] = fun;
        }
    }

    public static void UnRegister(int key, Action<object> fun)
    {
        if (mAllListenerMap.ContainsKey(key))
        {
            mAllListenerMap[key] -= fun;
            if (mAllListenerMap[key] == null)
            {
                mAllListenerMap.Remove(key);
            }
        }
    }

    public static void Send(int key, object param = null)
    {
        if (mAllListenerMap.ContainsKey(key))
        {
            var action = mAllListenerMap[key];

            //委托异常不要影响之后代码执行
            try
            {
                action?.Invoke(param);
            }
            catch (Exception e)
            {
                // ignored
                Debug.LogError(e.Message + e.StackTrace);
            }
        }
    }
}
