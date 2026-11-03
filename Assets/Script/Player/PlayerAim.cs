
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    Camera _mainCamera;
    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField] Transform _muzzlePointTransform;
    [SerializeField] float _maxDistance;
    [SerializeField] Player _playerCs;
    [SerializeField] PlayerWeaponController _weaponControllerCs;
    [SerializeField] SpreadAreaView _spreadAreaViewCs;

    WeaponData _currentVisualWeaponData;
    Vector3 _endPoint;
    private void Start()
    {
        _mainCamera = Camera.main;
    }
    private void Update()
    {
        Vector3 direction = Vector3.zero;


        Ray muzzlerRay = new Ray(_muzzlePointTransform.position, _muzzlePointTransform.forward);
        _endPoint = _muzzlePointTransform.position + _muzzlePointTransform.forward * _maxDistance;
        if (Physics.Raycast(muzzlerRay, out RaycastHit hit, _maxDistance))
        {
            _endPoint = hit.point;
        }
        //右スティック入力を取得
        if (Gamepad.current != null)
        {
            Vector2 stickInput =
                Gamepad.current.rightStick.ReadValue();
            //スティックが一定以上倒されている場合
            if (stickInput.sqrMagnitude > 0.09f)
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


        _lineRenderer.SetPosition(0, _muzzlePointTransform.position);
        _lineRenderer.SetPosition(1, _endPoint);
        if (_playerCs._isAiming)
        {
            _lineRenderer.enabled = true;
        }
        else
        {
            _lineRenderer.enabled = false;
        }
        UpdateAimVisual();
    }
    void UpdateAimVisual()
    {
        WeaponData weaponData = _weaponControllerCs.CurrentWeaponData;

        // 照準モードでない場合
        if (!_playerCs._isAiming || weaponData == null)
        {
            _lineRenderer.enabled = false;
            _spreadAreaViewCs.SetVisible(false);
            return;
        }

        // ショットガンの場合
        if (weaponData._weaponType == WeaponType.ShotGun)
        {
            _lineRenderer.enabled = false;
            _spreadAreaViewCs.SetVisible(true);

            // 武器が切り替わったときだけ設定値を更新
            if (_currentVisualWeaponData != weaponData)
            {
                _spreadAreaViewCs.SetSpreadAngle(weaponData._spreadAngle);
                _spreadAreaViewCs.SetDistance(weaponData._range);

                _currentVisualWeaponData = weaponData;
            }
        }
        else
        {
            _lineRenderer.enabled = true;
            _spreadAreaViewCs.SetVisible(false);

            _currentVisualWeaponData = weaponData;
        }
    }
}
