using UnityEngine;
using UniRx;
using UnityEngine.UI;
using System;
public class CenterUI : MonoBehaviour
{
    [SerializeField] WeaponUIController _weaponUIControllerCs;
    [SerializeField] Image _underImage;
    [SerializeField] Image _overImage;
    [SerializeField] Text _currentText;
    [SerializeField] Text _magazineText;
    //リロード時間購読解除用
    IDisposable _reloadSubsc;
    //残段数購読解除用
    IDisposable _magazineSubsc;
    //リロード開始検知解除用
    IDisposable _reloadStartSubsc;

    int _currentIndex = 2;
    float _reloadStartGauge;
    void Start()
    {
        //装備切り替えを検知
        _weaponUIControllerCs.CurrentSlotIndex.Subscribe(x => ChangeIndex(x)).AddTo(this);

        _weaponUIControllerCs.OnWeaponSlotUpdated
                .Subscribe(index =>
                {
                    if (index == _currentIndex)
                    {
                        ChangeIndex(index);
                    }
                })
                .AddTo(this);
    }
    //装備が変更されると
    void ChangeIndex(int index)
    {
        _currentIndex = index;
        //前の武器の購読を解除
        _reloadSubsc?.Dispose();
        _reloadSubsc = null;
        _magazineSubsc?.Dispose();
        _magazineSubsc = null;
        _reloadStartSubsc?.Dispose();
        _reloadStartSubsc = null;


        WeaponSlotUIData data = _weaponUIControllerCs._weaponSlotUIDatas[index];

        int maxmagazine = data._MaxMagazine.Value;
        int currentmagazine = data._CurrentMagazine.Value;
        float maxreloadtime = data._MaxReloadTime.Value;

        _magazineText.text = maxmagazine.ToString();
        _currentText.text = currentmagazine.ToString();
        _overImage.fillAmount = (float)currentmagazine / maxmagazine * 0.4f;


        _reloadStartSubsc = data._CurrentReloadTime
        .Pairwise()
        .Where(pair => pair.Previous <= 0f && pair.Current > 0f)
        .Subscribe(pair =>
        {
            _reloadStartGauge = _overImage.fillAmount;
        });
        //装備中の武器のリロード時間を購読
        _reloadSubsc = data._CurrentReloadTime.Subscribe(x => UpdateReloadGauge(x, maxreloadtime));
        //装備中の武器の残段数を購読
        _magazineSubsc = data._CurrentMagazine.Subscribe(x => UpdateShotGauge(x, maxmagazine));
    }
    void UpdateShotGauge(int currentmagazine, int maxmagazine)
    {
        _magazineText.text = maxmagazine.ToString();
        _currentText.text = currentmagazine.ToString();
        _overImage.fillAmount = (float)currentmagazine / maxmagazine * 0.4f;
    }
    void UpdateReloadGauge(float currentreload, float maxreload)
    {
        Debug.Log(currentreload);
        if (maxreload <= 0f)
        {
            _overImage.fillAmount = 0.4f;
            return;
        }
        float reloadProgress = 1f - currentreload / maxreload;

        _overImage.fillAmount =
            Mathf.Lerp(_reloadStartGauge, 0.4f, reloadProgress);
    }
}
