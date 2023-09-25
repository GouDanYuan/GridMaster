using UnityEngine;
using Sirenix.OdinInspector;
using System;

[CreateAssetMenu(fileName = "Player", menuName = "Player/Create Player")]
public class PlayerSO : ScriptableObject
{
    [Title("初始数据")]
    public PlayerData InitData;
}
