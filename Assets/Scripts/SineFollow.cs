using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SineFollow : MonoBehaviour
{
    public GameObject player;
    public float followDistance;
    public float movementSpeed = 5f;
    public float amplitude = 5f;
    public float period = 5f;

    private float _t = 0f;
    private Vector3 _startPosition;
    private Vector3 _followPosition;
    private Vector3 _targetPosition;

    private void Start()
    {
        _startPosition = transform.position;
    }

    void Update()
    {
        _targetPosition = player.transform.position;
        if(_targetPosition.x < transform.position.x)
        {
            return;
        }

        if(_targetPosition.x > followDistance)
        {
            return;
        }

        _t += Time.deltaTime;
        Vector3 pos = transform.position;
        pos.x += movementSpeed * Time.deltaTime;
        pos.y = _startPosition.y + Mathf.Sin(_t / period) * amplitude;

        transform.position = pos;
    }
}
