using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeToNextSceneTrigger : MonoBehaviour
{
    public GameObject fadeUI;
    public Image fadeImage;
    public int sceneIndex = 1;
    public float fadeLength = 1f;

    private bool _startFade;
    private float _timer = 0f;
    private float _t = 0f;
    private Color _fadeColor = Color.black;

    private void Start()
    {
        fadeImage = fadeUI.GetComponent<Image>();
    }

    private void Update()
    {
        if (_startFade)
        {
            _timer += Time.deltaTime;
            _t = Mathf.Clamp(_timer / fadeLength, 0f, 1f);

            _fadeColor = fadeImage.color;
            _fadeColor.a = Mathf.Lerp(0f, 1f, _t);

            fadeImage.color = _fadeColor;

            if(_t == 1f)
            {
                _startFade = false;
                SceneManager.LoadScene(sceneIndex);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _startFade = true;
        }
    }
}
