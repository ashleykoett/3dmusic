using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


// DEPRICATED lol
public class CharacterMovement : MonoBehaviour
{
    public CharacterController controller;
    
    public float jumpHeight;
    public float jumpFrequency;
    public float speed = 2f;
    public float gravity = -50f;

    private AudioSource audioSource;
    private Vector3 _playerVelocity;
    private bool _grounded = true;
    private float _lastJumpTime = 0.0f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // See if we just hit the ground
        if(!_grounded && controller.isGrounded)
        {
            audioSource.Play();
        }

        _grounded = controller.isGrounded;

        if ( _grounded && _playerVelocity.y < 0 )
        {
            _playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        controller.Move(speed * Time.deltaTime * move);

        if(move !=  Vector3.zero )
        {
            transform.forward = move;

            float timer = Time.time - _lastJumpTime;
            if (_grounded && timer >= jumpFrequency)
            {
                float height = jumpHeight;
                if( Input.GetKey(KeyCode.Space) )
                {
                    height = jumpHeight * 2;
                }
                Jump(height);
            }
        }

        _playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(_playerVelocity * Time.deltaTime);

    }

    private void Jump(float height)
    {
        _playerVelocity.y += Mathf.Sqrt(height * -1.0F * gravity);
        _lastJumpTime = Time.time;
    }
}
