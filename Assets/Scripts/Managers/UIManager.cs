using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public Image[] lifes = new Image[3];
    public int comboNeeded;

    public TMP_Text combo, comboFill;
    public RectMask2D mask;
    public Color[] _comboColors;
    public int currentCombo;
    private float _currentComboPart;

    private void Awake()
    {
        if(Instance)
            Destroy(gameObject);
        Instance = this;
    }

    private void Start()
    {
        currentCombo = 0;
        SetCombo(1);
    }

    private void Update()
    {
        mask.padding = new Vector4(0, 0, 0, Mathf.Lerp(125f, 0f, _currentComboPart / comboNeeded));
        _currentComboPart = Mathf.Clamp(_currentComboPart - Time.deltaTime, 0, float.MaxValue);

        if (_currentComboPart <= 0f && currentCombo > 1)
        {
            ResetCombo();
        }
    }

    public void UpdateHealthSlot(int health)
    {
        ResetCombo();
        lifes[health - 1].transform.DOScale(0, 0.25f).SetEase(Ease.InBack).OnComplete(() =>
        {
            lifes[health - 1].transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        });
    }

    public void SetCombo(int comboValue)
    {
        currentCombo = Math.Clamp(comboValue, 1, _comboColors.Length);
        combo.text = $"x{currentCombo}";
        comboFill.text = $"x{currentCombo}";
        combo.DOColor(_comboColors[currentCombo - 1], 0.25f);
        comboFill.DOColor(_comboColors[currentCombo - 1], 0.25f);
        combo.transform.localScale = Vector3.one * 1.2f;
        combo.transform.DOScale(1f, 0.25f);
    }

    public void ResetCombo()
    {
        SetCombo(1);
        combo.transform.localScale = Vector3.one * 0.5f;
        combo.transform.DOScale(1f, 1.5f);
    }

    public void AddComboPart()
    {
        Camera.main.DOShakePosition(0.1f, 0.12f).OnComplete(() => Camera.main.transform.position = new Vector3(0, 0, -10));
        
        _currentComboPart += 1f;
        if (_currentComboPart >= comboNeeded)
        {
            _currentComboPart = 1.5f;
            SetCombo(currentCombo + 1);
        }
    }
}
