using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UniRx;


public class PlayerWeaponController : MonoBehaviour
{
    //武器の所持・装備・交換、射撃入力を管理する
    [SerializeField] Transform _firePointTransform;
    [SerializeField] GameObject _handgunPrefab;
    [SerializeField] TextMeshProUGUI _currenIndexText;
    [Header("予備弾薬最大所持数")]
    [SerializeField]    int[] _maxAmmo = new int[4];
    [Header("予備弾薬所持数")]
    [SerializeField] int[] _ammo = new int[4];

    // 武器種ごとの予備弾薬の最大所持数
    Dictionary<WeaponType, int> _maxReserveAmmoDic =
        new Dictionary<WeaponType, int>();

    // 武器種ごとの現在の予備弾薬数
    Dictionary<WeaponType, int> _reserveAmmoDic =
        new Dictionary<WeaponType, int>();

    GameObject[] _weaponSlot = new GameObject[3];
    GameObject _stageManagerObj;
    StageManager _stageManagerCs;

    //装備中のスロット番号
    int _currentSlotIndex = 0;

    public ReactiveProperty<int> CurrentSlotIndex = new ReactiveProperty<int>(2);
    //装備中の武器のWeapon.cs
    Weapon _weaponCs;
    //装備中の武器の見た目
    GameObject _equippedWeaponVisual;

    //近くにある武器
    GameObject _nearByWeapon;

    //武器を捨てる位置
    Vector3 _dropPosition;
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        //インスペクターからの設定をDictionaryに適応
        for(int i = 0; i < _maxAmmo.Length; i++)
        {
            WeaponType type = (WeaponType)i;
            _maxReserveAmmoDic[type] = _maxAmmo[i];
            _reserveAmmoDic[type] = _ammo[i];
        }
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _stageManagerObj = GameObject.FindGameObjectWithTag("StageManager");
        _stageManagerCs = _stageManagerObj?.GetComponent<StageManager>();
        _currentSlotIndex = 2;
        CurrentSlotIndex.Value = 2;
    }
    void Start()
    {
        _weaponSlot[2] = _handgunPrefab;
        CurrentSlotIndex.Subscribe(index =>
        {
            _currentSlotIndex = index;

            if (_weaponSlot[index] != null)
            {
                _weaponCs =
                    _weaponSlot[index].GetComponent<Weapon>();
                Debug.Log("Weapon.cs取得");
            }
            else
            {
                _weaponCs = null;
            }
        })
    .AddTo(this);
    }

    void Update()
    {
        //射撃中でもリロード中でもないときスロット切り替え可能
        if (_weaponCs._fireIntervalTimer <= 0 && _weaponCs._reloadIntervalTimer <= 0)
        {
            //武器を拾う交換する
            if (_nearByWeapon != null)
            {
                //キーボード入力
                if (Input.GetKeyDown(KeyCode.F))
                {
                    PickUpWeapon();
                }
                //コントローラー入力
                if (Gamepad.current != null && Gamepad.current.buttonWest.wasPressedThisFrame)
                {
                    PickUpWeapon();
                }
            }
        }


        //武器装備スロット切り替え

        //射撃中でもリロード中でもないときスロット切り替え可能
        if(_weaponCs._fireIntervalTimer <= 0 &&_weaponCs._reloadIntervalTimer <= 0)
        {
            //キーボード入力
            if (Input.GetKeyDown(KeyCode.Alpha1) && _weaponSlot[0] != null)
            {
                _currentSlotIndex = 0;
                CurrentSlotIndex.Value = 0;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && _weaponSlot[1] != null)
            {
                _currentSlotIndex = 1;
                CurrentSlotIndex.Value = 1;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _currentSlotIndex = 2;
                CurrentSlotIndex.Value = 2;
            }
            //コントローラー入力
            if (Gamepad.current != null && Gamepad.current.leftShoulder.wasPressedThisFrame)
            {
                if (_weaponSlot[0] != null && _weaponSlot[1] != null)
                {
                    _currentSlotIndex--;
                    CurrentSlotIndex.Value--;
                }
                if (_weaponSlot[1] == null)
                {
                    _currentSlotIndex = 1;
                    CurrentSlotIndex.Value = 1;
                }
            }
            if (Gamepad.current != null && Gamepad.current.rightShoulder.wasPressedThisFrame)
            {
                _currentSlotIndex++;
                CurrentSlotIndex.Value++;
                if (_weaponSlot[1] == null)
                {
                    _currentSlotIndex = 2;
                    CurrentSlotIndex.Value = 2;
                }
            }
            _currentSlotIndex = Mathf.Clamp(_currentSlotIndex, 0, 2);
            CurrentSlotIndex.Value = Mathf.Clamp(CurrentSlotIndex.Value, 0, 2);
        }
        


        //強化要素がないとき
        if (_stageManagerObj == null && _weaponCs != null)
        {

            //射撃入力
            if (_weaponCs._weaponData._fireMode == FireMode.SemiAuto)
            {
                //キーボード入力
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    _weaponCs.SemiAutoShot();
                }
                //コントローラー入力
                if(Gamepad.current != null && Gamepad.current.rightTrigger.wasPressedThisFrame)
                {
                    _weaponCs.SemiAutoShot();
                }
            }
            if(_weaponCs._weaponData._fireMode == FireMode.FullAuto)
            {
                //キーボード入力
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    _weaponCs.FullAutoShot();
                }
                //コントローラー入力
                if (Gamepad.current != null && Gamepad.current.rightTrigger.isPressed)
                {
                    _weaponCs.FullAutoShot();
                }
            }

            //リロード入力
            //キーボード入力
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (TryReload())
                {
                    _weaponCs.Reload(ReturnNeedAmmo());
                }
            }
            //コントローラー入力
            if(Gamepad.current != null && Gamepad.current.buttonNorth.wasPressedThisFrame)
            {
                if (TryReload())
                {
                    _weaponCs.Reload(ReturnNeedAmmo());
                }
            }
        }
        //強化要素があるとき
        if(_stageManagerObj  != null && _weaponCs != null)
        {
            //射撃入力　単押し
            if (_weaponCs._weaponData._fireMode == FireMode.SemiAuto)
            {
                //装備中の武器タイプ取得
                WeaponType weaponType = _weaponCs._weaponData._weaponType;
                //ダメージ倍率取得
                float damagePrd = _stageManagerCs.GetDamageProduct(weaponType);
                //射撃間隔取得
                float fireInterval = _stageManagerCs.GetFireIntervalProduct(weaponType);

                //キーボード入力
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if(weaponType == WeaponType.HandGun)
                    {
                        _weaponCs.SemiAutoShot();
                    }
                    else
                    {
                        _weaponCs.EnhancedSemiAutoShot(fireInterval, damagePrd);
                    }
                }
                //コントローラー入力
                if (Gamepad.current != null && Gamepad.current.rightTrigger.wasPressedThisFrame)
                {
                    if (weaponType == WeaponType.HandGun)
                    {
                        _weaponCs.SemiAutoShot();
                    }
                    else
                    {
                        _weaponCs.EnhancedSemiAutoShot(fireInterval, damagePrd);
                    }
                }
            }
            //射撃入力　長押し
            if (_weaponCs._weaponData._fireMode == FireMode.FullAuto)
            {
                //装備中の武器タイプ取得
                WeaponType weaponType = _weaponCs._weaponData._weaponType;
                //ダメージ倍率取得
                float damagePrd = _stageManagerCs.GetDamageProduct(weaponType);
                //射撃間隔取得
                float fireInterval = _stageManagerCs.GetFireIntervalProduct(weaponType);

                //キーボード入力
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    if(weaponType == WeaponType.HandGun)
                    {
                        _weaponCs.FullAutoShot();
                    }
                    else
                    {
                        _weaponCs.EnhancedFullAutoShot(fireInterval, damagePrd);
                    }
                }
                //コントローラー入力
                if (Gamepad.current != null && Gamepad.current.rightTrigger.isPressed)
                {
                    if (weaponType == WeaponType.HandGun)
                    {
                        _weaponCs.FullAutoShot();
                    }
                    else
                    {
                        _weaponCs.EnhancedFullAutoShot(fireInterval, damagePrd);
                    }
                }


            }

            //リロード入力
            //キーボード入力
            if (Input.GetKeyDown(KeyCode.R))
            {
                //装備中の武器タイプ取得
                WeaponType weaponType = _weaponCs._weaponData._weaponType;
                //リロード短縮倍率取得
                float reloadInterval = _stageManagerCs.GetReloadIntervalProduct(weaponType);
                //強化後マガジンサイズ取得
                int maxMagazineSize = _weaponCs._weaponData._magazineSize + _stageManagerCs.GetMagazineSizeSum(weaponType);

                if(weaponType == WeaponType.HandGun)
                {
                    if (TryReload())
                    {
                        _weaponCs.Reload(ReturnNeedAmmo());
                    }
                }
                else if (EnhancedTryReload(weaponType, maxMagazineSize))
                {
                    _weaponCs.EnhanceReload(reloadInterval, EnhancedReturnNeedAmmo(weaponType, maxMagazineSize));
                }
            }
            //コントローラー入力
            if (Gamepad.current != null && Gamepad.current.buttonNorth.wasPressedThisFrame)
            {
                //装備中の武器タイプ取得
                WeaponType weaponType = _weaponCs._weaponData._weaponType;
                //リロード短縮倍率取得
                float reloadInterval = _stageManagerCs.GetReloadIntervalProduct(weaponType);
                //強化後マガジンサイズ取得
                int maxMagazineSize = _weaponCs._weaponData._magazineSize + _stageManagerCs.GetMagazineSizeSum(weaponType);

                if (weaponType == WeaponType.HandGun)
                {
                    if (TryReload())
                    {
                        _weaponCs.Reload(ReturnNeedAmmo());
                    }
                }
                else if (EnhancedTryReload(weaponType, maxMagazineSize))
                {
                    _weaponCs.EnhanceReload(reloadInterval, EnhancedReturnNeedAmmo(weaponType, maxMagazineSize));
                }
            }
        }

    }
    void PickUpWeapon()
    {
        //スロット１と２が空いていれば
        if (_weaponSlot[0] == null && _weaponSlot[1] == null)
        {
            _weaponSlot[0] = _nearByWeapon;
            PickUpWeaponOut(_weaponSlot[0], 0);
            return;
        }
        //スロット２だけ空いていれば
        if (_weaponSlot[0] != null && _weaponSlot[1] == null)
        {
            _weaponSlot[1] = _nearByWeapon;
            PickUpWeaponOut(_weaponSlot[1], 1);
            return;
        }
        //スロットが空いていなければ交換
        if (_weaponSlot[0] != null && _weaponSlot[1] != null)
        {
            _dropPosition = _nearByWeapon.transform.position;
            //スロット１と交換
            if (_currentSlotIndex == 2)
            {
                PickUpWeaponIn(_weaponSlot[1], _dropPosition);
                _weaponSlot[1] = _nearByWeapon;
                PickUpWeaponOut(_weaponSlot[1], 1);
                _nearByWeapon = null;
                return;
            }
            else
            {
                PickUpWeaponIn(_weaponSlot[_currentSlotIndex], _dropPosition);
                _weaponSlot[_currentSlotIndex] = _nearByWeapon;
                _weaponCs = _nearByWeapon.GetComponent<Weapon>();
                Debug.Log("Weapon.cs取得");
                PickUpWeaponOut(_weaponSlot[_currentSlotIndex], _currentSlotIndex);
                _nearByWeapon = null;
                return;
            }
        }

    }
    void PickUpWeaponOut(GameObject weaponObj, int index)
    {
        if (index == 0)
        {
            weaponObj.transform.position = new Vector3(0, 100f, 0);
        }
        if (index == 1)
        {
            weaponObj.transform.position = new Vector3(0, 200f, 0);

        }
    }
    void PickUpWeaponIn(GameObject weaponObj, Vector3 dropPos)
    {
        weaponObj.transform.position = dropPos;

    }
    bool EnhancedTryReload(WeaponType weaponType, int maxMagazineSize)
    {
        if (_weaponCs._fireIntervalTimer > 0)
        {
            Debug.Log("射撃インターバル中はリロード不可");
            return false;
        }
        if (_weaponCs._reloadIntervalTimer > 0)
        {
            Debug.Log("リロードインターバル中はリロード不可");
            return false;
        }

        if (_reserveAmmoDic[weaponType] <= 0)
        {
            Debug.Log("予備弾薬無し");
            return false;
        }
        if (_weaponCs._currentMagazineAmmo >= maxMagazineSize)
        {
            Debug.Log("マガジンが満タン");
            return false;
        }
        return true;
    }
    bool TryReload()
    {
        if (_weaponCs._fireIntervalTimer > 0)
        {
            Debug.Log("射撃インターバル中はリロード不可");
            return false;
        }
        if (_weaponCs._reloadIntervalTimer > 0)
        {
            Debug.Log("リロードインターバル中はリロード不可");
            return false;
        }
        //武器種を取得
        WeaponType weaponType = _weaponCs._weaponData._weaponType;
        if (_weaponCs._currentMagazineAmmo >= _weaponCs._weaponData._magazineSize)
        {
            Debug.Log("マガジンが満タン");
            return false;
        }
        //ハンドガンであれば予備弾薬必要なし
        if(weaponType == WeaponType.HandGun)
        {
            return true;
        }
        if (_reserveAmmoDic[weaponType] <= 0)
        {
            Debug.Log("予備弾薬無し");
            return false;
        }

        return true;
        
    }
    int EnhancedReturnNeedAmmo(WeaponType weaponType, int maxMagazineSize)
    {
        //必要弾薬数取得
        int needAmmo = maxMagazineSize - _weaponCs._currentMagazineAmmo;
        // 実際に補充できる弾数
        int addAmmo = Mathf.Min(needAmmo, _reserveAmmoDic[weaponType]);

        // 予備弾薬から補充予定の弾数を引く
        _reserveAmmoDic[weaponType] -= addAmmo;

        return addAmmo;
    }
    int ReturnNeedAmmo()
    {
        //武器種を取得
        WeaponType weaponType = _weaponCs._weaponData._weaponType;

        //必要弾薬数取得
        int needAmmo = _weaponCs._weaponData._magazineSize - _weaponCs._currentMagazineAmmo;
        //ハンドガンであれば予備弾薬関係なし
        if(weaponType == WeaponType.HandGun)
        {
            return needAmmo;
        }
        // 実際に補充できる弾数
        int addAmmo = Mathf.Min(needAmmo,_reserveAmmoDic[weaponType]);

        // 予備弾薬から補充予定の弾数を引く
        _reserveAmmoDic[weaponType] -= addAmmo;

        return addAmmo;

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            _nearByWeapon = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            _nearByWeapon = null;
        }
    }
}
