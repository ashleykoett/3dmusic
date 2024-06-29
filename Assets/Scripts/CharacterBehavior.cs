using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterBehavior : MonoBehaviour
{
    public CharacterController controller;

    public float jumpHeight;
    public float jumpFrequency;
    public float speed = 2f;
    public float gravity = -50f;

    public Vector3 compressedScale = new Vector3(0.8f, 0.8f, 0.8f);
    public Vector3 maxScale = new Vector3(1.2f, 1.2f, 1.2f);

    private AudioSource audioSource;

    private float _initialPitch;
    private Vector3 _playerVelocity;
    private bool _grounded = false;
    private float _lastJumpTime = 0.0f;
    private Vector3 _initialScale;
    private Vector3 _currentScale;
    private Vector3 _targetScale;

    private Vector3 _currentMoveVelocity = Vector3.zero;
    private Vector3 _targetMoveVelocity;

    private float _posSinTime;
    private float _scaleSinTime;
    private bool _soundPlayed = false;
    private bool _jumping = false;


    private void Start()
    {
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        _initialPitch = audioSource.pitch;
        _initialScale = transform.localScale;
        _currentScale = transform.localScale;

        compressedScale = new Vector3(compressedScale.x*_initialScale.x, compressedScale.y*_initialScale.y, compressedScale.z*_initialScale.z);
        maxScale = new Vector3(maxScale.x*_initialScale.x, maxScale.y*_initialScale.y, maxScale.z*_initialScale.z);
    }

    public virtual void MoveCharacter(Vector3 moveVelocity)
    {
        // See if we just hit the ground
        if (!_grounded && controller.isGrounded && !_soundPlayed)
        {
            _jumping = false;
            audioSource.Play();
            _soundPlayed = true;
        }

        _grounded = controller.isGrounded;

        // Stop player from moving through the floor
        if (_grounded && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }

        // Check if we are exiting slow mode
        if(Input.GetKeyUp(KeyCode.Space) && audioSource.pitch != _initialPitch)
        {
            audioSource.pitch = _initialPitch;
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

            if (_jumping)
            {
                // Map character velocity to scale (visual effect)
                float outputX = Mathf.Clamp(Map(_playerVelocity.y, 20, -20, compressedScale.x, maxScale.x), compressedScale.x, maxScale.x);
                float outputY = Mathf.Clamp(Map(_playerVelocity.y, 20, -20, compressedScale.y, maxScale.y), compressedScale.y, maxScale.y);
                float outputZ = Mathf.Clamp(Map(_playerVelocity.y, 20, -20, compressedScale.z, maxScale.z), compressedScale.z, maxScale.z);
            
                _currentScale = new Vector3(outputX, outputY, outputZ);
                transform.localScale = _currentScale;
            }
            

            // Jump
            float timer = Time.time - _lastJumpTime;
            if (_grounded && timer >= jumpFrequency)
            {
                float height = jumpHeight;
                if (Input.GetKey(KeyCode.Space))
                {
                    height = jumpHeight * 2;
                    audioSource.pitch = _initialPitch * 0.8f;
                }
                Jump(height);
            }
        }

        // Apply gravity and velocity
        _playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(_playerVelocity * Time.deltaTime);
    }

    public virtual void Jump(float height)
    {
        _jumping = true;
        _soundPlayed = false;
        _playerVelocity.y += Mathf.Sqrt(height * -1.0F * gravity);
        _lastJumpTime = Time.time;
    }

    /// <summary>
    /// Maps value from range A to range B
    /// </summary>
    private float Map(float value, float minA, float maxA, float minB, float maxB)
    {
        float t = Mathf.InverseLerp(minA, maxA, value);
        float output = Mathf.Lerp(minB, maxB, t);

        return output;
    }

    private float Evaluate(float x)
    {
        return 0.5f * Mathf.Sin(x - Mathf.PI / 2f) + 0.5f;
    }
}
