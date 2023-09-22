using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    private HashSet<PlayerController> m_playerSets = new HashSet<PlayerController>();
    private SpriteRenderer m_spriteRenderer;
    private Color m_defaultColor;
    private Transform m_progress;
    private SpriteRenderer m_progressSR;
    private Tween m_progressTween;
    public PlayerController m_settledPlayer;
    public PlayerController m_currentPlayer;

    private void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_defaultColor = m_spriteRenderer.color;
        m_progress = transform.Find("Progress");
        m_progressSR = m_progress.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var playerController = collision.GetComponent<PlayerController>();
        if (playerController != null)
        {
            if (!m_playerSets.Contains(playerController))
            {
                m_playerSets.Add(playerController);
            }
            if (m_playerSets.Count == 1)
            {
                SetColor(playerController.PlayerColor);
                m_currentPlayer = playerController;
                SetProgressAnim(1);
            }
            else
            {
                SetColor(GetDefaultColor());
                SetProgress(0);
                m_currentPlayer = null;
            }
        }
    }

    private void SetProgress(float progress)
    {
        if (m_progressTween != null)
        {
            m_progressTween.Kill();
        }
        m_progress.transform.localScale = new Vector3(1, progress, 1); ;
    }

    private void SetProgressAnim(float progress)
    {
        var targetScale = new Vector3(1, progress, 1);
        if (m_progressTween != null)
        {
            m_progressTween.Kill();
        }
        m_progressTween = m_progress.transform.DOScale(targetScale, 3);
        m_progressTween.OnComplete(() =>
        {
            if (m_currentPlayer != null)
            {
                m_settledPlayer = m_currentPlayer;
            }
        });
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var playerController = collision.GetComponent<PlayerController>();
        if (playerController != null)
        {
            if (m_playerSets.Contains(playerController))
            {
                m_playerSets.Remove(playerController);
            }
            if (m_playerSets.Count == 1)
            {
                SetColor(playerController.PlayerColor);
                m_currentPlayer = playerController;
                SetProgress(0);
                SetProgressAnim(1);
            }
            else
            {
                SetColor(GetDefaultColor());
                SetProgress(0);
                m_currentPlayer = null;
            }
        }
    }

    private void SetColor(Color color)
    {
        m_spriteRenderer.color = color;
        m_progressSR.color = color * 0.8f;
    }

    private Color GetDefaultColor()
    {
        if (m_settledPlayer)
        {
            return m_settledPlayer.PlayerColor;
        }
        return m_defaultColor;
    }

    public void Reset()
    {
        m_settledPlayer = null;
        m_currentPlayer = null;
        m_playerSets.Clear();
        SetColor(m_defaultColor);
        SetProgress(0);
    }
}
