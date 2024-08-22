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
            loadingBar.value = 0;
            float progressSpeed = 0.7f;
            while (loadingBar.value < 0.95f)
            {
                loadingIcon.Rotate(0, 0, -1);
            
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

    private void OnEnable()
    {
        SO_Manager.Get<GameSignals>().OnGameInitialize += Initialize;
    }
    
    private void OnDisable()
    {
        SO_Manager.Get<GameSignals>().OnGameInitialize -= Initialize;
    }

    #endregion
}