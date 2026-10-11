using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]    float _moveSpeed;

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
        //キーボード入力
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector2 keyboardInput = new Vector2(x, z);

        //コントローラーの左スティック入力
        Vector2 stickInput = Vector2.zero;

        if(Gamepad.current != null)
        {
            stickInput = Gamepad.current.leftStick.ReadValue();
        }

        //入力があるほうを採用
        _moveInput = stickInput.sqrMagnitude > 0.01f ? stickInput : keyboardInput;

        //斜め移動速度を一定にする
        _moveInput = Vector2.ClampMagnitude(_moveInput, 1f);
    }
    private void FixedUpdate()
    {
        _moveDirection = new Vector3(_moveInput.x, 0f, _moveInput.y);

        Vector3 linearvelocity = rb.linearVelocity;
        linearvelocity.x = _moveDirection.x * _moveSpeed;
        linearvelocity.z = _moveDirection.z * _moveSpeed;

        rb.linearVelocity = linearvelocity;

    }
}
