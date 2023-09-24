using UnityEngine;

public class Weapon : MonoBehaviour
{
    private float m_hitTime;

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
        }
    }
}
