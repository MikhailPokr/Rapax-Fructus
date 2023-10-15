using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RapaxFructus
{
    [RequireComponent(typeof(SphereCollider))]
    internal class TurretPoint : MonoBehaviour
    {
        [SerializeField] private const float _radius = 0.05f;
        private LevelFloor _floor;
        private Turret _currentTurret;
        private bool _active = false;

        private void OnEnable()
        {
            GetComponent<SphereCollider>().radius = _radius;
            _floor = GetComponentInParent<LevelFloor>();
            ManagerDirectory.Instance.CameraMovement.PositionChanged += OnCameraPositionChanged;
            ManagerDirectory.Instance.BuildManager.ObstacleChanged += OnObstacleChanged;
            OnCameraPositionChanged();
        }

        private void OnObstacleChanged()
        {
            if (ManagerDirectory.Instance.BuildManager.NoTurret)
            {
                if (_active || _currentTurret == null)
                {
                    return;
                }
                Destroy(_currentTurret.gameObject);
                _currentTurret = null;
            }
        }

        private void OnMouseEnter()
        {
            if (_active || ManagerDirectory.Instance.BuildManager.NoTurret)
                return;
            _currentTurret = Instantiate((Turret)ManagerDirectory.Instance.BuildManager.Get(_floor.Entrance));
            _currentTurret.enabled = false;
            _currentTurret.SetClickable(false);
            MeshRenderer[] meshRenderers = _currentTurret.GetComponentsInChildren<MeshRenderer>();
            _currentTurret.transform.position += -_currentTurret.GetComponentInChildren<AnchorPoint>().transform.position + transform.position;
            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                meshRenderer.material.color = ManagerDirectory.Instance.BuildManager.Color;
            }
        }
        private void OnMouseExit()
        {
            if (_active || _currentTurret == null || ManagerDirectory.Instance.BuildManager.NoTurret)
                return;
            Destroy(_currentTurret.gameObject);
            _currentTurret = null;
        }
        private void OnMouseDown()
        {
            if (_active || _currentTurret == null || ManagerDirectory.Instance.BuildManager.NoTurret)
                return;
            MeshRenderer[] meshRenderers = _currentTurret.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                meshRenderer.material.color = new Color(1,1,1,1);
            }
            _currentTurret.enabled = true;
            _currentTurret.SetClickable(true);
            _currentTurret.MyTurretPoint = this;
            _active = true;
        }
        public void Enable()
        {
            _active = false;
            _currentTurret = null;
        }
        private void OnCameraPositionChanged()
        {
            if (this == null)
                return;
            if (ManagerDirectory.Instance.LevelGenerator.Floors.IndexOf(_floor) != ManagerDirectory.Instance.CameraMovement.CurrentLevel - 1 || ManagerDirectory.Instance.CameraMovement.Position == CameraMovement.CameraPosition.Fruit)
                gameObject.layer = 2; //Raycast Ignore
            else
                gameObject.layer = 0;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.5f, 0, 1, 0.1f);
            Gizmos.DrawSphere(transform.position, 0.1f);
        }
#endif
    }
}