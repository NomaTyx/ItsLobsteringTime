using Unity.VisualScripting;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    [SerializeField] protected GameObject _target;

    public void SetTarget(Controller target)
    {
        _target = target.gameObject;
    }
}