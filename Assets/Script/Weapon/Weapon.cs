using UnityEngine;

public class Weapon : MonoBehaviour
{
    //残弾数、射撃、発射間隔、リロードを管理する


    [SerializeField]    WeaponData _weaponData;
    //マガジン残段数
    public int _currentMagazineAmmo;

    float _fireIntervalTimer = 0;

    void Start()
    {
        // マガジンを最大弾数で初期化
        _currentMagazineAmmo = _weaponData._magazineSize;
    }
    private void Update()
    {
        if(_fireIntervalTimer >= 0f)
        {
            _fireIntervalTimer -= Time.deltaTime;
        }


    }

    public void FullAutoShot()
    {
        //射撃間隔中なら撃てない
        if (_fireIntervalTimer > 0)
        {
            return;
        }
        //マガジン０なら撃てない
        if (_currentMagazineAmmo <= 0)
        {
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
        }
        else
        {
            //Raycast処理
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
        }
        else
        {
            //Raycast処理
        }

    }

    public void Reroad()
    {

    }
}
