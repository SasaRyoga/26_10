using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopRightUI : MonoBehaviour
{
    [SerializeField]    PlayerWeaponController _playerWeaponControllerCs;
    [SerializeField]    public Text[] _ammoText = new Text[4];

    public void Initialization(Dictionary<WeaponType,int> keyValuePairs)
    {
        for(int i = 0; i < 4; i++)
        {
            _ammoText[i].text = keyValuePairs[(WeaponType)i].ToString();
        }
    }
    public void RemoveAmmo(WeaponType weaponType, int resetveAmmo)
    {
        int index = (int)weaponType;
        _ammoText[index].text = resetveAmmo.ToString();
    }

    public void AddAmmo(WeaponType weaponType, int reserveAmmo, int addAmmo)
    {
        //追加前の所持弾薬
        int tempAmmo = reserveAmmo;
        reserveAmmo += addAmmo;
        int index = (int)weaponType;
        DOTween.To(() => tempAmmo,
            (int newAmmo) =>
            {
                _ammoText[index].text = newAmmo.ToString();
            },
            reserveAmmo,
            1
            );
    }
}
