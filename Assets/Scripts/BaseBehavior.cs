using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBehavior : MonoBehaviour
{
    protected const float PRIMARY_PITCH_SHIFT = -2f;
    protected const float SECONDARY_PITCH_SHIFT = 2F;
    protected const float THIRD_PITCH_SHIFT = -4f;
    protected const float FOURTH_PITCH_SHIFT = 4F;
    protected const float LEFT_PITCH_SHIFT = -5F;
    protected const float RIGHT_PITCH_SHIFT = 5;

    public CharacterController controller;
    public CharacterSoundController soundController;

    public virtual void Start()
    {
        soundController = GetComponent<CharacterSoundController>();
        controller = GetComponent<CharacterController>();
    }

    public virtual void Update()
    {
        HandleInput();
    }
    public virtual void MoveCharacter(Vector3 moveVelocity, bool jumpEnabled = true)
    {
        HandleInput();
    }

    public virtual void HandleInput()
    {
        // Check if we are exiting slow mode
        if (Input.GetButtonUp("Primary") || Input.GetButtonUp("Secondary") || Input.GetButtonUp("Third") || Input.GetButtonUp("Fourth") || Input.GetButtonUp("LeftTrigger") || Input.GetButtonUp("RightTrigger"))
        {
            soundController.RevertAudio();
            return;
        }

        if (Input.GetButton("Primary"))
        {
            if (soundController != null)
                soundController.ShiftAudio(PRIMARY_PITCH_SHIFT);
        }
        if (Input.GetButton("Secondary"))
        {
            if (soundController != null)
                soundController.ShiftAudio(SECONDARY_PITCH_SHIFT);
        }
        if (Input.GetButton("Third"))
        {
            if (soundController != null)
                soundController.ShiftAudio(THIRD_PITCH_SHIFT);
        }
        if (Input.GetButton("Fourth"))
        {
            if (soundController != null)
                soundController.ShiftAudio(FOURTH_PITCH_SHIFT);
        }
        if (Input.GetButton("LeftTrigger"))
        {
            if (soundController != null)
                soundController.ShiftAudio(LEFT_PITCH_SHIFT);
        }
        if (Input.GetButton("RightTrigger"))
        {
            if (soundController != null)
                soundController.ShiftAudio(RIGHT_PITCH_SHIFT);
        }
    }

    /// <summary>
    /// Maps value from range A to range B
    /// </summary>
    public float Map(float value, float minA, float maxA, float minB, float maxB)
    {
        float t = Mathf.InverseLerp(minA, maxA, value);
        float output = Mathf.Lerp(minB, maxB, t);

        return output;
    }

    public float Evaluate(float x)
    {
        return 0.5f * Mathf.Sin(x - Mathf.PI / 2f) + 0.5f;
    }
}
