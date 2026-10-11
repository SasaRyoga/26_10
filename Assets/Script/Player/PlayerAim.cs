
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }
    private void Update()
    {
        // マウス位置からカメラのRayを作る
        Ray ray = _mainCamera.ScreenPointToRay(
            Input.mousePosition
        );

        // 高さY=0の床を表す平面
        Plane groundPlane = new Plane(
            Vector3.up,
            Vector3.zero
        );

        // Rayと床の交点を求める
        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 targetPoint = ray.GetPoint(distance);

            // プレイヤーから目標地点への方向
            Vector3 direction = targetPoint - transform.position;
            direction.y = 0f;

            // 水平方向に回転させる
            if (direction.sqrMagnitude > 0.001f)
            {
                transform.rotation =
                    Quaternion.LookRotation(direction);
            }
        }
    }
}
