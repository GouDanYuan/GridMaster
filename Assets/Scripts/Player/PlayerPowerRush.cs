using UnityEngine;

public class PlayerPowerRush : MonoBehaviour
{

    public LayerMask wallLayer;
    public float chargeForce = 50f;
    public float chargeCooldown;
    public bool isRush = false;

    private Rigidbody2D rb;
    private PlayerData playerData;
    private Vector2 playerSize;
    private Vector2 dashDirection;
    private float dashDuration;
    private float currentDashTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerData = GetComponent<PlayerController>().PlayerData;
        CircleCollider2D playerCollider = GetComponent<CircleCollider2D>();

        float playerRadius = playerCollider.radius;
        float playerDiameter = playerRadius * 2.0f;
        playerSize = new Vector2(playerDiameter, playerDiameter);
    }

    private void FixedUpdate()
    {
        if (chargeCooldown > 0f)
        {
            chargeCooldown -= Time.deltaTime;
        }

        if (isRush)
        {
            rb.AddForce(dashDirection * chargeForce, ForceMode2D.Impulse);
            currentDashTime += Time.deltaTime;
            if (currentDashTime >= dashDuration || IsWallHit())
            {
                StopCharging();
            }
        }
    }

    public void StartCharging(Vector2 direction, Vector2 lookDirection)
    {
        if (chargeCooldown <= 0f)
        {
            chargeCooldown = playerData.CoolTime;
            isRush = true;
            dashDirection = direction.magnitude < 0.1f ? lookDirection.normalized : direction.normalized;
            float dashDistance = playerSize.magnitude * playerData.FlashDistance;
            dashDuration = (dashDistance / chargeForce) * 2;
            currentDashTime = 0f;
        }
    }

    void StopCharging()
    {
        isRush = false;
    }

    bool IsWallHit()
    {
        Vector2 rayStart = (Vector2)transform.position + dashDirection * (playerSize.magnitude / 2.0f);
        RaycastHit2D hit = Physics2D.Raycast(rayStart, dashDirection, 0.2f, wallLayer);
        return hit.collider != null;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isRush)
        {
            Rigidbody2D otherRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();

            if (otherRigidbody != null)
            {
                Debug.Log("qwe");
                Vector2 forceDirection = dashDirection;

                float knockbackForce = 200f; 
                otherRigidbody.AddForce(forceDirection * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }

}
