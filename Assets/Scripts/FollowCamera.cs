using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject player;

    public CharacterBehavior characterBehavior;

    [Range(-50f, 50f)]
    public float xOffset = 0.0f;

    [Range(-50f, 50f)]
    public float yOffset = 0.0f;

    [Range(-50f, 50f)]
    public float zOffset = 0.0f;

    [Range(0.01f, 20f)]
    public float smoothing = 5f;

    private Vector3 _offsetVector;
    private Vector3 playerPosition = Vector3.zero;
    private Vector3 _currentPosition;
    private Vector3 _fromPosition = Vector3.zero;
    private Vector3 _targetPosition;
    private float _t = 0f;

    private Quaternion _targetRotation;
    private Quaternion _initialRotation;
    private Quaternion _fromRotation;
    private Quaternion _currentRotation;
    private float _animTime = 0;
    private bool _animateOffset = false;
    private bool _animateRotation = false;
    private float _animationLength = 1f;

    private void Start()
    {
        characterBehavior = player.GetComponent<CharacterBehavior>();
        _offsetVector = new Vector3(xOffset, yOffset, zOffset);
        _initialRotation = transform.rotation;
    }
    void Update()
    {
        // Handle current animation state
        if (_animateRotation || _animateOffset)
        {
            _animTime += Time.deltaTime;
            float _animT = Mathf.Clamp(_animTime / _animationLength, 0, 1);

            if (_animateRotation)
            {
                _currentRotation = Quaternion.Slerp(_fromRotation, _targetRotation, _animT);
                transform.rotation = _currentRotation;
            }

            if (_animateOffset)
            {
                _currentPosition = Vector3.Lerp(_fromPosition, _targetPosition, _animT);
                transform.position = _currentPosition;
            }

            if (_animT == 1)
            {
                _animateOffset = false;
                _animateRotation = false;
            }
        }

        // Handle the typical camera  -> player offset, lerping the y position based off the player's grounded position
        playerPosition = player.transform.position;
        Vector3 targetPos = new Vector3(_offsetVector.x + playerPosition.x, characterBehavior.GetLastGroundedPosition() + _offsetVector.y, _offsetVector.z + playerPosition.z);

        if (!_animateOffset)
        {
            _targetPosition = targetPos;

            float t = Time.deltaTime * smoothing;
            _t = Mathf.Clamp(t, 0, 1);

            _currentPosition = Vector3.Lerp(transform.position, _targetPosition, _t);

            transform.position = _currentPosition;
        }
    }

    public void RotateCamera(Quaternion targetRotation, float animationLength)
    {
        _animTime = 0;
        _targetRotation = targetRotation;
        _fromRotation = transform.rotation;
        _animationLength = animationLength;
        _animateRotation = true;
    }

    public void ResetCameraRotation(float animationLength)
    {
        RotateCamera(_initialRotation, animationLength);
    }

    public void ChangeOffsetPostion(Vector3 offset, float animationLength)
    {
        Vector3 playerPosition = player.transform.position;
        _animTime = 0;
        _offsetVector = offset;
        _targetPosition = playerPosition + offset;
        _fromPosition = transform.position;
        _animationLength = animationLength;
        _animateOffset = true;
        _t = 0;
    }

    public void ResetOffsetPosition(float animationLength)
    {
        Vector3 playerPosition = player.transform.position;
        _animTime = 0;
        _offsetVector = new Vector3(xOffset, yOffset, zOffset);
        _targetPosition = _offsetVector + playerPosition;
        _fromPosition = transform.position;
        _animationLength = animationLength;
        _animateOffset = true;
        _t = 0;
    }

    private float Evaluate(float x)
    {
        return 0.5f * Mathf.Sin(x - Mathf.PI / 2f) + 0.5f;
    }
}
