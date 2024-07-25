using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineMove : MonoBehaviour
{
    public string axis = "x";
    public float movementSpeed = 5f;
    public float amplitude = 5f;
    public float period = 5f;

    protected float _t = 0f;
    private Vector3 _initialPosition;

    void Start()
    {
        _initialPosition = transform.position;
    }

    void Update()
    {
        _t += Time.deltaTime;
        Vector3 pos = transform.position;

        if(axis == "x")
        {
            pos.x = _initialPosition.x + Mathf.Sin(_t / period) * amplitude;
        }
        if (axis == "y")
        {
            pos.y = _initialPosition.y + Mathf.Sin(_t / period) * amplitude;
        }
        if (axis == "z")
        {
            pos.z = _initialPosition.z + Mathf.Sin(_t / period) * amplitude;
        }
        

        transform.position = pos;
    }
}
