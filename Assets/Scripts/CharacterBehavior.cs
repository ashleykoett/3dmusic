using System;
using UnityEngine;

public class CharacterBehavior : BaseBehavior
{
    public float jumpHeight;
    public float jumpFrequency;
    public float speed = 2f;
    public float gravity = -50f;

    public Vector3 compressedScale = new Vector3(0.8f, 0.8f, 0.8f);
    public Vector3 maxScale = new Vector3(1.2f, 1.2f, 1.2f);
    
    protected Vector3 _playerVelocity;
    protected bool _grounded = false;
    private float _lastJumpTime = 0.0f;
    private Vector3 _initialScale;
    private Vector3 _currentScale;
    private Vector3 _targetScale;
    protected Vector3 _currentMoveVelocity = Vector3.zero;
    protected Vector3 _targetMoveVelocity;
    protected float _posSinTime;
    protected float _scaleSinTime;
    protected bool _soundPlayed = false;
    protected bool _jumping = false;
    protected float _lastGroundedPosY = 0f;
    protected float _currentJumpHeight = 0f;


    public override void Start()
    {
        controller = GetComponent<CharacterController>();
        _initialScale = transform.localScale;
        _currentScale = transform.localScale;
        _currentJumpHeight = jumpHeight;

        _lastGroundedPosY = transform.position.y;

        compressedScale = new Vector3(compressedScale.x*_initialScale.x, compressedScale.y*_initialScale.y, compressedScale.z*_initialScale.z);
        maxScale = new Vector3(maxScale.x*_initialScale.x, maxScale.y*_initialScale.y, maxScale.z*_initialScale.z);

        soundController = GetComponent<CharacterSoundController>();
    }

    public override void MoveCharacter(Vector3 moveVelocity, bool jumpEnabled = true)
    {
        // See if we just hit the ground
        if (!_grounded && controller.isGrounded && !_soundPlayed)
        {
            _jumping = false;
            _lastGroundedPosY = transform.position.y;

            // PlayNextSound();
            if(soundController != null)
                soundController.PlayNextSound();

            _soundPlayed = true;
        }

        _grounded = controller.isGrounded;

        // Stop player from moving through the floor
        if (_grounded && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }

        // Check if we are exiting slow mode
        if(Input.GetButtonUp("Primary") || Input.GetButtonUp("Secondary") || Input.GetButtonUp("Third") || Input.GetButtonUp("Fourth"))
        {
            soundController.RevertAudio();
            _currentJumpHeight = jumpHeight;
        }

        if(moveVelocity != _targetMoveVelocity)
        {
            _posSinTime = 0;
        }

        _targetMoveVelocity = moveVelocity;
        
        // Smooth player direction
        if (_currentMoveVelocity != _targetMoveVelocity)
        {
            _posSinTime += Time.deltaTime * 20; // Sorry magic # for now
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

        if (moveVelocity.Equals(Vector3.zero) || !_jumping)
        {
            // Scale the player back to normal smoothly
            if (transform.localScale != _initialScale)
            {
                _targetScale = _initialScale;
                _scaleSinTime += Time.deltaTime * 40; // Sorry magic # for now
                _scaleSinTime = Mathf.Clamp(_scaleSinTime, 0, Mathf.PI);
                float t = Evaluate(_scaleSinTime);
                transform.localScale = Vector3.Lerp(_currentScale, _targetScale, t);
            }
            else
            {
                _scaleSinTime = 0;
            }
        }
        if (moveVelocity != Vector3.zero)
        {
            transform.forward = moveVelocity;

            // Jump
            float timer = Time.time - _lastJumpTime;
            if (_grounded && timer >= jumpFrequency)
            {
                HandleInput();
                Jump(_currentJumpHeight);
            }

            if (_jumping)
            {
                // Map character velocity to scale (visual effect)
                float outputX = Mathf.Clamp(Map(_playerVelocity.y, 20, -20, compressedScale.x, maxScale.x), compressedScale.x, maxScale.x);
                float outputY = Mathf.Clamp(Map(_playerVelocity.y, 20, -20, compressedScale.y, maxScale.y), compressedScale.y, maxScale.y);
                float outputZ = Mathf.Clamp(Map(_playerVelocity.y, 20, -20, compressedScale.z, maxScale.z), compressedScale.z, maxScale.z);
            
                _currentScale = new Vector3(outputX, outputY, outputZ);
                transform.localScale = _currentScale;
            }
        }

        // Apply gravity and velocity
        _playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(_playerVelocity * Time.deltaTime);

        _currentMoveVelocity = moveVelocity;
    }

    public override void HandleInput()
    {
        if(Input.GetButton("Primary"))
        {
            _currentJumpHeight = jumpHeight * 2f;
            if (soundController != null)
                soundController.ShiftAudio(PRIMARY_PITCH_SHIFT);
        }
        if (Input.GetButton("Secondary"))
        {
            _currentJumpHeight = jumpHeight * 1.5f;
            if (soundController != null)
                soundController.ShiftAudio(SECONDARY_PITCH_SHIFT);
        }
        if (Input.GetButton("Third"))
        {
            _currentJumpHeight = jumpHeight * 0.8f;
            if (soundController != null)
                soundController.ShiftAudio(THIRD_PITCH_SHIFT);
        }
        if (Input.GetButton("Fourth"))
        {
            _currentJumpHeight = jumpHeight * 2.2f;
            if (soundController != null)
                soundController.ShiftAudio(FOURTH_PITCH_SHIFT);
        }
    }

    public virtual void Jump(float height)
    {
        _jumping = true;
        _soundPlayed = false;
        _playerVelocity.y += Mathf.Sqrt(height * -1.0F * gravity);
        _lastJumpTime = Time.time;
    }

    public float GetLastGroundedPosition()
    {
        return _lastGroundedPosY;
    }
}
