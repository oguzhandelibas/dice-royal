using System.Collections;
using System.Collections.Generic;
using LevelEditor;
using Unity.Mathematics;
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
        transform.rotation = quaternion.Euler(0,0,0);
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
        
        if (tileData.SelectedElement != SelectedElement.Null)
        {
            CollectInventoryItem(tileData);
        }
        else
        {
            AudioManager.Instance.PlayAudioEffect(AudioType.EmptyTile);
        }
    }

    private void CollectInventoryItem(TileData tileData)
    {
        SO_Manager.Get<InventorySignals>().AddInventoryElement?.Invoke(tileData.SelectedElement, tileData.ElementCount);
        AudioManager.Instance.PlayAudioEffect(AudioType.InventoryCollect);
            
        GameObject effect = EffectManager.Instance.GetEffect(EffectType.CollectFlash);
        effect.transform.position = transform.position + Vector3.up;
        effect.transform.rotation = Quaternion.Euler(0, 0, 0);
            
        GameObject confetti1 = EffectManager.Instance.GetEffect(EffectType.Confetti);
        GameObject confetti2 = EffectManager.Instance.GetEffect(EffectType.Confetti);
        confetti1.transform.position = transform.position + new Vector3(1,-0.5f, 0);
        confetti2.transform.position = transform.position + new Vector3(-1,-0.5f, 0);
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
        
        while (elapsedTime < duration)
        {
            float progress = elapsedTime / duration;
            float heightOffset = Mathf.Sin(progress * Mathf.PI) * jumpHeight;
            transform.position = Vector3.Lerp(startPosition, targetPosition, progress) + Vector3.up * heightOffset;
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
        Quaternion targetRotation = quaternion.Euler(0,0,0);
        switch (_playerDirection)
        {
            case PlayerDirection.Left:
                targetRotation = Quaternion.Euler(0, -90, 0);
                break;
            case PlayerDirection.Right:
                targetRotation = Quaternion.Euler(0, 90, 0);
                break;
            case PlayerDirection.Forward:
                targetRotation = Quaternion.Euler(0, 0, 0);
                break;
            case PlayerDirection.Backward:
                targetRotation = Quaternion.Euler(0, 180, 0);
                break;
        }
        transform.rotation = targetRotation;
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