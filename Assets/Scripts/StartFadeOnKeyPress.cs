using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartFadeOnKeyPress : MonoBehaviour
{
    public FadeToNextSceneTrigger fade;
    public string buttonName = "";

    private void Start()
    {
        fade = GetComponent<FadeToNextSceneTrigger>();
    }
    void Update()
    {
        if (Input.GetButton(buttonName))
        {
            fade.StartFade();
        }
    }
}
