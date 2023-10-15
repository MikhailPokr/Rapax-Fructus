using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RapaxFructus
{
    internal class ManagerDirectory : SingletonBase<ManagerDirectory>
    {
        public LevelGenerator LevelGenerator;
        public BuildManager BuildManager;
        public CameraMovement CameraMovement;
        public ControlSystem ControlSystem;
        public Player Player;
        [Space]
        public CoreHpsDisplayManager CoreHpsDisplayManager;
        public EnengyDisplay EnengyDisplay;
        public TimeStop TimeStop;
        public CloseEntrances CloseEntrances;
    }
}