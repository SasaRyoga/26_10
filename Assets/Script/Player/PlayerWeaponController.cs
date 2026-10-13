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

    GameObject[] _weaponSlot = new GameObject[3];
    GameObject _stageManagerObj;

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
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _stageManagerObj = GameObject.FindGameObjectWithTag("StageManager");
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

        //武器装備スロット切り替え
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
        //射撃入力
        if (_stageManagerObj == null)
        {
            //_weaponSlot[_currentSlotIndex].GetComponent<Weapon>().;
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
            weaponObj.gameObject.SetActive(false);
        }
        if (index == 1)
        {
            weaponObj.transform.position = new Vector3(0, 200f, 0);
            weaponObj.gameObject.SetActive(false);

        }
    }
    void PickUpWeaponIn(GameObject weaponObj, Vector3 dropPos)
    {
        weaponObj.gameObject.SetActive(true);
        weaponObj.transform.position = dropPos;

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
