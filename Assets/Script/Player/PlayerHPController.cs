using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHPController : MonoBehaviour
{
    [SerializeField] int _currentHealItem;
    [SerializeField]    int _maxHealItem;
    [SerializeField] int _maxHP;
    [SerializeField] Image _hpImage;
    [SerializeField] Text _currrentText;
    [SerializeField] Text _maxText;
    [SerializeField] float _damageInterval;
    [SerializeField]    Text _healCntText;
    public int _currentHP;
    bool _isHealing;
    bool _canTakeDamage = true;

    private void Awake()
    {
        _currentHP = _maxHP;
        SceneManager.sceneLoaded += OnsceneLoaded;
    }
    void OnsceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateUIIncrease(_currentHP);
        _currentHP = _maxHP;
        _currentHealItem = _maxHealItem;
        UpdateUIHealCnt();
    }

    void Start()
    {

    }

    void Update()
    {

    }
    public void EnhanceHP(int addHP)
    {
        _maxHP += addHP;
        _maxText.text = _maxHP.ToString();
    }
    public void PlayerDamage(int damage)
    {
        if (_isHealing) { return; }
        if (!_canTakeDamage) { return; }
        _currentHP -= damage;
        _currentHP = Mathf.Clamp(_currentHP, 0, _maxHP);
        UpdateUIDecrease();
        StartCoroutine(DamageCoroutine());
    }
    public void PlayerHeal()
    {
        if (_currentHealItem <= 0) { return; }
        if (_currentHP >= _maxHP) { return; }
        _currentHealItem--;
        UpdateUIHealCnt();
        UpdateUIIncrease(_currentHP);
        _currentHP = _maxHP;
    }
    public void UpdateUIHealCnt()
    {
        _healCntText.text = _currentHealItem.ToString();
    }
    public void UpdateUIIncrease(int tempHp)
    {
        _isHealing = true;
        DOTween.To(
            () => _hpImage.fillAmount,
            (float newfillamount) =>
            {
                _hpImage.fillAmount = newfillamount;
            },
            1,
            0.5f
            );
        DOTween.To(
            () => tempHp,
            (int addHP) =>
            {
                _currrentText.text = addHP.ToString();
            },
            _maxHP,
            0.5f
            ).OnComplete(
            () =>
            {
                _isHealing = false;
            });
    }
    public void UpdateUIDecrease()
    {
        _currrentText.text = _currentHP.ToString();
        _hpImage.fillAmount = (float)_currentHP / _maxHP;

    }
    IEnumerator DamageCoroutine()
    {
        _canTakeDamage = false;
        yield return new WaitForSeconds(_damageInterval);
        _canTakeDamage = true;
    }
}
