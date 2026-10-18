using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;

public class Weapon : MonoBehaviour
{
    //残弾数、射撃、発射間隔、リロードを管理する


    [SerializeField]    public WeaponData _weaponData;

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
            if( _fireIntervalTimer <= 0)
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

    public void FullAutoShot()
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
        }
    }
    public void SemiAutoShot()
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
        if(_currentMagazineAmmo <= 0)
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
        if(_weaponData._hitMethod == HitMethod.Projectile)
        {
            //弾生成処理
            Debug.Log("弾生成");
        }
        else
        {
            //Raycast処理
            Debug.Log("Raycast処理");
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
    public void EnhancedSemiAutoShot(float fireInterval, float damage)
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
        }
    }
    public void EnhancedFullAutoShot(float fireInterval, float damage)
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
        }
    }
}
