using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    private SpriteRenderer m_spriteRenderer;
    private Color m_defaultColor;
    private Transform m_progress;
    private SpriteRenderer m_progressSR;
    private Tween m_progressTween;
    private HashSet<PlayerController> m_playerControllers = new HashSet<PlayerController>();

    public PlayerController m_currentPlayer;
    //public PlayerData playerdata;

    private void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_defaultColor = m_spriteRenderer.color;
        m_progress = transform.Find("Progress");
        m_progressSR = m_progress.GetComponent<SpriteRenderer>();
        //playerdata = GetComponent<PlayerController>().PlayerData;
        
    }

    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var playerController = collision.GetComponent<PlayerController>();
        if (!m_playerControllers.Contains(playerController))
        {
            m_playerControllers.Add(playerController);
        }
        if (playerController != null)
        {
            SetOwner(playerController);
        }
    }

    /// <summary>
    /// 设置当前格子属于那个玩家
    /// </summary>
    /// <param name="playerController"></param>
    public void SetOwner(PlayerController playerController)
    {
        SetColor(playerController.PlayerColor);
        m_currentPlayer = playerController;
        SetProgress(1);
        SetProgressAnim(0,playerController);
        playerController.SetCurrentGridCell(this);
    }

    /// <summary>
    /// 直接设置格子当前的占领进度 没有动画
    /// </summary>
    /// <param name="progress"></param>
    private void SetProgress(float progress)
    {
        if (m_progressTween != null)
        {
            m_progressTween.Kill();
        }
        m_progress.transform.localScale = new Vector3(1, progress, 1); ;
    }

    /// <summary>
    /// 设置格子当前的占领进度 有动画 动画结束后 格子设置为默认格子
    /// </summary>
    /// <param name="progress"></param>
    private void SetProgressAnim(float progress,PlayerController playerController)
    {
        var targetScale = new Vector3(1, progress, 1);
        if (m_progressTween != null)
        {
            m_progressTween.Kill();
        }
        m_progressTween = m_progress.transform.DOScale(targetScale, playerController.PlayerData.CaptureTime);
        m_progressTween.OnComplete(() =>
        {
            m_currentPlayer = null;
            SetColor(m_defaultColor);
        });
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var playerController = collision.GetComponent<PlayerController>();
        if (m_playerControllers.Contains(playerController))
        {
            m_playerControllers.Remove(playerController);
        }
        RemoveCellForPlayer(playerController);
    }

    /// <summary>
    /// 设置格子颜色
    /// </summary>
    /// <param name="color"></param>
    private void SetColor(Color color)
    {
        m_spriteRenderer.color = color;
        m_progressSR.color = color * 0.8f;
    }

    /// <summary>
    /// 移除当前格子所属玩家
    /// </summary>
    /// <param name="playerController"></param>
    private void RemoveCellForPlayer(PlayerController playerController)
    {
        var currentCell = playerController.GetCurrenGridCell();
        if (currentCell == this)
        {
            playerController.SetCurrentGridCell(null);
        }
    }

    /// <summary>
    /// 获取当前格子的玩家
    /// </summary>
    /// <returns></returns>
    public PlayerController GetCurrentPlayerController()
    {
        return m_currentPlayer;
    }

    /// <summary>
    /// 重置
    /// </summary>
    public void Reset()
    {
        m_currentPlayer = null;
        SetColor(m_defaultColor);
        SetProgress(0);
    }

    private void OnDestroy()
    {
        if (m_progressTween != null)
        {
            m_progressTween.Kill();
        }
    }
}
