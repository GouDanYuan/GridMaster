using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerInfo : MonoBehaviour
{
    public Image PlayerColor;
    public TextMeshProUGUI PlayerName;
    public TextMeshProUGUI Speed;
    public TextMeshProUGUI CoolTime;
    public TextMeshProUGUI ColliderScale;
    public TextMeshProUGUI FlashDistance;
    public TextMeshProUGUI PushTime;
    public TextMeshProUGUI CaptureTime;
    public TextMeshProUGUI BounceCount;

    private PlayerController m_playerController;

    public void SetPlayerController(PlayerController playerController)
    {
        m_playerController = playerController;
    }

    public void Refresh()
    {
        PlayerColor.color = m_playerController.PlayerColor;
        PlayerName.text = m_playerController.PlayerEnum.ToString();
        Speed.text = $"Speed:{m_playerController.PlayerData.Speed}";
        CoolTime.text = $"CoolTime:{m_playerController.PlayerData.CoolTime}";
        ColliderScale.text = $"ColliderScale:{m_playerController.PlayerData.ColliderScale}";
        FlashDistance.text = $"FlashDistance:{m_playerController.PlayerData.FlashDistance}";
        PushTime.text = $"PushTime:{m_playerController.PlayerData.PushTime}";
        CaptureTime.text = $"CaptureTime:{m_playerController.PlayerData.CaptureTime}";
        BounceCount.text = $"BounceCount:{m_playerController.PlayerData.BounceCount}";
    }
}
