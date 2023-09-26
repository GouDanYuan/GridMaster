using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public int GAME_MAXTIME = 10;

    private Button m_StartBtn;
    private Button m_Continue;
    private TextMeshProUGUI m_TimeTmp;
    private TextMeshProUGUI m_StartTmp;
    private float m_startTime;
    private int m_deltaTime;
    private List<PlayerController> m_playerControllers = new List<PlayerController>();
    private List<UIPlayerInfo> m_uiPlayerInfos;

    private void Start()
    {
        m_StartBtn = transform.Find("Start").GetComponent<Button>();
        m_Continue = transform.Find("Continue").GetComponent<Button>();
        m_TimeTmp = transform.Find("Time").GetComponent<TextMeshProUGUI>();
        m_StartTmp = m_StartBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        m_StartBtn.onClick.AddListener(OnStartBtnClick);
        m_Continue.onClick.AddListener(OnContinueBtnClick);

        m_Continue.gameObject.SetActive(false);
        m_StartBtn.gameObject.SetActive(true);

        foreach (PlayerEnum playerEnum in Enum.GetValues(typeof(PlayerEnum)))
        {
            PlayerController pCtrl = GameObject.Find(playerEnum.ToString()).GetComponent<PlayerController>();
            m_playerControllers.Add(pCtrl);
        }

        GameObject playerInfoGo = transform.Find("PlayerInfo/Info").gameObject;
        for (int i = 0; i < m_playerControllers.Count; i++)
        {
            UIPlayerInfo uIPlayerInfo;
            if (i == 0)
            {
                uIPlayerInfo = playerInfoGo.GetComponent<UIPlayerInfo>();
            }
            else
            {
                uIPlayerInfo = Instantiate(playerInfoGo, playerInfoGo.transform.parent).GetComponent<UIPlayerInfo>();
            }
            uIPlayerInfo.SetPlayerController(m_playerControllers[i]);
            uIPlayerInfo.Refresh();
        }
        Reset();
    }

    private void OnContinueBtnClick()
    {
        m_Continue.gameObject.SetActive(false);
        foreach (PlayerController player in m_playerControllers)
        {
            player.Resume();
        }
        m_startTime = Time.realtimeSinceStartup;
    }

    private void OnStartBtnClick()
    {
        m_StartBtn.gameObject.SetActive(false);
        foreach (PlayerController player in m_playerControllers)
        {
            player.Resume();
        }
        m_startTime = Time.realtimeSinceStartup;
    }

    private void Reset()
    {
        m_StartBtn.gameObject.SetActive(true);
        GridCell[] gridCells = GameObject.Find("Grid").GetComponentsInChildren<GridCell>();
        foreach (var cell in gridCells)
        {
            cell.Reset();
        }
        ResetPlayer();
        m_startTime = float.MaxValue;
    }

    private void ResetPlayer()
    {
        foreach (PlayerController player in m_playerControllers)
        {
            player.Pause();
            player.Reset();
        }
    }

    private void Update()
    {
        m_deltaTime = (int)(Time.realtimeSinceStartup - m_startTime);
        if (m_deltaTime < 0)
        {
            m_TimeTmp.text = "";
            return;
        }
        if (m_deltaTime > GAME_MAXTIME)
        {
            var winPCtrl = JudgeGameEnd();
            if (winPCtrl != null)
            {
                m_StartTmp.text = $"{winPCtrl.PlayerEnum} Win \nReset";
                Reset();
            }
            else
            {
                foreach (PlayerController player in m_playerControllers)
                {
                    player.Pause();
                }
                m_Continue.gameObject.SetActive(true);
            }
        }
        else
        {
            m_TimeTmp.text = m_deltaTime.ToString();
        }
    }

    Dictionary<PlayerController, int> playerScore = new Dictionary<PlayerController, int>();
    private PlayerController JudgeGameEnd()
    {
        playerScore.Clear();
        GridCell[] gridCells = GameObject.Find("Grid").GetComponentsInChildren<GridCell>();
        foreach (var cell in gridCells)
        {
            var pCtrl = cell.GetCurrentPlayerController();
            if (pCtrl != null)
            {
                if (!playerScore.ContainsKey(pCtrl))
                {
                    playerScore.Add(pCtrl, 0);
                }
                playerScore[pCtrl]++;
            }
        }
        if (playerScore.Count == 1)
        {
            return playerScore.First().Key;
        }
        return null;
    }
}
