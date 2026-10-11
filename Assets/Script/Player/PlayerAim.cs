
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }
    private void Update()
    {
        Vector3 direction = Vector3.zero;

        //右スティック入力を取得
        if(Gamepad.current != null)
        {
            Vector2 stickInput =
                Gamepad.current.rightStick.ReadValue();
            //スティックが一定以上倒されている場合
            if(stickInput.sqrMagnitude > 0.09f)
            {
                direction = new Vector3(
                    stickInput.x,
                    0f,
                    stickInput.y
                    );
            }
        }
        //右スティック入力がない場合マウスを使う
        if (direction.sqrMagnitude < 0.001f)
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
                direction = targetPoint - transform.position;
                direction.y = 0f;
            }

        }

            //プレイヤーの向きを変更
            if (direction.sqrMagnitude > 0.001f)
            {
                transform.rotation =
                    Quaternion.LookRotation(direction);
            }
        
    }
}
