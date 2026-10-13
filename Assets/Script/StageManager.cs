using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("マガジンサイズ加算値")]
    [SerializeField]    int[] _magazineSizeSum = new int[4];
    [Header("射撃間隔乗算値")]
    [SerializeField]    float[] _fireIntervalProduct = new float[4];
    [Header("リロード時間乗算値")]
    [SerializeField]    float[] _reloadIntervalProduct = new float[4];
    [Header("ダメージ乗算値")]
    [SerializeField]    float[] _damageProduct = new float[4];
    [Header("アングル乗算値")]
    [SerializeField]    float[] _spreadAngleProduct = new float[4];
    Dictionary<WeaponType,int> _magazineSizeSumDct = new Dictionary<WeaponType,int>();
    Dictionary<WeaponType, float> _fireIntervalProductDct = new Dictionary<WeaponType,float>();
    Dictionary<WeaponType, float> _reloadIntervalProductDct = new Dictionary<WeaponType, float>();
    Dictionary<WeaponType, float> _damageProductDct = new Dictionary<WeaponType,float>();
    Dictionary<WeaponType, float> _spreadAngleProductDct = new Dictionary<WeaponType, float>();
    private void Awake()
    {
        //インスペクターからの設定をDictionaryに適応
        SetupDct();
    }
    void Start()
    {

    }

    void Update()
    {
        
    }
    void SetupDct()
    {
        for(int i = 0; i < 4; i++)
        {
            WeaponType weaponType = (WeaponType)i;
            _magazineSizeSumDct[weaponType] = _magazineSizeSum[i];
            _fireIntervalProductDct[weaponType] = _fireIntervalProduct[i];
            _reloadIntervalProductDct[weaponType]= _reloadIntervalProduct[i];
            _damageProductDct[weaponType] = _damageProduct[i];
            _spreadAngleProductDct[weaponType] = _spreadAngleProduct[i];
        }
    }
}
