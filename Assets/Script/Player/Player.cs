using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]    float _moveSpeed;
    [SerializeField]    float _dodgeSpeed;
    [SerializeField]    float _dodgeCoolTime;
    [SerializeField]    float _dodgeTime;

    float _dodgeCoolTimer = 0;
    float _dodgeTimer = 0;
    bool _isDodging;
    Vector3 _dodgeDirection;
    Vector2 _moveInput;
    Vector3 _moveDirection;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {

    }


    void Update()
    {
        PlayerMove();
        PlayerDodge();
    }
    private void FixedUpdate()
    {
        if( _isDodging)
        {
            _dodgeTimer -= Time.fixedDeltaTime;

            if (_dodgeTimer <= 0)
            {

                rb.linearVelocity = Vector3.zero;
                _isDodging = false;
                return;
            }

            Vector3 dodgelinervelocity = rb.linearVelocity;
            dodgelinervelocity.x = _dodgeDirection.x * _dodgeSpeed;
            dodgelinervelocity.z = _dodgeDirection.z * _dodgeSpeed;

            rb.linearVelocity = dodgelinervelocity;
            return;
        }


        _moveDirection = new Vector3(_moveInput.x, 0f, _moveInput.y);

        Vector3 linearvelocity = rb.linearVelocity;
        linearvelocity.x = _moveDirection.x * _moveSpeed;
        linearvelocity.z = _moveDirection.z * _moveSpeed;

        rb.linearVelocity = linearvelocity;

    }
    public void PlayerMove()
    {
        //キーボード入力
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector2 keyboardInput = new Vector2(x, z);

        //コントローラーの左スティック入力
        Vector2 stickInput = Vector2.zero;

        if (Gamepad.current != null)
        {
            stickInput = Gamepad.current.leftStick.ReadValue();
        }

        //入力があるほうを採用
        _moveInput = stickInput.sqrMagnitude > 0.01f ? stickInput : keyboardInput;

        //斜め移動速度を一定にする
        _moveInput = Vector2.ClampMagnitude(_moveInput, 1f);
    }

    public void PlayerDodge()
    {
        if (_dodgeCoolTimer >= 0)
        {
            _dodgeCoolTimer -= Time.deltaTime;
        }

        bool _isDodgePressed = false;
        //回避キーボード入力
        if (!_isDodging && Input.GetKeyDown(KeyCode.Space) && _dodgeCoolTimer <= 0)
        {
            _isDodgePressed = true;
        }
        //回避コントローラー入力
        if (!_isDodging && Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame && _dodgeCoolTimer <= 0)
        {
            _isDodgePressed = true;
        }
        if (_isDodgePressed)
        {
            StartDodge();
        }
    }

    public void StartDodge()
    {
        if(_moveInput.sqrMagnitude <= 0f)
        {
            return;
        }
        _isDodging = true;
        _dodgeCoolTimer = _dodgeCoolTime;
        _dodgeTimer = _dodgeTime;

        _dodgeDirection = new Vector3(_moveInput.x,0f,_moveInput.y).normalized;
    }
}
