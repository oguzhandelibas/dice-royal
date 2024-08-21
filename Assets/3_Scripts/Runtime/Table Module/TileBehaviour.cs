using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject tileTopPlane;
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
    
    public void BlinkTileTopPlane(float blinkDuration = 0.1f)
    {
        StartCoroutine(BlinkCoroutine(blinkDuration));
    }

    private IEnumerator BlinkCoroutine(float blinkDuration)
    {
        tileTopPlane.SetActive(true);
        yield return new WaitForSeconds(blinkDuration);
        tileTopPlane.SetActive(false);
        yield return new WaitForSeconds(blinkDuration);
    }
}
