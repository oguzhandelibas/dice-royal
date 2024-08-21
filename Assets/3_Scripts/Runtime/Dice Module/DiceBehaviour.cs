using System.Collections;
using ODProjects.PoolModule.Signals;
using UnityEngine;

public class DiceBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    private Quaternion _desiredFinalRotation;
    public void Roll(Vector3 rollForce, Vector3 rollTorque, Quaternion desiredFinalRotation)
    {
        rb.AddForce(rollForce, ForceMode.Impulse);
        rb.AddTorque(rollTorque, ForceMode.Impulse);
        _desiredFinalRotation = desiredFinalRotation;
    }

    private void OnCollisionEnter(Collision other)
    {
        AudioManager.Instance.PlayAudioEffect(AudioType.DiceCollision);
        GameObject effect = EffectManager.Instance.GetEffect(EffectType.DiceCollision);
        effect.transform.position = transform.position;
        
        StartCoroutine(RollRoutine(_desiredFinalRotation));
    }

    private IEnumerator RollRoutine(Quaternion desiredFinalRotation)
    {
        while (rb.angularVelocity.magnitude > 6.0f)
        {
            yield return null;
        }
        
        float rotationSpeed = 3f;
        while (Quaternion.Angle(transform.rotation, desiredFinalRotation) > 0.01f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredFinalRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
        
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.rotation = desiredFinalRotation;
    }

}
