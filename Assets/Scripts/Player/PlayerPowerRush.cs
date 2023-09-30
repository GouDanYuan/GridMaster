using UnityEngine;

public enum MoveStatus
{
    Move,
    Rush,
    Stun,
    StunWall
}

public class PlayerPowerRush : MonoBehaviour
{

    public LayerMask wallLayer;
    public MoveStatus currentMoveStatus = MoveStatus.Move;
    public float chargeForce = 20f;
    public float chargeCooldown;

    private PlayerData playerData;
    private Vector2 playerSize;
    private Vector2 dashDirection;
    private float dashDuration;
    private float currentDashTime = 0f;
    private int bounceCount;
    private int currentBounceCount;

    void Start()
    {
        CircleCollider2D playerCollider = GetComponent<CircleCollider2D>();
        playerData = GetComponent<PlayerController>().PlayerData;

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

        switch (currentMoveStatus)
        {
            case MoveStatus.Move:
                break;
            case MoveStatus.Rush:
                Rush(chargeForce, dashDirection);
                currentDashTime += Time.deltaTime;
                if (currentDashTime >= dashDuration || IsWallHit())
                {
                    StopCharging();
                }
                break;
            case MoveStatus.Stun:
                Rush(chargeForce, dashDirection);
                currentDashTime += Time.deltaTime;
                if (currentDashTime >= dashDuration)
                {
                    StopStun();
                    return;
                }
                if (IsWallHit())
                {
                    dashDirection = -dashDirection;
                    StartStunWall();
                }
                break;
            case MoveStatus.StunWall:
                Rush(chargeForce, dashDirection);
                break;
        }
    }

    public void StartCharging(Vector2 direction, Vector2 lookDirection)
    {
        if (chargeCooldown <= 0f)
        {
            chargeCooldown = playerData.CoolTime;
            currentMoveStatus = MoveStatus.Rush;
            dashDirection = direction.magnitude < 0.1f ? lookDirection.normalized : direction.normalized;
            float dashDistance = playerSize.magnitude * playerData.FlashDistance;
            dashDuration = dashDistance / chargeForce;
            currentDashTime = 0f;
        }
    }

    void StopCharging()
    {
        currentMoveStatus = MoveStatus.Move;
        EnableCollision(true);
    }

    bool IsWallHit()
    {
        Vector2 rayStart = (Vector2)transform.position;
        RaycastHit2D hit = Physics2D.Raycast(rayStart, dashDirection, 0.1f, wallLayer);
        return hit.collider != null;
    }

    void Rush(float chargeForce, Vector2 dashDirection)
    {
        transform.Translate(chargeForce * Time.deltaTime * dashDirection, Space.World);
    }

    public void StartStun(Vector2 direction, float pushDistance, int bounceCount)
    {
        this.bounceCount = bounceCount;
        dashDirection = direction;
        float dashDistance = playerSize.magnitude * pushDistance;
        dashDuration = dashDistance / chargeForce;
        currentDashTime = 0f;
        EnableCollision(false);
        currentMoveStatus = MoveStatus.Stun;
    }

    void StopStun()
    {
        currentMoveStatus = MoveStatus.Move;
        EnableCollision(true);
    }

    void StartStunWall()
    {
        currentMoveStatus = MoveStatus.StunWall;
    }

    void StopStunWall()
    {
        bounceCount = 0;
        currentBounceCount = 0;
        currentMoveStatus = MoveStatus.Move;
        EnableCollision(true);
    }

    void EnableCollision(bool collisionSwitch)
    {
        int playerLayer = LayerMask.NameToLayer("Player");
        Physics2D.IgnoreLayerCollision(playerLayer, playerLayer, !collisionSwitch);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentMoveStatus == MoveStatus.Rush && collision.gameObject.CompareTag("Player"))
        {
            StopCharging();
            PlayerPowerRush playerPowerRush = collision.transform.GetComponent<PlayerPowerRush>();
            if (playerPowerRush.currentMoveStatus == MoveStatus.Move)
            {
                Vector3 otherPlayerPosition = collision.gameObject.transform.position;
                Vector2 knockbackDirection = (otherPlayerPosition - transform.position).normalized;
                playerPowerRush.StartStun(knockbackDirection, playerData.PushDistance, playerData.BounceCount);
            }
            else if (playerPowerRush.currentMoveStatus == MoveStatus.Rush)
            {
                playerPowerRush.StopCharging();
            }
            return;
        }

        if ((currentMoveStatus == MoveStatus.Stun || currentMoveStatus == MoveStatus.StunWall) && collision.gameObject.CompareTag("Wall"))
        {
            if (playerData.BounceCount <= 0)
                StopStun();

            Vector2 wallNormal = collision.contacts[0].normal;
            dashDirection = Vector2.Reflect(dashDirection, wallNormal).normalized;
            if (currentMoveStatus == MoveStatus.Stun)
            {
                StartStunWall();
            }
            else
            {
                currentBounceCount++;
                if (currentBounceCount >= bounceCount)
                {
                    StopStunWall();
                }
            }
        }
    }

}