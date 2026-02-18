using Unity.VisualScripting;
using UnityEngine;

public class PlayerEnergy : MonoBehaviour
{
    [SerializeField] private int _maxEnergy = 100;
    [SerializeField] private float _energyDrainPerSecond = 5;
    [SerializeField] private float _playerGrowthIntervalSeconds = 100;
    [SerializeField] private float[] _playerGrowthEnergyCosts;

    private float _currentEnergy;

    private void Start()
    {
        _currentEnergy = _maxEnergy;

        GameManager.Instance.MoltTimerExpired += Grow;
    }

    public void GainEnergy(float amount)
    {
        _currentEnergy = Mathf.Min(_currentEnergy + amount, _maxEnergy);
    }

    private void Grow()
    {

    }

    private void Die()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //for the love of GOD please remember to change this later
        Debug.Log($"Current energy: {_currentEnergy}");
        _currentEnergy -= _energyDrainPerSecond * Time.fixedDeltaTime;
    }
}
