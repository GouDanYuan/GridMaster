using UnityEngine;

public class Weapon : MonoBehaviour
{
    private float m_hitTime;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var rigidbody2D = collision.GetComponent<Rigidbody2D>();
        float timeDelta = Time.realtimeSinceStartup - m_hitTime;
        if (rigidbody2D != null && timeDelta > 1)
        {
            var startPos = collision.bounds.ClosestPoint(this.transform.position);
            rigidbody2D.AddForceAtPosition((collision.transform.position - startPos).normalized * 10f, startPos, ForceMode2D.Impulse);
            m_hitTime = Time.realtimeSinceStartup;
            rigidbody2D.velocity = Vector2.zero;
        }
    }
}
