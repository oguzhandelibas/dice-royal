using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
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
    
    #region EVETN SUBSCRIPTION

    private void OnEnable()
    {
        PlayerSignals playerSignals = SO_Manager.Get<PlayerSignals>();
        playerSignals.PlayerInitialized += Initialize;
    }

    private void OnDisable()
    {
        PlayerSignals playerSignals = SO_Manager.Get<PlayerSignals>();
        playerSignals.PlayerInitialized -= Initialize;;
    }

    #endregion
}
