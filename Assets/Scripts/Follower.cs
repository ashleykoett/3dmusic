using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public CharacterBehavior characterBehavior;
    public GameObject target;
    public float closeDistance = 1f;
    public float followDistance = 100f;
    public bool startActive = true;
    public string groupName = "";

    private bool _followActive = true;
    private Vector3 _initialPosition;
    private Vector3 _followPosition;
    private Vector3 _targetPosition;
    private Vector3 _dir;
    private float _distance;
    private bool _nearOrigin = false;

    private void OnEnable()
    {
        DismissFollwers.OnDismiss += Unfollow;
    }

    private void OnDisable()
    {
        DismissFollwers.OnDismiss -= Unfollow;
    }

    private void Start()
    {
        characterBehavior = GetComponent<CharacterBehavior>();
        _initialPosition = transform.position;
    }

    public void Update()
    {
        if (!_followActive && _nearOrigin)
        {
            return;
        }

        _targetPosition = target.transform.position;
        _distance = Vector3.Distance(_targetPosition, transform.position);

        // Move towards player
        if (_distance <= followDistance && _followActive)
        {
            _nearOrigin = false;
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
            _targetPosition = _initialPosition;
            _distance = Vector3.Distance(_targetPosition, transform.position);

            if (_distance <= closeDistance)
            {
                _dir = Vector3.zero;
                _nearOrigin = true;
            }
            else
            {
                _nearOrigin = false;
                _followPosition = _initialPosition;
                _dir = _dir = (_followPosition - transform.position).normalized;
                _dir.y = 0;
            }
        }

        characterBehavior.MoveCharacter(_dir);
    }

    public void Unfollow(string group)
    {
        if(groupName == group)
        {
            SetFollowActive(false);
        }
    }

    public void SetFollowActive(bool active)
    {
        _followActive = active;
    }
}
