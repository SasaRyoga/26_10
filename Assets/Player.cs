using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]    float _moveSpeed;
    [SerializeField]    GameObject _camera;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
       
    }
    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 forward = _camera.transform.forward;
        Vector3 right = _camera.transform.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 direction = (forward * z + right * x);
        rb.linearVelocity = new Vector3(direction.x * _moveSpeed, 0, direction.z * _moveSpeed);
    }
}
