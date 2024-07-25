using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnButtonPress : MonoBehaviour
{
    public int sceneIndex = 0;
    public string buttonName = "";
    void Update()
    {
        if (Input.GetButton(buttonName))
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
