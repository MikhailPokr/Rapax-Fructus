using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace RapaxFructus
{
    [Serializable]
    public class Saver<T>
    {
        public T Data;
        public static bool TryLoad(string filename, out T data)
        {
            if (File.Exists(Path(filename)))
            {
                data = JsonUtility.FromJson<Saver<T>>(File.ReadAllText(Path(filename))).Data;
                return true;
            }
            data = default;
            return false;
        }
        public static void Save(string filename, T data)
        {
            Saver<T> wrapper = new Saver<T> { Data = data };
            File.WriteAllText(Path(filename), JsonUtility.ToJson(wrapper));
        }
        private static string Path(string filename)
        {
            return $"{Application.persistentDataPath}/{filename}";
        }

    }
}