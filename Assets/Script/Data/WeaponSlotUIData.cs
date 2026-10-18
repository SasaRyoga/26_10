using UnityEngine;
using UniRx;

public class WeaponSlotUIData
{
    /// <summary>武器を所持しているか：初期値 False</summary>
    public ReactiveProperty<bool> _isHasWeapon = new ReactiveProperty<bool>(false);
    /// <summary>アイコン</summary>
    public ReactiveProperty<Sprite> _Icon = new ReactiveProperty<Sprite>(null);
    /// <summary>名前</summary>
    public ReactiveProperty<string> _Name = new ReactiveProperty<string>("");
    /// <summary>残段数 </summary>
    public ReactiveProperty<int> _CurrentMagazine = new ReactiveProperty<int>(0);
    /// <summary>マガジン容量</summary>
    public ReactiveProperty<int> _MaxMagazine = new ReactiveProperty<int>(0);
    /// <summary> 武器タイプ</summary>
    public ReactiveProperty<WeaponType> _WeaponType = new ReactiveProperty<WeaponType>();
    /// <summary>残りリロード時間</summary>
    public ReactiveProperty<float> _CurrentReloadTime = new ReactiveProperty<float>(0);
    /// <summary>リロード時間</summary>
    public ReactiveProperty<float> _MaxReloadTime = new ReactiveProperty<float>(0);



}
