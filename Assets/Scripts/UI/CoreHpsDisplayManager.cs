using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RapaxFructus
{
    internal class CoreHpsDisplayManager : MonoBehaviour
    {
        [SerializeField] private CoreHpDisplay _coreHPDisplayPrefab;
        [SerializeField] private RectTransform _modelTopPrefab;

        private List<Core> _cores;
        private List<CoreHpDisplay> _coreHPDisplays = new();

        private Dictionary<Core, CoreHpDisplay> _coreDisplay = new();
        public Dictionary<Core, CoreHpDisplay> CoreDisplay => _coreDisplay;

        public void Activate()
        {
            _cores = ManagerDirectory.Instance.Player.Cores;
            _cores.OrderBy(x => x.transform.position.x);
            for (int i = 0; i < _cores.Count; i++)
            {
                _cores[i].HPChanged += OnHPChanged;

                CoreHpDisplay coreHPDisplay = Instantiate(_coreHPDisplayPrefab, transform);
                _coreHPDisplays.Add(coreHPDisplay);
                _coreDisplay.Add(_cores[i], _coreHPDisplays[i]);
                _coreHPDisplays[i].SetSize(_cores[i].Floor.Size);
                _coreHPDisplays[i].SetHP(_cores[i].CurrentHP, _cores[i].MaxHP);
                _coreHPDisplays[i].GetComponent<RectTransform>().localPosition = new Vector2(0, ((_cores.Count - 1)/2f - i) * _coreHPDisplays[i].SizeY);
            }
            RectTransform modelTop = Instantiate(_modelTopPrefab, transform);
            modelTop.localPosition = new Vector2(0, ((_cores.Count - 1) / 2f + 1) * _coreHPDisplayPrefab.SizeY + modelTop.rect.y);
        }

        private void OnHPChanged(Core core)
        {
            _coreDisplay[core].SetHP(core.CurrentHP, core.MaxHP);
        }
    }
}