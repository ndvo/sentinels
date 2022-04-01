using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoryManager : MonoBehaviour
{

    private int _currentText = 0;
    private string[] _story = {
        "It took Earth thousand of years to balance life, culture and technology.",
        "It finally arrived. Earth grew in power and splendour and became a beacon in the sky.",
        "The beacon was noticed.",
        "Unfortunately, instead of a guide it was viewed as an unlimited source of wealth",
        "Wealth to be expropriated, to be used as a shortcut by other ecosystems.",
        "The next step in Earth's struggle is not so dissimilar from the previous ones.",
        "Sentinel, you are Earth's arm to protect, recover, and preserve."
    };
    public GameObject gui;
    public GameObject left;
    public GameObject right;
    private TextMeshProUGUI _textMesh;

    private void Start()
    {
        _textMesh = gui.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        _currentText = Mathf.Clamp(_currentText, 0, _story.Length - 1);
        _textMesh.text = _story[_currentText];
        left.SetActive(_currentText != 0);
        right.SetActive(_currentText != _story.Length - 1);
    }
    
    public void Next()
    {
        _currentText += 1;
    }

    public void Previous()
    {
        _currentText -= 1;
    }
}
