using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerBehavior : CharacterBehavior
{
    public Spin spin;
    private bool _moving = false;

    private void Start()
    {
        spin = GetComponentInChildren<Spin>();
    }
    public override void MoveCharacter(Vector3 moveVelocity, bool jumpEnabled = true)
    {
        if(moveVelocity != Vector3.zero && !_moving) 
        {
            _moving = true;
            soundController.FadeInSound();
        }

        if(moveVelocity == Vector3.zero && _moving)
        {
            soundController.FadeOutSound();
            _moving = false;
        }

        base._grounded = controller.isGrounded;

        // Stop player from moving through the floor
        if (base._grounded && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }

        if (moveVelocity != _targetMoveVelocity)
        {
            _posSinTime = 0;
        }

        _targetMoveVelocity = moveVelocity * base.speed;

        // Smooth player direction
        if (_currentMoveVelocity != _targetMoveVelocity)
        {
            _posSinTime += Time.deltaTime * 40; // Sorry magic # for now
            _posSinTime = Mathf.Clamp(_posSinTime, 0, Mathf.PI);
            float t = Evaluate(_posSinTime);
            moveVelocity = Vector3.Lerp(_currentMoveVelocity, _targetMoveVelocity, t);
        }
        else
        {
            _posSinTime = 0;
        }

        // Move the player
        controller.Move(speed * Time.deltaTime * moveVelocity);
        if (moveVelocity != Vector3.zero)
        {
            transform.forward = moveVelocity;

            if(spin != null)
            {
                spin.enabled = true;
            }
        }
        else
        {
            if (spin != null)
            {
                spin.enabled = false;
            }
        }

        // Apply gravity and velocity
        _playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(_playerVelocity * Time.deltaTime);

        _currentMoveVelocity = moveVelocity;
    }
}
