using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanelView: View
{
    [SerializeField] private Slider loadingBar;
    [SerializeField] private TextMeshProUGUI loadingBarText;
    [SerializeField] private Transform loadingIcon;
    public bool active = false;
    
    public override void Initialize()
    {
        StartCoroutine(LoadingRoutine());
    }
    
    private IEnumerator LoadingRoutine()
    {
        if (active)
        {
            const float progressSpeed = 0.7f;
            const float targetValue = 0.95f;
            const float rotationSpeed = -1f;

            loadingBar.value = 0;

            while (loadingBar.value < targetValue)
            {
                loadingIcon.Rotate(0, 0, rotationSpeed);

                loadingBar.value = Mathf.Lerp(loadingBar.value, 1f, progressSpeed * Time.deltaTime);
                loadingBarText.text = $"{(int)(loadingBar.value * 100)}%";
                yield return null;
            }

            loadingBar.value = 1;
            loadingBarText.text = "100%";
        }

        UIManager.Instance.Show<InGamePanelView>();
    }

    
    #region EVENT SUBSCRIPTIONS
    
    private GameSignals _gameSignals;
    private void OnEnable()
    {
        _gameSignals = SO_Manager.Get<GameSignals>();
        _gameSignals.OnGameInitialize += Initialize;
    }
    
    private void OnDisable()
    {
        _gameSignals.OnGameInitialize -= Initialize;
    }

    #endregion
}