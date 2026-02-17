using UnityEngine;

public class PlayerEnergy : MonoBehaviour
{
    [SerializeField] private int _maxEnergy = 100;
    [SerializeField] private float _energyDrainPerSecond = 5;
    [SerializeField] private float _playerGrowthIntervalSeconds = 100;

    private float _currentEnergy;

    private void Start()
    {
        _currentEnergy = _maxEnergy;
    }

    public void GainEnergy(float amount)
    {
        _currentEnergy = Mathf.Min(_currentEnergy + amount, _maxEnergy);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log($"Current energy: {_currentEnergy}");
        _currentEnergy -= _energyDrainPerSecond * Time.fixedDeltaTime;
    }
}
