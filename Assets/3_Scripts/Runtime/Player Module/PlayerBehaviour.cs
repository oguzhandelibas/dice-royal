using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    private Vector3[] _roadPositions;
    private int _currentIndex;
    public void Initialize(PlayerType type, Vector3[] positions)
    {
        _currentIndex = -1;
        _roadPositions = positions;
        GameObject player = playerData.playerPrefabs[type];
        Instantiate(player, transform.position, Quaternion.identity, transform);
        MoveTargetPosition(1);
    }

    private void MoveTargetPosition(int moveCount)
    {
        StartCoroutine(MoveSequentialPositions(moveCount));
    }

    private IEnumerator MoveSequentialPositions(int moveCount)
    {
        for (int i = 0; i < moveCount; i++)
        {
            yield return StartCoroutine(MoveNextPosition());
        }
        SO_Manager.Get<PlayerSignals>().MovementComplete?.Invoke(true);
    }

    private IEnumerator MoveNextPosition()
    {
        _currentIndex++;
        if(_currentIndex >= _roadPositions.Length)
        {
            _currentIndex = 0;
        }
        
        float elapsedTime = 0f;
        float duration = 0.5f;
        float jumpHeight = 1.0f; // Zıplama yüksekliği
        
        Vector3 originalScale = transform.localScale;
        Vector3 jumpScale = originalScale * 1.2f; // Zıplarken büyüme miktarı
        Vector3 targetPosition = _roadPositions[_currentIndex] + new Vector3(0,0.77f,0);
        Vector3 startPosition = transform.position;
        
        while (elapsedTime < duration)
        {
            float progress = elapsedTime / duration;
        
            // Pozisyon animasyonu: zıplama etkisi
            float heightOffset = Mathf.Sin(progress * Mathf.PI) * jumpHeight;
            transform.position = Vector3.Lerp(startPosition, targetPosition, progress) + Vector3.up * heightOffset;

            // Scale animasyonu: büyüyüp küçülme etkisi
            transform.localScale = Vector3.Lerp(originalScale, jumpScale, Mathf.Sin(progress * Mathf.PI));

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    
        // Son pozisyonu ve scale değerini garanti altına al
        transform.position = targetPosition;
        transform.localScale = originalScale;
    }


    
    #region EVETN SUBSCRIPTION

    private void OnEnable()
    {
        PlayerSignals playerSignals = SO_Manager.Get<PlayerSignals>();
        playerSignals.MoveTargetPosition += MoveTargetPosition;
    }

    private void OnDisable()
    {
        PlayerSignals playerSignals = SO_Manager.Get<PlayerSignals>();
        playerSignals.MoveTargetPosition -= MoveTargetPosition;
    }

    #endregion
}
