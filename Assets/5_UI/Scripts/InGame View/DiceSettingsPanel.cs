using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DiceSettingsPanel : MonoBehaviour
{
    [SerializeField] private RectTransform settingsPanel;
    [SerializeField] private Dropdown diceCountDropdown;
    [SerializeField] private Dropdown diceIndexDropdown;
    [SerializeField] private Dropdown diceValueDropdown;

    private DiceData _diceData;
    private int _currentIndex = 0;

    private void Start()
    {
        settingsPanel.sizeDelta = new Vector2(0, settingsPanel.rect.height);
        _diceData = SO_Manager.Get<DiceData>();
        
        Initialize();
    }

    private void Initialize()
    {
        SetDiceCountDropdown(50);
        SetDiceValueDropdown(6);
        
        if (_diceData.diceValues.Length != 0)
        {
            // var olan bilgiler işlenecek
            int diceCount = _diceData.diceValues.Length;
            
            SetDiceIndexDropdown(diceCount);
            
            diceCountDropdown.value = diceCount-1;
            diceIndexDropdown.value = 0;
            diceValueDropdown.value = (int)_diceData.diceValues[0];
            // ilk indexteki elemanın value'su setlenecek
        }
        else
        {
            _diceData.CreateRandomDiceValues(1);
            SetDiceIndexDropdown(1);
            diceValueDropdown.value = (int)_diceData.diceValues[0];
        }
        
        
        diceCountDropdown.onValueChanged.AddListener(OnDiceCountClick);
        diceIndexDropdown.onValueChanged.AddListener(OnDiceIndexClick);
        diceValueDropdown.onValueChanged.AddListener(OnDiceValueClick);
    }

    #region Listeners

    private void OnDiceCountClick(int value)
    {
        value++;
        _diceData.CreateEmptyDiceValues(value);
        SetDiceIndexDropdown(value);
        diceValueDropdown.value = (int)_diceData.diceValues[0];
    }
    
    private void OnDiceIndexClick(int value)
    {
        _currentIndex = value;
        diceValueDropdown.value = (int)_diceData.diceValues[value];
    }
    
    private void OnDiceValueClick(int value)
    {
        _diceData.SetDiceValue(_currentIndex, value);
    }

    #endregion

    #region Dropdown Initialization

    private void SetDiceCountDropdown(int value)
    {
        SetDropdown(diceCountDropdown, value);
    }
    
    private void SetDiceIndexDropdown(int value)
    {
        SetDropdown(diceIndexDropdown, value);
        
    }
    
    private void SetDiceValueDropdown(int value)
    {
        SetDropdown(diceValueDropdown, value);
    }
    
    private void SetDropdown(Dropdown dropdown, int value)
    {
        dropdown.options.Clear();
        for (int i = 0; i < value; i++)
        {
            var option = new Dropdown.OptionData((i+1).ToString());
            dropdown.options.Add(option);
        }
        dropdown.value = 0;
    }

    #endregion

    #region Settings Panel Animation
    
    private Coroutine _resizeCoroutine;
    private bool _settingsPanelActiveness = false;
    
    public void _ToggleSettingsPanel()
    {
        _settingsPanelActiveness = !_settingsPanelActiveness;
        if (_resizeCoroutine != null)
        {
            StopCoroutine(_resizeCoroutine);
        }
        
        _resizeCoroutine = StartCoroutine(ResizePanel(_settingsPanelActiveness ? 380 : 0));
    }

    private IEnumerator ResizePanel(float targetWidth)
    {
        while (Mathf.Abs(settingsPanel.sizeDelta.x - targetWidth) > 0.01f)
        {
            float newWidth = Mathf.MoveTowards(settingsPanel.sizeDelta.x, targetWidth, 1000 * Time.deltaTime);
            settingsPanel.sizeDelta = new Vector2(newWidth, settingsPanel.sizeDelta.y);;
            yield return null;
        }
        settingsPanel.sizeDelta = new Vector2(targetWidth, settingsPanel.rect.height);
    }

    #endregion

    private void OnDestroy()
    {
        diceCountDropdown.onValueChanged.RemoveListener(OnDiceCountClick);
        diceIndexDropdown.onValueChanged.RemoveListener(OnDiceIndexClick);
        diceValueDropdown.onValueChanged.RemoveListener(OnDiceValueClick);
    }
}