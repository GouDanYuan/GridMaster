using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    private Button m_StartBtn;
    private Button m_Continue;
    private TextMeshProUGUI m_TimeTmp;
    private TextMeshProUGUI m_StartTmp;
    private float m_startTime;
    private int m_deltaTime;
    private List<PlayerController> m_playerControllers = new List<PlayerController>();
    public int GAME_MAXTIME = 10;

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

        PlayerController p1 = GameObject.Find("P1").GetComponent<PlayerController>();
        m_playerControllers.Add(p1);
        PlayerController p2 = GameObject.Find("P2").GetComponent<PlayerController>();
        m_playerControllers.Add(p2);
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
