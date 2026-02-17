using System;
using UnityEngine;

[Serializable]
public class Food : MonoBehaviour
{
    [SerializeField] private float _energyValue = 10f;

    public void OnEaten()
    {
        GameManager.instance.Player.GetComponent<PlayerEnergy>().GainEnergy(_energyValue);
        Destroy(gameObject);
    }
}
