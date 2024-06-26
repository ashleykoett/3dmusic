using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject player;
    [Range(-50f, 50f)]
    public float xOffset = 0.0f;
    [Range(-50f, 50f)]
    public float zOffset = 0.0f;

    private Vector3 playerPosition = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        playerPosition = player.transform.position;

        Vector3 pos = new Vector3();
        pos.x = playerPosition.x + xOffset;
        pos.y = transform.position.y;
        pos.z = playerPosition.z + zOffset;

        transform.position = pos;
    }
}
