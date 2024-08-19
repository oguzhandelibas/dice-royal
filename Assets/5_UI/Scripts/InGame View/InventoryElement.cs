using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryElement : MonoBehaviour
{
    [SerializeField] private Image elementIcon;
    [SerializeField] private TextMeshProUGUI elementCount;

    public void Initialize(Sprite icon, int count)
    {
        elementIcon.sprite = icon;
        SetCount(count);
    }
    
    public void SetCount(int count)
    {
        elementCount.text = "X" + count.ToString();
    }
}