using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;

public class GlobalVFXManager : MonoBehaviour
{
    [SerializeField] private GameObject _damageTextPrefab;

    private void Start()
    {
        Health.Damaged += SpawnDamageNumber;
        Health.Damaged += CameraDamageBehavior;
    }

    private void OnDestroy()
    {
        Health.Damaged -= SpawnDamageNumber;
        Health.Damaged -= CameraDamageBehavior;
    }

    void SpawnDamageNumber(DamageInfo info)
    {
        GameObject text = Instantiate(_damageTextPrefab, GameManager.UICanvas.transform);
        text.transform.position = Camera.main.WorldToScreenPoint(info.Target.transform.position);
        text.GetComponent<TMP_Text>().SetText(info.Amount.ToString());

        RectTransform rect = text.GetComponent<RectTransform>();
        rect.anchoredPosition3D = new Vector3(rect.anchoredPosition.x, rect.anchoredPosition.y, 0);

    }

    void CameraDamageBehavior(DamageInfo info)
    {
        CameraShake.Instance.ShakeOnHit();
        StartCoroutine(HitStop());
    }

    private IEnumerator HitStop()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = 1;
    }
}
