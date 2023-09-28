using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public int GAME_MAXTIME = 10;
    public int p1Score;
    public int p2Score;
    private Button m_StartBtn;
    private Button m_Continue;
    private TextMeshProUGUI m_TimeTmp;
    private TextMeshProUGUI m_StartTmp;
    private float m_startTime;
    public int m_deltaTime;
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
            GameObject pObj = GameObject.Find(playerEnum.ToString());
            if (pObj != null)
            {
                PlayerController pCtrl = pObj.GetComponent<PlayerController>();
                m_playerControllers.Add(pCtrl);
            }
        }

        // UI玩家信息显示
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

    /// <summary>
    /// 判定失败后 继续游戏
    /// </summary>
    private void OnContinueBtnClick()
    {
        m_Continue.gameObject.SetActive(false);
        foreach (PlayerController player in m_playerControllers)
        {
            player.Resume();
        }
        m_startTime = Time.realtimeSinceStartup;
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    private void OnStartBtnClick()
    {
        m_StartBtn.gameObject.SetActive(false);
        foreach (PlayerController player in m_playerControllers)
        {
            player.Resume();
        }
        m_startTime = Time.realtimeSinceStartup;
    }

    /// <summary>
    /// 重置所有
    /// </summary>
    private void Reset()
    {
        m_StartBtn.gameObject.SetActive(true);
        GridCell[] gridCells = GameObject.Find("Grid").GetComponentsInChildren<GridCell>();
        foreach (var cell in gridCells)
        {
            cell.Reset();
        }

        p1Score = 0;
        p2Score = 0;
        ResetPlayer();
        m_startTime = float.MaxValue;
    }

    /// <summary>
    /// 重置所有玩家
    /// </summary>
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
        //var x = JudgeGameEnd();       //这也是查看实时分数用的，也不用管
        if (m_deltaTime > GAME_MAXTIME)
        {
            var winPCtrl = JudgeGameEnd();
            if ((winPCtrl!=null)&&(winPCtrl.Count() == 1))  //先判断是否为空，不然会报错
            {
                m_StartTmp.text = $"{winPCtrl.First().PlayerEnum} Win \nReset";
                Reset();
            }
            else
            {
                foreach (PlayerController player in m_playerControllers)
                {
                    player.Pause();
                }
                Reset();
                m_Continue.gameObject.SetActive(true);
            }
        }
        else
        {
            m_TimeTmp.text = m_deltaTime.ToString();
        }
    }

    public Dictionary<PlayerController, int> playerScore = new Dictionary<PlayerController, int>();
    /// <summary>
    /// 判定游戏是否结束
    /// </summary>
    /// <returns></returns>
    public IEnumerable<PlayerController> JudgeGameEnd()
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
                //这两个if我查看分数用的，不用管
                if (pCtrl.PlayerEnum == PlayerEnum.P1)
                {
                    p1Score = playerScore[pCtrl];
                }

                if (pCtrl.PlayerEnum == PlayerEnum.P2)
                {
                    p2Score = playerScore[pCtrl];
                }
            }
        }
       
        if (playerScore.Count>=1)
        {
            var maxScoreValue = playerScore.Values.Max();
            var maxScorePlayerControllers = playerScore.Where(x => x.Value == maxScoreValue).Select(x => x.Key);
            return maxScorePlayerControllers;
        }
        return null;
        
    }
}
