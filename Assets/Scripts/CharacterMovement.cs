using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public CharacterController controller;
    
    public float jumpHeight;
    public float jumpFrequency;
    public float speed = 2f;
    public float gravity = -50f;

    private Vector3 _playerVelocity;
    private bool _grounded = true;
    private float _lastJumpTime = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Don't move on the ground.
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
