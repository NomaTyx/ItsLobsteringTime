using UnityEngine;
using UnityEngine.UI;

public class MoltWarning : MonoBehaviour
{
    [SerializeField] private Image _bar;

    public void TurnOnWarning()
    {
        gameObject.SetActive(true);
    }

    public void TurnOffWarning()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        _bar.fillAmount = PlayerEnergy.Instance.MoltTimerSeconds / PlayerEnergy.Instance.MoltWarningTime;
    }

    private void OnDestroy()
    {
        PlayerEnergy.PlayerMoltWarning -= TurnOnWarning;
    }
}
