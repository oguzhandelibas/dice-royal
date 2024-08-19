using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryElementBehaviour : MonoBehaviour
{
    [SerializeField] private Image elementIcon;
    [SerializeField] private TextMeshProUGUI elementCount;
    private int _currentCount = 0;

    public void Initialize(Sprite icon, int count)
    {
        elementIcon.sprite = icon;
        AddElementCount(count);
    }

    public void AddElementCount(int count)
    {
        _currentCount += count;
        elementCount.text = "x" + _currentCount;
    }
}