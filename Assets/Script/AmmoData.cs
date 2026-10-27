using UnityEngine;

[CreateAssetMenu(fileName = "AmmoData", menuName ="Game/Ammo Data")]
public class AmmoData : ScriptableObject
{
    public WeaponType _weaponType;
    public int _ammoCount;
    public Sprite _ammoIcon;
}
