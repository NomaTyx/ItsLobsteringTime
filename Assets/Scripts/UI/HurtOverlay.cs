using UnityEngine;

public class HurtOverlay : MonoBehaviour
{
    [SerializeField] private float _hurtOverlayDuration = 0.5f;
    CanvasGroup _canvasGroup;

    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Display()
    {
        _canvasGroup.alpha = 1;
    }

    void FixedUpdate()
    {
        if(_canvasGroup.alpha == 0) return;
        _canvasGroup.alpha -= Time.fixedDeltaTime / _hurtOverlayDuration;
    }
}
