using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RapaxFructus
{
    public class SpecialResourseCounter : MonoBehaviour
    {
        [SerializeField] private Image[] _counter;
        [SerializeField] private Color _colorOn;
        [SerializeField] private Color _colorOff;
        [SerializeField] private Color _colorNo;
        private void OnEnable()
        {
            ManagerDirectory.Instance.Player.ResourseCountChanged += OnResourseCountChanged;
            OnResourseCountChanged();
        }

        private void OnResourseCountChanged()
        {
            int count = ManagerDirectory.Instance.Player.SpecialResourseCount;
            if (!DataManager.Save.CurrentGameData.PowerUps.TryGetValue("Resourses", out int level))
                level = 0;
            level += 3;
            if (_counter == null || _counter.Length != 11)
                return;
            for (int i = 0; i < _counter.Length; i++)
            {
                if (i < count)
                {
                    _counter[i].color = _colorOn;
                }
                else if (i < level)
                {
                    _counter[i].color = _colorOff;
                }
                else
                {
                    _counter[i].color = _colorNo;
                }
            }
        }

        private void OnDisable()
        {
            ManagerDirectory.Instance.Player.ResourseCountChanged -= OnResourseCountChanged;
        }
    }
}