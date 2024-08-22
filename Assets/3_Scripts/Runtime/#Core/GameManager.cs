using ODProjects;
using UnityEngine;

public class GameManager : AbstractSingleton<GameManager>
{
    [HideInInspector] public bool isGameStarted = false;
    private void Start()
    {
        UIManager.Instance.Show<LoadingPanelView>();
        SO_Manager.Get<GameSignals>().OnGameInitialize?.Invoke();
    }
}