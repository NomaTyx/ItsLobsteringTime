using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobsterWalker : MonoBehaviour
{
    [SerializeField] private GameObject[] _legTargets;
    [SerializeField] private GameObject[] _legCubes;
    [SerializeField] private GameObject _lobster;

    [SerializeField] private float _maxStepDistance;
    [SerializeField] private float _framesForLegToMove;
    [SerializeField] private float _velocitySmoothness;
    [SerializeField] private float _overStepMultiplier = 1.5f;
    [SerializeField] private int _framesBetweenSteps = 2;
    [SerializeField] private float _jitterCutoff = 0.1f;
    [SerializeField] private float _stepHeight = 0.1f;

    private bool _currentLeg = true;

    private Vector3[] _legPositions;
    private Vector3[] _originalLegPositions;

    private Vector3 _velocity;
    private Vector3 _lastLobsterPosition;
    private Vector3 _lastVelocity;

    //these lists exist to track which feet are moving and which feet are about to move
    private List<int> _oppositeLegIndex = new List<int>();
    private List<int> _nextIndexToMove = new List<int>();
    private List<int> _indexMoving = new List<int>();

    void Start()
    {
        _lastLobsterPosition = _lobster.transform.position;

        _legPositions = new Vector3[_legTargets.Length];
        _originalLegPositions = new Vector3[_legTargets.Length];

        for (int i = 0; i < _legTargets.Length; i++)
        {
            _legPositions[i] = _legTargets[i].transform.position;
            _originalLegPositions[i] = _legPositions[i];

            if(_currentLeg)
            {
                _oppositeLegIndex.Add(i + 1);
                _currentLeg = false;
            }
            else
            {
                _oppositeLegIndex.Add(i - 1);
                _currentLeg = true;
            }
        }
    }

    private void Update()
    {
        _velocity = _lobster.transform.position - _lastLobsterPosition;
        _velocity += _velocitySmoothness * _lastVelocity;
        _velocity /= _velocitySmoothness + 1;

        MoveLegs();

        _lastLobsterPosition = _lobster.transform.position;
        _lastVelocity = _velocity;
    }

    private void MoveLegs()
    {
        for(int i = 0; i < _legTargets.Length;i++)
        {
            if (Vector3.Distance(_legTargets[i].transform.position, _legCubes[i].transform.position) >= _maxStepDistance)
            {
                if(!_nextIndexToMove.Contains(i) && !_indexMoving.Contains(i))
                {
                    _nextIndexToMove.Add(i);
                }
            }
            else if (!_indexMoving.Contains(i)) 
            { 
                _legTargets[i].transform.position = _originalLegPositions[i];
            }
        }

        if (_nextIndexToMove.Count == 0 || _indexMoving.Count != 0) return; 

        Vector3 targetPosition = _legCubes[_nextIndexToMove[0]].transform.position;
        targetPosition += Mathf.Clamp(_velocity.magnitude * _overStepMultiplier, 0, 2) * (_legCubes[_nextIndexToMove[0]].transform.position - _legTargets[_nextIndexToMove[0]].transform.position) + (_velocity * _overStepMultiplier);
        StartCoroutine(Step(_nextIndexToMove[0], targetPosition, false));
    }

    private IEnumerator Step(int index, Vector3 moveTo, bool isOpposite)
    {
        if (!isOpposite) MoveOppositeLeg(_oppositeLegIndex[index]);

        _nextIndexToMove.Remove(index);

        if(!_indexMoving.Contains(index))
        {
            _indexMoving.Add(index);
        }

        Vector3 startingPosition = _originalLegPositions[index];

        for(int i = 1; i <= _framesForLegToMove; i++)
        {
            _legTargets[index].transform.position = Vector3.Lerp(startingPosition, moveTo + new Vector3(0, Mathf.Sin(i / (_framesForLegToMove * _jitterCutoff) * Mathf.PI) * _stepHeight, 0), i / _framesForLegToMove);
            yield return new WaitForEndOfFrame();
        }

        _originalLegPositions[index] = moveTo;

        for(int i = 0; i < _framesBetweenSteps; i++)
        {
            yield return new WaitForEndOfFrame();
        }

        _indexMoving.Remove(index);
    }

    private void MoveOppositeLeg(int index)
    {
        Vector3 targetPosition = _legCubes[index].transform.position;
        targetPosition = targetPosition + Mathf.Clamp(_velocity.magnitude * _overStepMultiplier, 0, 2) * (_legCubes[index].transform.position - _legTargets[index].transform.position) + (_velocity * _overStepMultiplier);
        StartCoroutine(Step(index, targetPosition, true));
    }
}
