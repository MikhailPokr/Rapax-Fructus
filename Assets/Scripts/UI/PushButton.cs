using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RapaxFructus
{
    public class PushButton : MonoBehaviour
    {
        [SerializeField] float _speed;
        [SerializeField] private RectTransform _border1;
        [SerializeField] private RectTransform _border2;
        [SerializeField] private RectTransform _button;
        [SerializeField] private RectTransform _info;
        [SerializeField] private RectTransform _image;

        private bool _stop = true;

        private Vector3 _positionBorder1;
        private Vector3 _positionBorder2;

        
        [SerializeField] private bool _retracted;
        [SerializeField] private bool _vertical;

        private void Update()
        {
            if (_stop)
            {
                _positionBorder1 = _border1.position;
                _positionBorder2 = _border2.position;
                _stop = false;
            }
            if (!_retracted)
            {
                if (_border1.position != _positionBorder2)
                {
                    if (Vector2.Distance(_border1.position, _positionBorder2) > _speed)
                    {
                        if (_vertical)
                            _info.position = new Vector3(_info.position.x, _info.position.y + _speed * (_border1.position.y > _border2.position.y ? -1 : 1));
                        else
                            _info.position = new Vector3(_info.position.x + _speed * (_border1.position.x > _border2.position.x ? -1 : 1), _info.position.y);
                    }
                    else
                    {
                        if (_vertical)
                            _info.position = new Vector3(_info.position.x, _info.position.y + Vector2.Distance(_border1.position, _positionBorder2) * (_border1.position.y > _border2.position.y ? -1 : 1));
                        else
                            _info.position = new Vector3(_info.position.x + Vector2.Distance(_border1.position, _positionBorder2) * (_border1.position.x > _border2.position.x ? -1 : 1), _info.position.y);
                    }
                    return;
                }
                else
                    enabled = false;
            }
            else
            {
                if (_border2.position != _positionBorder1)
                {
                    /*if (Vector2.Distance(_border2.position, _positionBorder1) > _speed)
                        _info.position = new Vector3(_info.position.x + _speed * (_border2.position.x > _border1.position.y ? -1 : 1), _info.position.y);
                    else
                        _info.position = new Vector3(_info.position.x + Vector2.Distance(_border2.position, _positionBorder1) * (_border2.position.x > _border1.position.y ? -1 : 1), _info.position.y);
                    return;*/
                    if (Vector2.Distance(_border2.position, _positionBorder1) > _speed)
                    {
                        if (_vertical)
                            _info.position = new Vector3(_info.position.x, _info.position.y + _speed * (_border2.position.y > _border1.position.y ? -1 : 1));
                        else
                            _info.position = new Vector3(_info.position.x + _speed * (_border2.position.x > _border1.position.x ? -1 : 1), _info.position.y);
                    }
                    else
                    {
                        if (_vertical)
                            _info.position = new Vector3(_info.position.x, _info.position.y + Vector2.Distance(_border2.position, _positionBorder1) * (_border2.position.y > _border1.position.y ? -1 : 1));
                        else
                            _info.position = new Vector3(_info.position.x + Vector2.Distance(_border2.position, _positionBorder1) * (_border2.position.x > _border1.position.x ? -1 : 1), _info.position.y);
                    }
                    return;
                }
                else
                    enabled = false;
            }
            _stop = true;
            _retracted = !_retracted;
            _image.rotation = Quaternion.Euler(0, 0, _image.rotation.eulerAngles.z - 180);
        }
    }
}