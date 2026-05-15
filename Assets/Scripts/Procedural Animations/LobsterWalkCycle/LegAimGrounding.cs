using UnityEngine;

public class LegAimGrounding : MonoBehaviour
{
    private GameObject raycastOrigin;
    private int _layerMask;

    private void Start()
    {
        raycastOrigin = transform.parent.gameObject;
        _layerMask = LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        if(Physics.Raycast(raycastOrigin.transform.position, -transform.up, out RaycastHit hit, Mathf.Infinity, _layerMask))
        {
            transform.position = hit.point;
        }
    }
}