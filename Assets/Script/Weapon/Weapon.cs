using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;

public class Weapon : MonoBehaviour
{
    //残弾数、射撃、発射間隔、リロードを管理する


    [SerializeField] public WeaponData _weaponData;

    //マガジン残段数
    public int _currentMagazineAmmo;
    public ReactiveProperty<int> _currentMagazineAmmo2 = new ReactiveProperty<int>(0);
    public ReactiveProperty<float> _reloadIntervalTimer2 = new ReactiveProperty<float>(0);
    int _pendingReloadAmmo;

    public float _fireIntervalTimer = 0;
    public float _reloadIntervalTimer = 0;
    private void Awake()
    {
        // マガジンを最大弾数で初期化
        _currentMagazineAmmo = _weaponData._magazineSize;
        _currentMagazineAmmo2.Value = _weaponData._magazineSize;
    }

    void Start()
    {

    }
    private void Update()
    {

        //Fireインターバル中
        if (_fireIntervalTimer > 0)
        {
            _fireIntervalTimer -= Time.deltaTime;
            if (_fireIntervalTimer <= 0)
            {
                _fireIntervalTimer = 0;
            }
        }
        // リロード中
        if (_reloadIntervalTimer > 0)
        {
            _reloadIntervalTimer -= Time.deltaTime;
            _reloadIntervalTimer2.Value =
                Mathf.Max(0f, _reloadIntervalTimer);
            // リロード時間が終了した
            if (_reloadIntervalTimer <= 0)
            {
                _reloadIntervalTimer = 0;
                _reloadIntervalTimer2.Value = 0;
                // マガジンに弾薬を補充
                _currentMagazineAmmo += _pendingReloadAmmo;
                _currentMagazineAmmo2.Value += _pendingReloadAmmo;


                Debug.Log("リロード完了");
                Debug.Log($"リロード完了: {gameObject.name}, " +
          $"補充弾数={_pendingReloadAmmo}");

                // 補充予定の弾数をリセット
                _pendingReloadAmmo = 0;
            }
        }
    }

    public void FullAutoShot(Transform firepoint)
    {
        //リロード中なら撃てない
        if (_reloadIntervalTimer > 0)
        {
            Debug.Log("リロード中は射撃不可");
            return;
        }
        //射撃間隔中なら撃てない
        if (_fireIntervalTimer > 0)
        {
            Debug.Log("射撃インターバル中");
            Debug.Log(_fireIntervalTimer);
            return;
        }
        //マガジン０なら撃てない
        if (_currentMagazineAmmo <= 0)
        {
            Debug.Log("マガジン０");
            return;

        }
        //射撃インターバル開始
        _fireIntervalTimer = _weaponData._fireInterval;

        //弾消費
        _currentMagazineAmmo--;
        _currentMagazineAmmo2.Value--;

        //ここから発射処理

        //弾の発射が生成である場合
        if (_weaponData._hitMethod == HitMethod.Projectile)
        {
            //弾生成処理
            Debug.Log("弾生成");
        }
        else
        {
            //Raycast処理
            Debug.Log("Raycast処理");
            RaycastShot(firepoint, _weaponData._range, _weaponData._damage);

        }
    }
    public void SemiAutoShot(Transform firepoint)
    {
        //リロード中なら撃てない
        if (_reloadIntervalTimer > 0)
        {
            Debug.Log("リロード中は射撃不可");
            return;
        }
        //射撃間隔中なら撃てない
        if (_fireIntervalTimer > 0)
        {
            return;
        }
        //マガジン０なら撃てない
        if (_currentMagazineAmmo <= 0)
        {
            Debug.Log("マガジン0");
            return;
        }
        //射撃インターバル開始
        _fireIntervalTimer = _weaponData._fireInterval;

        //弾消費
        _currentMagazineAmmo--;
        _currentMagazineAmmo2.Value--;

        //ここから発射処理

        //弾の発射が生成である場合
        if (_weaponData._hitMethod == HitMethod.Projectile)
        {
            //弾生成処理
            Debug.Log("弾生成");
        }
        else
        {
            //Raycast処理
            Debug.Log("Raycast処理");
            RaycastShot(firepoint, _weaponData._range, _weaponData._damage);

        }

    }

    public void Reload(int addAmmo)
    {
        Debug.Log("リロード開始");
        //リロードインターバル開始
        _reloadIntervalTimer = _weaponData._reloadTime;
        _reloadIntervalTimer2.Value = _reloadIntervalTimer;
        // 補充予定の弾数を保存
        _pendingReloadAmmo = addAmmo;

    }
    public void EnhanceReload(float reloadTime, int addAmmo)
    {
        Debug.Log("リロード開始");
        //リロードインターバル開始
        _reloadIntervalTimer = _weaponData._reloadTime * reloadTime;
        _reloadIntervalTimer2.Value = _reloadIntervalTimer;
        //補充予定の段数を保存
        _pendingReloadAmmo = addAmmo;
    }
    public void EnhancedSemiAutoShot(float fireInterval, float damage, Transform firepoint)
    {
        //リロード中なら撃てない
        if (_reloadIntervalTimer > 0)
        {
            Debug.Log("リロード中は射撃不可");
            return;
        }
        //射撃間隔中なら撃てない
        if (_fireIntervalTimer > 0)
        {
            return;
        }
        //マガジン０なら撃てない
        if (_currentMagazineAmmo <= 0)
        {
            Debug.Log("マガジン0");
            return;
        }
        //射撃インターバル開始
        _fireIntervalTimer = _weaponData._fireInterval * fireInterval;

        //弾消費
        _currentMagazineAmmo--;
        _currentMagazineAmmo2.Value--;

        //ここから発射処理

        //弾の発射が生成である場合
        if (_weaponData._hitMethod == HitMethod.Projectile)
        {
            //弾生成処理
            //ダメージ加算処理
            Debug.Log("弾生成 damage×" + damage);
        }
        else
        {
            //Raycast処理
            //ダメージ加算処理
            Debug.Log("Raycast処理×" + damage);
            RaycastShot(firepoint, _weaponData._range, damage * _weaponData._damage);

        }
    }
    public void EnhancedFullAutoShot(float fireInterval, float damage, Transform firepoint)
    {
        //リロード中なら撃てない
        if (_reloadIntervalTimer > 0)
        {
            Debug.Log("リロード中は射撃不可");
            return;
        }
        //射撃間隔中なら撃てない
        if (_fireIntervalTimer > 0)
        {
            return;
        }
        //マガジン０なら撃てない
        if (_currentMagazineAmmo <= 0)
        {
            Debug.Log("マガジン0");
            return;
        }
        //射撃インターバル開始
        _fireIntervalTimer = _weaponData._fireInterval * fireInterval;

        //弾消費
        _currentMagazineAmmo--;
        _currentMagazineAmmo2.Value--;

        //ここから発射処理

        //弾の発射が生成である場合
        if (_weaponData._hitMethod == HitMethod.Projectile)
        {
            //弾生成処理
            //ダメージ加算処理
            Debug.Log("弾生成 damage×" + damage);
        }
        else
        {
            //Raycast処理
            //ダメージ加算処理
            Debug.Log("Raycast処理×" + damage);
            RaycastShot(firepoint, _weaponData._range, damage * _weaponData._damage);
        }
    }
    private void RaycastShot(Transform firepoint, float range, float damage)
    {
        // ショットガンの場合
        if (_weaponData._weaponType == WeaponType.ShotGun)
        {
            int count = _weaponData._projectileCount;

            // Rayを複数本発射
            for (int i = 0; i < count; i++)
            {
                float angle = 0f;

                // Rayが2本以上の場合
                if (count > 1)
                {
                    float t = (float)i / (count - 1);

                    angle = Mathf.Lerp(
                        -_weaponData._spreadAngle,
                        _weaponData._spreadAngle,
                        t
                    );
                }

                // Rayの方向を決定
                Vector3 direction =
                    Quaternion.AngleAxis(angle, Vector3.up) * firepoint.forward;

                // 命中判定
                CheckRaycast(firepoint.position, direction, range, damage);
            }
        }
        else
        {
            // 通常武器の場合
            float randomAngle = Random.Range(
                -_weaponData._spreadAngle,
                _weaponData._spreadAngle
            );

            Vector3 direction =
                Quaternion.AngleAxis(randomAngle, Vector3.up) * firepoint.forward;

            // 命中判定
            CheckRaycast(firepoint.position, direction, range, damage);
        }
    }
    private void CheckRaycast(
        Vector3 origin,
        Vector3 direction,
        float range,
        float damage)
    {
        Ray shotRay = new Ray(origin, direction);

        // Rayの終点を射程距離の位置にする
        Vector3 endPoint = origin + direction * range;

        if (Physics.Raycast(shotRay, out RaycastHit hit, range))
        {
            // 命中した場合は命中位置を終点にする
            endPoint = hit.point;

            Debug.Log("命中：" + hit.collider.name);
            Debug.Log("ダメージ：" + damage);
        }

        // Rayの表示処理
        Debug.DrawLine(origin, endPoint, Color.red, 0.2f);
    }
}
