using UnityEngine;

public enum Dir
{
    X = 'x',
    Y = 'y',
    Z = 'z',
}

public class Spin : MonoBehaviour
{
    public float speed = 5f;
    public Dir axis = Dir.X;

    private float _initialSpeed;
    private float _curentSpeed;

    private void Start()
    {
        _initialSpeed = speed;
        _curentSpeed = speed;
    }

    void LateUpdate()
    {
        switch (axis)
        {
            case Dir.X:
                transform.Rotate(_curentSpeed*Time.deltaTime,0f,0f, Space.Self);
                break;

            case Dir.Y:
                transform.Rotate(0f, _curentSpeed * Time.deltaTime, 0f, Space.Self);
                break;

            case Dir.Z:
                transform.Rotate(0f, 0f, _curentSpeed * Time.deltaTime, Space.Self);
                break;
        }
    }

    public void AdjustSpeed(float multiplier)
    {
        _curentSpeed = speed * multiplier;
    }

    public void RevertSpeed()
    {
        _curentSpeed = speed;
    }
}
