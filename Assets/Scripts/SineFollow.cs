using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SineFollow : Follower
{
    public GameObject player;
    public float movementSpeed = 5f;
    public float amplitude = 5f;
    public float period = 5f;

    public CharacterSoundController characterSoundController;
    public BaseBehavior baseBehavior;

    private float _t = 0f;
    private bool _moving = false;

    public override void Start()
    {
        _initialPosition = transform.position;
        characterSoundController = GetComponent<CharacterSoundController>();
        baseBehavior = GetComponent<BaseBehavior>();
    }

    public override void Update()
    {
        _targetPosition = player.transform.position;
        if(_targetPosition.x < transform.position.x || _targetPosition.x > followDistance)
        {
            if (_moving)
            {
                characterSoundController.FadeOutSound();
            }
            
            _moving = false;
            return;
        }

        if(!_moving)
        {
            characterSoundController.FadeInSound();
            _moving = true;
        }

        baseBehavior.MoveCharacter(Vector3.zero);
        _t += Time.deltaTime;
        Vector3 pos = transform.position;
        pos.x += movementSpeed * Time.deltaTime;
        pos.y = _initialPosition.y + Mathf.Sin(_t / period) * amplitude;

        transform.position = pos;
    }
}
