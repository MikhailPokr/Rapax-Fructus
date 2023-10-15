using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RapaxFructus
{
    public class SettingMenu : MonoBehaviour
    {
        public void DeleteFruit()
        {
            DataManager.Save.CurrentGameData.Clear();
            DataManager.SaveAll();
            SceneHelper.ChangeScene(SceneHelper.Scene.Menu);
        }
        public void ClearData()
        {
            DataManager.DeleteAll();
            PlayerPrefs.DeleteAll();
            SceneHelper.ChangeScene(SceneHelper.Scene.Menu);
        }
    }
}