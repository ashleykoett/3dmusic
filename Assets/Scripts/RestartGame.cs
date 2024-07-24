using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    void Update()
    {
        if(Input.GetButton("Start"))
        {
            SceneManager.LoadScene(0);
        }
    }
}
