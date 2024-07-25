using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBackAndForth : MonoBehaviour
{
    public float speed = 1;
    public float RotAngleY = 45;

    private Quaternion _initialRot;

    private void Start()
    {
        _initialRot = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        float rY = Mathf.SmoothStep(0, RotAngleY, Mathf.PingPong(Time.time * speed, 1));
        transform.rotation = Quaternion.Euler(_initialRot.eulerAngles.x, rY, _initialRot.eulerAngles.z);
    }
}
