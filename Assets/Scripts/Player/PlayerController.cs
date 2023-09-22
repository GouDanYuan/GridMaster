using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerEnum PlayerEnum = PlayerEnum.P1;
    public float Speed = 4.0f;
    public Color PlayerColor;

    private InputActionAsset m_inputAction;
    private InputAction m_MoveAction;
    private InputAction m_AttackAction;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private Vector3 m_startPosition;
    private Vector2 m_CurrentLookDirection;

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
            var movement = move * Speed;
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

    public void Pause()
    {
        m_MoveAction.Disable();
        m_AttackAction.Disable();
    }

    public void Resume()
    {
        m_MoveAction.Enable();
        m_AttackAction.Enable();
        gameObject.SetActive(true);
    }

    public void Reset()
    {
        gameObject.SetActive(false);
        transform.position = m_startPosition;
    }
}
