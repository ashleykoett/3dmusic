using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerBehavior : CharacterBehavior
{
    public Spin[] spins;
    private bool _moving = false;

    public override void Start()
    {
        soundController = GetComponent<CharacterSoundController>();
        controller = GetComponent<CharacterController>();
        spins = GetComponentsInChildren<Spin>();
    }
    public override void MoveCharacter(Vector3 moveVelocity, bool jumpEnabled = true)
    {
        if (moveVelocity != Vector3.zero && !_moving)
        {
            _moving = true;
            soundController.FadeInSound();
        }

        if (moveVelocity == Vector3.zero && _moving)
        {
            soundController.FadeOutSound();
            _moving = false;
        }

        base.HandleInput();

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

            foreach (var spin in spins)
            {
                spin.enabled = true;
            }
        }
        else
        {
            foreach (var spin in spins)
            {
                spin.enabled = false;
            }
        }

        // Apply gravity and velocity
        _playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(_playerVelocity * Time.deltaTime);

        _currentMoveVelocity = moveVelocity;
    }

    public override void HandleInput()
    {
        float multiplier = 1f;
        // Check if we are exiting slow mode
        if (Input.GetButtonUp("Primary") || Input.GetButtonUp("Secondary") || Input.GetButtonUp("Third") || Input.GetButtonUp("Fourth") || Input.GetButtonUp("LeftTrigger") || Input.GetButtonUp("RightTrigger"))
        {
            soundController.RevertAudio();
            foreach (var spin in spins)
            {
                spin.RevertSpeed();
            }
            return;
        }

        if (Input.GetButton("Primary"))
        {
            if (soundController != null)
                soundController.ShiftAudio(PRIMARY_PITCH_SHIFT);
            multiplier = 0.8f;
        }
        if (Input.GetButton("Secondary"))
        {
            if (soundController != null)
                soundController.ShiftAudio(SECONDARY_PITCH_SHIFT);
            multiplier = 1.2f;
        }
        if (Input.GetButton("Third"))
        {
            if (soundController != null)
                soundController.ShiftAudio(THIRD_PITCH_SHIFT);
            multiplier = 0.6f;
        }
        if (Input.GetButton("Fourth"))
        {
            if (soundController != null)
                soundController.ShiftAudio(FOURTH_PITCH_SHIFT);
            multiplier = 1.5f;
        }
        if (Input.GetButton("LeftTrigger"))
        {
            if (soundController != null)
                soundController.ShiftAudio(LEFT_PITCH_SHIFT);
            multiplier = 0.5f;
        }
        if (Input.GetButton("RightTrigger"))
        {
            if (soundController != null)
                soundController.ShiftAudio(RIGHT_PITCH_SHIFT);
            multiplier = 1.7f;
        }

        if (multiplier != 1)
        {
            foreach (var spin in spins)
            {
                spin.AdjustSpeed(multiplier);
            }
        }
    }
}
