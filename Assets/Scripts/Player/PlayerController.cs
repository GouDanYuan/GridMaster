using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerEnum PlayerEnum = PlayerEnum.P1;
    public Color PlayerColor;
    public PlayerData PlayerData;

    /// <summary>
    /// 输入
    /// </summary>
    private InputActionAsset m_inputAction;
    /// <summary>
    /// 移动input
    /// </summary>
    private InputAction m_MoveAction;
    /// <summary>
    /// 攻击input
    /// </summary>
    private InputAction m_AttackAction;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    /// <summary>
    /// 初始位置
    /// </summary>
    private Vector3 m_startPosition;
    private Vector2 m_CurrentLookDirection;
    /// <summary>
    /// 当前所在格子
    /// </summary>
    private GridCell m_currentGridCell;

    private void Awake()
    {
        m_inputAction = Resources.Load("InpuAction") as InputActionAsset;
        m_MoveAction = m_inputAction.FindAction($"{PlayerEnum}/Move");
        m_MoveAction.Enable();
        m_AttackAction = m_inputAction.FindAction($"{PlayerEnum}/Attack");
        m_AttackAction.Enable();
        m_AttackAction.performed += (context) =>
        {
            m_Animator.SetTrigger("Attack");
        };

        PlayerSO playerSO = Resources.Load("SO/PlayerSO") as PlayerSO;
        PlayerData = playerSO.InitData;

        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponentInChildren<Animator>();
        PlayerColor = transform.GetComponent<SpriteRenderer>().color * Color.gray;
        m_startPosition = transform.position;
    }

    private void FixedUpdate()
    {
        var move = m_MoveAction.ReadValue<Vector2>();

        if (move != Vector2.zero)
        {
            SetLookDirectionFrom(move);
            var movement = move * PlayerData.Speed;
            var speed = movement.sqrMagnitude;
            var angle = Mathf.Atan2(move.y, move.x) * Mathf.Rad2Deg - 90;
            m_Rigidbody.MoveRotation(angle);
            m_Rigidbody.MovePosition(m_Rigidbody.position + movement * Time.deltaTime);
        }
    }

    private void SetLookDirectionFrom(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            m_CurrentLookDirection = direction.x > 0 ? Vector2.right : Vector2.left;
        }
        else
        {
            m_CurrentLookDirection = direction.y > 0 ? Vector2.up : Vector2.down;
        }
    }

    /// <summary>
    /// 攻击其他玩家
    /// </summary>
    /// <param name="otherPlayer"></param>
    public void AttackOther(PlayerController otherPlayer)
    {
        if (m_currentGridCell != null)
        {
            m_currentGridCell.SetOwner(this);
        }
    }

    /// <summary>
    /// 设置当前所在格子
    /// </summary>
    /// <param name="gridCell"></param>
    public void SetCurrentGridCell(GridCell gridCell)
    {
        m_currentGridCell = gridCell;
    }

    /// <summary>
    /// 获取当前所在格子
    /// </summary>
    /// <returns></returns>
    public GridCell GetCurrenGridCell()
    {
        return m_currentGridCell;
    }

    /// <summary>
    /// 暂停
    /// </summary>
    public void Pause()
    {
        m_MoveAction.Disable();
        m_AttackAction.Disable();
    }

    /// <summary>
    /// 恢复
    /// </summary>
    public void Resume()
    {
        m_MoveAction.Enable();
        m_AttackAction.Enable();
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 重置
    /// </summary>
    public void Reset()
    {
        gameObject.SetActive(false);
        transform.position = m_startPosition;
    }
}
