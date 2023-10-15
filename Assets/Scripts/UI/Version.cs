using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RapaxFructus
{
    public class Version : MonoBehaviour
    {
        [SerializeField] private Text _text;
        private void OnEnable()
        {
            _text.text = "Game version: "+Application.version;
        }
    }
}