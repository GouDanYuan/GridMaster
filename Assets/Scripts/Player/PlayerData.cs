using Sirenix.OdinInspector;
using System;

[Serializable]
public class PlayerData
{
    /// <summary>
    /// 速度
    /// </summary>
    [Title("速度")]
    [PropertyRange(0, 10)]
    public float Speed;

    /// <summary>
    /// 冷却时间
    /// </summary>
    [Title("冷却时间")]
    [PropertyRange(0.5f, 10)]
    public float CoolTime;

    /// <summary>
    /// 碰撞体缩放倍数
    /// </summary>
    [Title("碰撞体缩放倍数")]
    [PropertyRange(0.5f, 10)]
    public float ColliderScale;

    /// <summary>
    /// 冲刺距离
    /// </summary>
    [Title("冲刺距离")]
    [PropertyRange(0.1f, 10)]
    public float FlashDistance;

    /// <summary>
    /// 被击退距离
    /// </summary>
    [Title("被击退距离")]
    [PropertyRange(0.1f, 10)]
    public float PushDistance;

    /// <summary>
    /// 击退持续时间
    /// </summary>
    [Title("击退持续时间")]
    [PropertyRange(0.1f, 1f)]
    public float PushTime;

    /// <summary>
    /// 占领时间
    /// </summary>
    [Title("占领时间")]
    [PropertyRange(0.5f, 10f)]
    public float CaptureTime;

    /// <summary>
    /// 反弹次数
    /// </summary>
    [Title("反弹次数")]
    [PropertyRange(0, 10)]
    public int BounceCount;

    public override string ToString()
    {
        return $"Speed:{Speed} CoolTime:{CoolTime} ColliderScale:{ColliderScale}" +
            $" FlashDistance:{FlashDistance} PushDistance:{PushDistance} PushTime:{PushTime} CaptureTime:{CaptureTime}" +
            $"BounceCount:{BounceCount}";
    }
}
