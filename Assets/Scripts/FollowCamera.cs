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

    [Range(0.01f, 5f)]
    public float smoothing =0.1f;

    private float _targetYOffset = 0f;
    private Vector3 playerPosition = Vector3.zero;
    private Vector3 _currentOffset = Vector3.zero;

    private float _t = 0f;

    private void Start()
    {
        characterBehavior = player.GetComponent<CharacterBehavior>();
    }
    void Update()
    {
        playerPosition = player.transform.position;

        float targetOffset = characterBehavior.GetLastGroundedPosition() + yOffset;

        if (targetOffset != _targetYOffset)
        {
            _t = 0;
        }

        _targetYOffset = targetOffset;

        float t = _t + Time.deltaTime * smoothing;
        _t = Mathf.Clamp(t, 0, 1);

        _currentOffset.y = Mathf.Lerp(transform.position.y, _targetYOffset, _t);

        _currentOffset.x = playerPosition.x + xOffset;
        _currentOffset.z = playerPosition.z + zOffset;

        transform.position = _currentOffset;
    }

    private float Evaluate(float x)
    {
        return 0.5f * Mathf.Sin(x - Mathf.PI / 2f) + 0.5f;
    }
}
