using UnityEngine;
using Unity.Cinemachine;

public class CameraShake : MonoBehaviour
{
    private CinemachineImpulseSource _impulseSource;

    private static CameraShake _instance;
    public static CameraShake Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    private void Start()
    {
        _impulseSource = PlayerEnergy.Instance.gameObject.GetComponent<CinemachineImpulseSource>();
    }

    public void ShakeOnHit(float force = 0.1f)
    {
        _impulseSource.GenerateImpulseWithForce(force);
    }
}