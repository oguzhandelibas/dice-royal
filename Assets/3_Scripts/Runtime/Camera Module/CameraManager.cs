using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private float shakeMagnitude = 2.0f;
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private Vector2 XZ_Offset;
    private Transform _cameraTransform;
    private Transform _target; 
    
    private void Initialize(Transform target)
    {
        if (Camera.main) _cameraTransform = Camera.main.transform;
        _target = target;
    }

    private void LateUpdate()
    {
        if(!_cameraTransform || !_target) return;
        Vector3 targetPosition = new Vector3(
                                     _target.position.x, _cameraTransform.position.y, _target.position.z) 
                                 + new Vector3(XZ_Offset.x,0,XZ_Offset.y);
        _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, targetPosition, followSpeed * Time.deltaTime);
    }

    private void CameraShake()
    {
        StartCoroutine(Shake(shakeDuration, shakeMagnitude));
    }

    private IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPosition = _cameraTransform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetZ = Random.Range(-1f, 1f) * magnitude;

            _cameraTransform.position = new Vector3(originalPosition.x + offsetX, originalPosition.y, originalPosition.z + offsetZ);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        _cameraTransform.position = originalPosition;
    }

    #region EVENT SUBSCRIPTION

    private void OnEnable()
    {
        SO_Manager.Get<CameraSignals>().ShakeCamera += CameraShake;
        PlayerSignals playerSignals = SO_Manager.Get<PlayerSignals>();
        playerSignals.PlayerInitialized += Initialize;
    }

    private void OnDisable()
    {
        SO_Manager.Get<CameraSignals>().ShakeCamera -= CameraShake;
        PlayerSignals playerSignals = SO_Manager.Get<PlayerSignals>();
        playerSignals.PlayerInitialized -= Initialize;
    }

    #endregion
}