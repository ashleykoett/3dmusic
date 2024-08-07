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

    protected bool _followActive = true;
    protected Vector3 _initialPosition;
    protected Vector3 _followPosition;
    protected Vector3 _targetPosition;
    protected Vector3 _dir;
    protected float _distance;
    protected bool _nearOrigin = false;

    private void OnEnable()
    {
        DismissFollowers.OnDismiss += Unfollow;
        ActivateFollowers.OnActivateFollowers += ActivateFollow;
        
    }

    private void OnDisable()
    {
        DismissFollowers.OnDismiss -= Unfollow;
        ActivateFollowers.OnActivateFollowers -= ActivateFollow;
    }

    public virtual void Start()
    {
        characterBehavior = GetComponent<CharacterBehavior>();
        _initialPosition = transform.position;

        _followActive = startActive;
    }

    public virtual void Update()
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

    public void ActivateFollow(string group)
    {
        if(groupName == group)
        {
            SetFollowActive(true);
        }
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
