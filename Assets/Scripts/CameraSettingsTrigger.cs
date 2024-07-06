using DigitalRuby.Tween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSettingsTrigger : MonoBehaviour
{
    public FollowCamera followCamera;
    public bool rotate = true;
    public Quaternion rotation = Quaternion.identity;
    public float animationLength = 1;

    public bool changeOffset = true;
    public Vector3 offset = Vector3.zero;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(rotate)
                followCamera.RotateCamera(rotation, animationLength);
            if(changeOffset)
                followCamera.ChangeOffsetPostion(offset, animationLength);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(rotate)
                followCamera.ResetCameraRotation(animationLength);
            if (changeOffset)
                followCamera.ResetOffsetPosition(animationLength);
        }
    }
}
