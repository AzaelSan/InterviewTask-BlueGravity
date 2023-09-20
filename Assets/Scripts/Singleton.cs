using UnityEngine;

public class Singleton<T> : ScriptableObject where T : Singleton<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                T[] assets = Resources.LoadAll<T>("");

                if (assets == null || assets.Length < 1)
                {
                    throw new System.Exception("No se encontro ni una instancia de Singleton Scriptable Object en Resources");
                }
                else if (assets.Length > 1)
                {
                    Debug.LogWarning("Multiples instancias de Singleton Scriptable Object detectadas en Resources");
                }

                instance = assets[0];
            }
            return instance;
        }
    }
}
