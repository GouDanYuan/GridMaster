using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Reflection;

[CreateAssetMenu(fileName = "Player", menuName = "Player/Create Player")]
public class PlayerSO : ScriptableObject
{
    [Title("初始数据")]
    [SerializeField]
    private PlayerData data;

    public PlayerData InitData
    {
        get
        {
            PlayerData data = GenPlayerData();
            Debug.Log(data.ToString());
            return data;
        }
    }

    public PlayerData GenPlayerData()
    {
        Type thisType = data.GetType();
        var listTypes = thisType.GetFields(BindingFlags.Public | BindingFlags.Instance);
        PlayerData playerData = new PlayerData();
        var listPlayerTypes = playerData.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (var item in listTypes)
        {
            foreach (var playerItem in listPlayerTypes)
            {
                if (item.Name == playerItem.Name)
                {
                    playerItem.SetValue(playerData, item.GetValue(data));
                }
            }
        }
        return playerData;
    }
}
