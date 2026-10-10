using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]    WeaponData _weaponData;
    //マガジン残段数
    public int _currentMagazineAmmo;

    void Start()
    {
        // マガジンを最大弾数で初期化
        _currentMagazineAmmo = _weaponData._magazineSize;
    }

    void Update()
    {
        
    }
}
