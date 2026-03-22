using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HealthBar : MonoBehaviour
{
    private Image _healthBarImage;

    private void Start()
    {
        _healthBarImage = GetComponent<Image>();
    }
}
