using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class WeaponSlotUI : MonoBehaviour
{
    [SerializeField]    Image _reloadGage;
    [SerializeField]    Text _reloadTime;
    [SerializeField]    Text _slotIndex;
    [SerializeField]    Sprite _nonSprite;
    [SerializeField]    Image[] _slotImage = new Image[3];
    [SerializeField]    Text[] _currentMagazune = new Text[3];
    [SerializeField]    Text[] _magazineSize = new Text[3];
    [SerializeField]    Text[] _reserveAmmo = new Text[3];

    [SerializeField]    WeaponUIController _weaponUIControllerCs;
    WeaponSlotUIData[] weaponSlotUIDatas  = new WeaponSlotUIData[3];
    int _currentSlotIndex = 2;
    void Start()
    {
        _weaponUIControllerCs.CurrentSlotIndex.Subscribe(x =>  IndexSlot(x));
        
      for(int i = 0; i < 3; i++)
        {
            int index = i;
            weaponSlotUIDatas[index] = _weaponUIControllerCs._weaponSlotUIDatas[index];
            //アイコン
            weaponSlotUIDatas[index]._Icon.Subscribe(x =>
            {
                if(x == null)
                {
                    _slotImage[index].sprite = _nonSprite;
                }
                else
                {
                    _slotImage[index].sprite = x;
                }
            })
                .AddTo(this);
            
            weaponSlotUIDatas[index]._CurrentMagazine.Subscribe(x => _currentMagazune[index].text = x.ToString()).AddTo(this);

            weaponSlotUIDatas[index]._MaxMagazine.Subscribe(x => _magazineSize[index].text = x.ToString()).AddTo(this);

            weaponSlotUIDatas[index]._CurrentReloadTime.Subscribe(x => ReloadStart(index, x)).AddTo(this);
            
        }
    }

    void ReloadStart(int index, float currentReloadTime)
    {
        if (index != _currentSlotIndex)
            return;

        float maxReloadTime =
            _weaponUIControllerCs
            ._weaponSlotUIDatas[index]
            ._MaxReloadTime.Value;

        _reloadGage.fillAmount =
            maxReloadTime > 0f
            ? Mathf.Clamp01(currentReloadTime / maxReloadTime)
            : 0f;

        _reloadTime.text = currentReloadTime.ToString("0.0");
    }

    void IndexSlot(int x)
    {
        _currentSlotIndex = x;
        _slotIndex.text = $"{x + 1}" ;
    }
}
