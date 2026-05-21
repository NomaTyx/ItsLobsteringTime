using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StarvingWarning : MonoBehaviour
{
    [SerializeField] private Image _starvingBarImage;

    [SerializeField] private float _textOscillationAmplitude = 1;
    [SerializeField] private float _textOscillationFrequency = 2.5f;
    [SerializeField] private float _fadeTimeSeconds = 0.5f;

    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private bool _fadingIn = false;
    private bool _fadingOut = false;
    private Coroutine _animating;
    private float _timeStarving;

    private void Start()
    {
        PlayerEnergy.PlayerStarving += ToggleWarning;
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnDestroy()
    {
        PlayerEnergy.PlayerStarving -= ToggleWarning;

    }

    private void ToggleWarning(bool warning)
    {
        if (_fadingIn && !warning || _fadingOut && warning)
        {
            StopCoroutine(_animating);
        }
        if (warning)
        {
            StartCoroutine(StartWarning());
        }
        else
        {
            StartCoroutine(StopWarning());
        }
    }

    private IEnumerator StartWarning()
    {
        _fadingIn = true;
        float elapsedTime = 0f;
        _timeStarving = 0;

        while (elapsedTime < _fadeTimeSeconds)
        {
            elapsedTime += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Clamp01(elapsedTime / _fadeTimeSeconds);
            yield return null;
        }
        _fadingIn = false;
    }

    private IEnumerator StopWarning()
    {
        _fadingOut = true;
        float elapsedTime = 0f;

        while (elapsedTime < _fadeTimeSeconds)
        {
            elapsedTime += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Clamp01(1 - elapsedTime / _fadeTimeSeconds);
            yield return null;
        }
        _fadingOut = false;
    }

    void FixedUpdate()
    {
        //i don't know if this is how i should be doing it.
        if (Mathf.Approximately(_canvasGroup.alpha, 0)) return;
        _timeStarving += Time.fixedDeltaTime;
        float newY = _rectTransform.anchoredPosition.y + Mathf.Sin(Time.time * _textOscillationFrequency) * _textOscillationAmplitude;
        _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x, newY);
        _starvingBarImage.fillAmount = 1 - _timeStarving / PlayerEnergy.Instance.StarvationTime;
    }
}
