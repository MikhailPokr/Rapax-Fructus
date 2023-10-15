using UnityEngine.SceneManagement;

namespace RapaxFructus
{
    public class SceneHelper
    {
        public enum Scene
        {
            Menu,
            Game,
            Unknown
        }

        public static Scene CurrentScene
        {
            get
            {
                if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0))
                    return Scene.Menu;
                if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(1))
                    return Scene.Game;
                return Scene.Unknown;
            }
        }
        public static void ChangeScene(Scene scene)
        {
            switch(scene)
            {
                case Scene.Menu:
                    SceneManager.LoadScene(0);
                    break;
                case Scene.Game:
                    SceneManager.LoadScene(1);
                    break;
                case Scene.Unknown:
                    throw new System.Exception("Unknow Scene");
            }
        }
    }
}