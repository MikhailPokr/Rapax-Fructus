using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RapaxFructus
{
    public class FatalLose : MonoBehaviour
    {
        private void OnEnable()
        {
            DataManager.Save.CurrentGameData.Clear();
            DataManager.SaveAll();
        }
        public void ClickToMenu()
        {
            SceneHelper.ChangeScene(SceneHelper.Scene.Menu);
        }
    }
}