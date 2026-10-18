using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Game/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Weapon Settings")]
    public WeaponType _weaponType;
    public FireMode _fireMode;
    public HitMethod _hitMethod;

    [Header("Shooting Settings")]
    public int _projectileCount;
    public float _spreadAngle;
    public float _fireInterval;
    public float _bulletSpeed;
    public float _damage;

    [Header("Magazine Settings")]
    public int _magazineSize;
    public float _reloadTime;

    [Header("Prefabs")]

    public GameObject _bulletPrefab;
    public GameObject _weaponPrefab;
    [Header("WaeponName")]
    public string _weaponName;
}
