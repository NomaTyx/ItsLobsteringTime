using Unity.VisualScripting;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    [SerializeField] private Controller _target;

    public void SetTarget(Controller target)
    {
        _target = target;
    }
}
