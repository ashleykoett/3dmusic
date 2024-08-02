using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowTextOnTrigger : MonoBehaviour
{
    public GameObject go;
    public Text textComp;
    public string text = "";
    public float duration = 3f;
    public bool repeatable = false;

    private bool _showText = false;
    private bool _shown = false;
    private float _timer = 0f;

    private void Start()
    {
        textComp = go.GetComponent<Text>();
    }

    private void Update()
    {
        if(_showText)
        {
            _timer += Time.deltaTime;
            if(_timer >= duration)
            {
                go.SetActive(false);
                _showText = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!repeatable && _shown) { return; }

        if(other.gameObject.CompareTag("Player"))
        {
            _showText = true;
            textComp.text = text;
            go.SetActive(true);
        }
    }
}
