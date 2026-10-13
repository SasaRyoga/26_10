using UnityEngine;
using UnityEngine.SceneManagement;

public class Weapon : MonoBehaviour
{
    //残弾数、射撃、発射間隔、リロードを管理する


    [SerializeField]    public WeaponData _weaponData;

    //マガジン残段数
    public int _currentMagazineAmmo;
    int _pendingReloadAmmo;

    public float _fireIntervalTimer = 0;
    public float _reloadIntervalTimer = 0;

    void Start()
    {
        // マガジンを最大弾数で初期化
        _currentMagazineAmmo = _weaponData._magazineSize;
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

            // リロード時間が終了した
            if (_reloadIntervalTimer <= 0)
            {
                _reloadIntervalTimer = 0;

                // マガジンに弾薬を補充
                _currentMagazineAmmo += _pendingReloadAmmo;

                Debug.Log("リロード完了");

                // 補充予定の弾数をリセット
                _pendingReloadAmmo = 0;
            }
        }
    }

    public void FullAutoShot()
    {
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
        //射撃間隔中なら撃てない
        if(_fireIntervalTimer > 0)
        {
            return;
        }
        //マガジン０なら撃てない
        if(_currentMagazineAmmo <= 0)
        {
            return;
        }
        //射撃インターバル開始
        _fireIntervalTimer = _weaponData._fireInterval;

        //弾消費
        _currentMagazineAmmo--;

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
        //リロードインターバル開始
        _reloadIntervalTimer = _weaponData._reloadTime;

        // 補充予定の弾数を保存
        _pendingReloadAmmo = addAmmo;

    }
}
