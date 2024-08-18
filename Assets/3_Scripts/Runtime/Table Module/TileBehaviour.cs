using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileBehaviour : MonoBehaviour
{
    [SerializeField] private Image tileImage;
    [SerializeField] private TextMeshProUGUI indexText;
    [SerializeField] private TextMeshProUGUI elementCountText;
    
    public void InitializeTile(Sprite sprite, int index, int elementCount, bool isEmpty = false)
    {
        indexText.text = index.ToString();
        if (isEmpty)
        {
            tileImage.gameObject.SetActive(false);
            elementCountText.gameObject.SetActive(false);
            return;
        }
        tileImage.sprite = sprite;
        
        elementCountText.text = elementCount.ToString();
    }
}
