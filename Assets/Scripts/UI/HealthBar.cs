using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthBarImage;
    [SerializeField] private Image _energyUIIndicator;

    private void Update()
    {
        _healthBarImage.fillAmount = PlayerEnergyManager.Instance.Energy / PlayerEnergyManager.Instance.MaxEnergy;
    }
}
