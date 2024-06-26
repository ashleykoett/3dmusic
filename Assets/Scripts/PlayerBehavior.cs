using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public CharacterBehavior characterBehavior;

    private void Start()
    {
        characterBehavior = GetComponent<CharacterBehavior>();
    }

    private void Update()
    {
        if (characterBehavior != null)
        {
            Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            characterBehavior.MoveCharacter(move);
        }
    }
}
