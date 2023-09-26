using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "EventLine", menuName = "Player/EventLineSO")]
public class EventLineSO : ScriptableObject
{
    [InspectorName("时间线")]
    public List<EventLineData> EventLines = new List<EventLineData>();
}

[Serializable]
public enum EventEnum
{
    /// <summary>
    /// 属性随机
    /// </summary>
    [InspectorName("属性随机")]
    P_Attribute,

    /// <summary>
    /// 准心刷新
    /// </summary>
    [InspectorName("准心刷新")]
    R_Target,

    /// <summary>
    /// 判定结束
    /// </summary>
    [InspectorName("判定结束")]
    Q_JudgeGame,
}

[Serializable]
public struct EventLineData
{
    /// <summary>
    /// 事件触发的时间点
    /// </summary>
    [InspectorName("触发时间点")]
    public float Time;

    /// <summary>
    /// 事件类型
    /// </summary>
    [InspectorName("事件类型")]
    public EventEnum Event;
}
