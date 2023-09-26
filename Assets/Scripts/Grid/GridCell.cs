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
            SetColor(playerController.PlayerColor);
            m_currentPlayer = playerController;
            SetProgress(1);
            SetProgressAnim(0);
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
            m_currentPlayer = null;
            SetColor(m_defaultColor);
        });
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
    }

    private void SetColor(Color color)
    {
        m_spriteRenderer.color = color;
        m_progressSR.color = color * 0.8f;
    }

    public PlayerController GetCurrentPlayerController()
    {
        return m_currentPlayer;
    }

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
