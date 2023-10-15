using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace RapaxFructus
{
    internal class PauseButton : MonoBehaviour
    {
        [SerializeField] private Text _text;
        [SerializeField] private Image _image;
        [SerializeField] private Color _colorOn;
        [SerializeField] private Color _colorOff;
        private void Start()
        {
            ManagerDirectory.Instance.TimeStop.TimeStopNotify += OnTimeStop;
        }

        private void OnTimeStop(bool stop)
        {
            _image.color = stop ? _colorOff : _colorOn;
            _text.text = stop ? "Play" : "Pause";
            _text.color = stop ? _colorOn : _colorOff;
        }
    }
}