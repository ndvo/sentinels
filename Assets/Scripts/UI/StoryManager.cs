using TMPro;
using UnityEngine;

namespace UI
{
    /// <summary>
    ///     StoryManager controls the display of the game story.
    ///     It provides a button to move forward and a button to move backwards in the story.
    ///     As it is now the story is told only through
    /// </summary>
    public class StoryManager : MonoBehaviour
    {
        public GameObject gui;
        public GameObject left;
        public GameObject right;

        private int _currentText;

        private readonly string[] _story =
        {
            "It took Earth thousand of years to balance life, culture and technology.",
            "It finally arrived. Earth grew in power and splendour and became a beacon in the sky.",
            "The beacon was noticed.",
            "Unfortunately, instead of a guide it was viewed as an unlimited source of wealth.",
            "Wealth to be expropriated, to be used as a shortcut by other ecosystems.",
            "Sentinel, you are Earth's arm to protect, recover, and preserve."
        };

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
}