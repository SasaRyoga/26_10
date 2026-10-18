using UnityEngine;
using UniRx;
public class WeaponUIController : MonoBehaviour
{
    [SerializeField]    PlayerWeaponController _weaponControllerCs;
    public WeaponSlotUIData[] _weaponSlotUIDatas
        = new WeaponSlotUIData[3];
    /// <summary>装備中のインデックス </summary>
    public ReactiveProperty<int> CurrentSlotIndex = new ReactiveProperty<int>(2);
    private void Awake()
    {

    }
    public void Initialization()
    {
        //UI用データを３つ作成
        for (int i = 0; i < 3; i++)
        {
            _weaponSlotUIDatas[i] = new WeaponSlotUIData();
        }
    }
    void Start()
    {
        _weaponControllerCs.CurrentSlotIndex.Subscribe(x => CurrentSlotIndex.Value = x)
            .AddTo(this);
    }

    /// <summary>指定したスロットの武器情報を更新する</summary>
    public void UpdateWeaponSlotUIData(int index, Weapon weapon, int maxMagazinSize, float maxReloadTime)
    {
        // スロット番号の確認
        if (index < 0 || index >= _weaponSlotUIDatas.Length)
        {
            Debug.LogWarning("無効なスロット番号です");
            return;
        }

        WeaponSlotUIData uiData = _weaponSlotUIDatas[index];

        // 武器がない場合
        if (weapon == null)
        {
            uiData._HasWeapon.Value = false;
            uiData._Icon.Value = null;
            uiData._Name.Value = "";
            uiData._CurrentMagazine.Value = 0;
            uiData._MaxMagazine.Value = 0;
            uiData._CurrentReloadTime.Value = 0f;
            uiData._MaxReloadTime.Value = 0f;
            return;
        }

        // 武器がある場合
        WeaponData data = weapon._weaponData;

        uiData._HasWeapon.Value = true;
        uiData._Icon.Value = data._weaponIcon;
        uiData._Name.Value = data._weaponName;
        uiData._WeaponType.Value = data._weaponType;
        uiData._CurrentMagazine.Value = weapon._currentMagazineAmmo;
        uiData._MaxMagazine.Value = maxMagazinSize;
        uiData._CurrentReloadTime.Value = maxReloadTime;
        uiData._MaxReloadTime.Value = data._reloadTime;
    }
}
