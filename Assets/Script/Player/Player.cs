using UnityEngine;

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
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        _moveInput = new Vector2(x, z).normalized;
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
