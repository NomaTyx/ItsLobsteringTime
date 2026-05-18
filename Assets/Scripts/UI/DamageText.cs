using System.Collections;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private float _lifetimeSeconds = 1f;
    private Color _newColor;
    private TMP_Text _text;

    private void Start()
    {
        Destroy(gameObject, _lifetimeSeconds);
        _text = GetComponent<TMP_Text>();
        _newColor = _text.color;
        StartCoroutine(RenderText());
        Debug.Log("spawned");
    }

    private IEnumerator RenderText()
    {
        float time = 0;

        while(true)
        {
            time += Time.deltaTime;

            transform.localScale *= 1.001f;
            transform.Translate(new Vector3(0, 1f, 0));

            _newColor.a = Mathf.Lerp(1.0f, 0, time / _lifetimeSeconds);
            _text.color = _newColor;

            yield return null;
        }
    }
}
