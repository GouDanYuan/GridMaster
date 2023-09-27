using UnityEngine;

/// <summary>
/// 武器 攻击玩家判定 动画驱动
/// </summary>
public class Weapon : MonoBehaviour
{
    private float m_hitTime;
    private PlayerController m_playerController;

    private void Start()
    {
        m_playerController = transform.parent.parent.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var rigidbody2D = collider.GetComponent<Rigidbody2D>();
        float timeDelta = Time.realtimeSinceStartup - m_hitTime;
        if (rigidbody2D != null && timeDelta > 1)
        {
            var startPos = collider.bounds.ClosestPoint(this.transform.position);
            startPos = collider.transform.position - startPos;
            Vector2 moveDir = new Vector2(startPos.x, startPos.y);
            rigidbody2D.MovePosition(rigidbody2D.position + moveDir);
            m_hitTime = Time.realtimeSinceStartup;
            var attackTarget = collider.GetComponent<PlayerController>();
            m_playerController.AttackOther(attackTarget);
        }
    }
}
