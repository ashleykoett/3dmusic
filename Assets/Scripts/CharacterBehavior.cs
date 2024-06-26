using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBehavior : MonoBehaviour
{
    public readonly string Name;
    public CharacterController controller;

    public float jumpHeight;
    public float jumpFrequency;
    public float speed = 2f;
    public float gravity = -50f;

    private AudioSource audioSource;

    private Vector3 _playerVelocity;
    private bool _grounded = true;
    private float _lastJumpTime = 0.0f;

    private Vector3 _moveVelocity = Vector3.zero;

    public CharacterBehavior(string name, CharacterController controller, float jumpHeight, float jumpFrequency, float speed, float gravity, AudioSource audioSource)
    {
        Name = name;
        this.controller = controller;
        this.jumpHeight = jumpHeight;
        this.jumpFrequency = jumpFrequency;
        this.speed = speed;
        this.gravity = gravity;
        this.audioSource = audioSource;
    }

    public Vector3 GetMoveVelocity()
    {
        return _moveVelocity;
    }

    public void SetMoveVelocity(Vector3 v)
    {
        _moveVelocity = v;
    }

    public virtual void MoveCharacter(Vector3 pos)
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

        controller.Move(speed * Time.deltaTime * _moveVelocity);

        if (_moveVelocity != Vector3.zero)
        {
            transform.forward = _moveVelocity;

            float timer = Time.time - _lastJumpTime;
            if (_grounded && timer >= jumpFrequency)
            {
                float height = jumpHeight;
                if (Input.GetKey(KeyCode.Space))
                {
                    height = jumpHeight * 2;
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
