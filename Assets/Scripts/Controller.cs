using Unity.VisualScripting;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    [SerializeField] protected GameObject _target;

    public float Size { get; protected set; }

    public void SetTarget(Controller target)
    {
        _target = target.gameObject;
    }
}