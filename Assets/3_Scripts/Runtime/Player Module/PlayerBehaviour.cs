using System.Collections;
using System.Collections.Generic;
using LevelEditor;
using ODProjects.PoolModule.Signals;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private List<TileData> _tileDatas;
    private int _currentIndex;
    private PlayerDirection _playerDirection;

    public void Initialize(PlayerType type, List<TileData> tileDatas)
    {
        _currentIndex = -1;
        _tileDatas = tileDatas;
        GameObject player = SO_Manager.Get<PlayerData>().playerTypes[type].dummyPrefab;
        Instantiate(player, transform.position + Vector3.up*.1f, Quaternion.identity, transform);
        StartCoroutine(MoveSequentialPositions(1, false));
    }

    private void MoveTargetPosition(int moveCount)
    {
        StartCoroutine(MoveSequentialPositions(moveCount));
    }

    private IEnumerator MoveSequentialPositions(int moveCount, bool collectElement = true)
    {
        for (int i = 0; i < moveCount; i++)
        {
            yield return StartCoroutine(MoveNextPosition());
        }

        if (!collectElement) yield break;
        SO_Manager.Get<PlayerSignals>().MovementComplete?.Invoke(true);
        var tileData = _tileDatas[_currentIndex];
        ;
        InventorySignals inventorySignals = SO_Manager.Get<InventorySignals>();
        if (tileData.SelectedElement != SelectedElement.Null)
        {
            inventorySignals.AddInventoryElement?.Invoke(tileData.SelectedElement, tileData.ElementCount);
            Debug.Log("Movement completed: " + tileData.SelectedElement + " " + tileData.ElementCount);
            AudioManager.Instance.PlayAudioEffect(AudioType.InventoryCollect);
            
            GameObject effect = EffectManager.Instance.GetEffect(EffectType.CollectFlash);
            effect.transform.position = transform.position + Vector3.up;
            effect.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            AudioManager.Instance.PlayAudioEffect(AudioType.EmptyTile);
        }
    }

    private int GetCurrentIndex()
    {
        _currentIndex++;
        if (_currentIndex >= _tileDatas.Count)
        {
            _currentIndex = 0;
            AudioManager.Instance.PlayAudioEffect(AudioType.DummyFly);
        }

        return _currentIndex;
    }

    private IEnumerator MoveNextPosition()
    {
        float elapsedTime = 0f;
        float duration = 0.5f;
        float jumpHeight = 1.0f;
        
        TileData tileData = _tileDatas[GetCurrentIndex()];
        
        Vector3 originalScale = transform.localScale;
        Vector3 jumpScale = originalScale * 1.2f;
        Vector3 targetPosition = tileData.Position + new Vector3(0, 0.4f, 0);
        Vector3 startPosition = transform.position;

        Vector3 difference = targetPosition - startPosition;
        CalculatePlayerDirection(difference);
        AudioManager.Instance.PlayAudioEffect(AudioType.DummyJump);
        
        GameObject effect = EffectManager.Instance.GetEffect(EffectType.JumpEffect);
        effect.transform.position = transform.position+(Vector3.up*.15f);
        effect.transform.rotation = Quaternion.Euler(90,0,0);
        
        while (elapsedTime < duration)
        {
            float progress = elapsedTime / duration;

            // Pozisyon animasyonu: zıplama etkisi
            float heightOffset = Mathf.Sin(progress * Mathf.PI) * jumpHeight;
            transform.position = Vector3.Lerp(startPosition, targetPosition, progress) + Vector3.up * heightOffset;

            // büyüyüp küçülme etkisi
            transform.localScale = Vector3.Lerp(originalScale, jumpScale, Mathf.Sin(progress * Mathf.PI));

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        AudioManager.Instance.PlayAudioEffect(AudioType.DummyLand);
        tileData.TileBehaviour.BlinkTileTopPlane();
        
        transform.position = targetPosition;
        transform.localScale = originalScale;
    }

    private void CalculatePlayerDirection(Vector3 difference)
    {
        _playerDirection = PlayerDirection.Forward;
        if (difference.magnitude < 0.1f) _playerDirection = PlayerDirection.Forward;
        if (Mathf.Abs(difference.x) > Mathf.Abs(difference.z)) _playerDirection = difference.x > 0 ? PlayerDirection.Right : PlayerDirection.Left;
        else _playerDirection = difference.z > 0 ? PlayerDirection.Forward : PlayerDirection.Backward;
    }

    private Transform GetPlayerTransform() => transform;
    private PlayerDirection GetPlayerDirection() => _playerDirection;

    #region EVETN SUBSCRIPTION

    private void OnEnable()
    {
        PlayerSignals playerSignals = SO_Manager.Get<PlayerSignals>();
        playerSignals.MoveTargetPosition += MoveTargetPosition;
        playerSignals.GetPlayerTransform += GetPlayerTransform;
        playerSignals.GetPlayerDirection += GetPlayerDirection;
    }

    private void OnDisable()
    {
        PlayerSignals playerSignals = SO_Manager.Get<PlayerSignals>();
        playerSignals.MoveTargetPosition -= MoveTargetPosition;
        playerSignals.GetPlayerTransform -= GetPlayerTransform;
        playerSignals.GetPlayerDirection -= GetPlayerDirection;
    }

    #endregion
}