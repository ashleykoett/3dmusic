using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public CharacterBehavior characterBehavior;
    public GameObject target;
    public float closeDistance = 1f;
    public float followDistance = 100f;

    private Vector3 _initialPosition;
    private Vector3 _followPosition;
    private Vector3 _targetPosition;
    private Vector3 _dir;
    private float _distance;

    private void Start()
    {
        characterBehavior = GetComponent<CharacterBehavior>();
        _initialPosition = transform.position;
        // GetComponent<Renderer>().material.color = color;
    }

    public void Update()
    {
        _targetPosition = target.transform.position;
        _distance = Vector3.Distance(_targetPosition, transform.position);

        // Move towards player
        if (_distance <= followDistance)
        {
            if(_distance <= closeDistance)
            {
                _dir = Vector3.zero;
            }
            else
            {
                _followPosition = _targetPosition;
                _dir = (_followPosition - transform.position).normalized;
                _dir.y = 0;
            }
        }

        // Move towards target
        else
        {
            if (_distance <= closeDistance)
            {
                _dir = Vector3.zero;
            }
            else
            {
                _followPosition = _initialPosition;
                _dir = _dir = (_followPosition - transform.position).normalized;
                _dir.y = 0;
            }
        }

        characterBehavior.MoveCharacter(_dir);
    }
}
