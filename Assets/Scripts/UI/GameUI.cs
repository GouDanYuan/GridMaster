using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    private Button m_StartBtn;
    private TextMeshProUGUI m_TimeTmp;
    private float m_startTime;
    private int m_deltaTime;
    private List<PlayerController> m_playerControllers = new List<PlayerController>();

    private void Start()
    {
        m_StartBtn = transform.Find("Start").GetComponent<Button>();
        m_TimeTmp = transform.Find("Time").GetComponent<TextMeshProUGUI>();

        m_StartBtn.onClick.AddListener(OnStartBtnClick);

        m_StartBtn.gameObject.SetActive(true);

        PlayerController p1 = GameObject.Find("P1").GetComponent<PlayerController>();
        m_playerControllers.Add(p1);
        PlayerController p2 = GameObject.Find("P2").GetComponent<PlayerController>();
        m_playerControllers.Add(p2);
        Reset();
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
        if (m_deltaTime > 30)
        {
            Reset();
        }
        else
        {
            m_TimeTmp.text = m_deltaTime.ToString();
        }
    }
}
