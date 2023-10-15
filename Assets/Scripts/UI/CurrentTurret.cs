using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RapaxFructus
{
    internal class CurrentTurret : MonoBehaviour
    {
        [SerializeField] private Text _text;
        private void Start()
        {
            ManagerDirectory.Instance.BuildManager.ObstacleChanged += OnObstacleChanged;
            OnObstacleChanged();
        }

        private void OnObstacleChanged()
        {
            GameEntity obstacle = ManagerDirectory.Instance.BuildManager.Get(
                ManagerDirectory.Instance.LevelGenerator.Floors[ManagerDirectory.Instance.CameraMovement.CurrentLevel - 1].Entrance);
            if (obstacle != null)
            {
                _text.text = obstacle.Name;
                _text.color = obstacle.UIColor;
            }
            else
            {
                _text.text = "Nothing";
                _text.color = Color.gray;
            }
        }
    }
}