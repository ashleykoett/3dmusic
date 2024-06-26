using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CharacterBehavior : MonoBehaviour
{
    public CharacterController controller;

    public float jumpHeight;
    public float jumpFrequency;
    public float speed = 2f;
    public float gravity = -50f;

    private AudioSource audioSource;

    private float _initialPitch;
    private Vector3 _playerVelocity;
    private bool _grounded = false;
    private float _lastJumpTime = 0.0f;


    private void Start()
    {
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        _initialPitch = audioSource.pitch;
    }

    public virtual void MoveCharacter(Vector3 moveVelocity)
    {
        // See if we just hit the ground
        if (!_grounded && controller.isGrounded)
        {
            audioSource.Play();
        }

        _grounded = controller.isGrounded;

        if (_grounded && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            audioSource.pitch = _initialPitch;
        }

        controller.Move(speed * Time.deltaTime * moveVelocity);

        if (moveVelocity != Vector3.zero)
        {
            transform.forward = moveVelocity;

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

        _playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(_playerVelocity * Time.deltaTime);
    }

    public virtual void Jump(float height)
    {
        _playerVelocity.y += Mathf.Sqrt(height * -1.0F * gravity);
        _lastJumpTime = Time.time;
    }
}
