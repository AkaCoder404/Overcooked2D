using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T instance; // 
    public static T Instance
    {
        get
        {

            if (instance != null)
            {
                Debug.Log(typeof(T).ToString() + " instance already exists");
                return instance;
            }

            //  Find existing instance
            var objects = FindObjectsOfType(typeof(T)) as T[];
            if (objects.Length > 0)
            {
                instance = objects[0];
            }

            if (objects.Length > 1)
            {
                Debug.LogError("There is more than one " + typeof(T).ToString() + " in the scene");
            }

            if (instance != null) return instance;

            // Create a new instance if one doesn't exist
            Debug.Log("[Singleton] An instance of " + typeof(T) + " is needed in the scene, so a new one was created");
            GameObject obj = new GameObject();
            obj.name = $"{typeof(T)} Singleton";
            instance = obj.AddComponent<T>();
            return instance;
        }
    }
}