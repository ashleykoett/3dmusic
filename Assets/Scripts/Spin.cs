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

    void LateUpdate()
    {
        switch (axis)
        {
            case Dir.X:
                transform.Rotate(speed*Time.deltaTime,0f,0f, Space.Self);
                break;

            case Dir.Y:
                transform.Rotate(0f, speed * Time.deltaTime, 0f, Space.Self);
                break;

            case Dir.Z:
                transform.Rotate(0f, 0f, speed * Time.deltaTime, Space.Self);
                break;
        }
    }
}
