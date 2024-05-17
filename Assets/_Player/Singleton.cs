using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    // Property to get the singleton instance
    public static T Instance
    {
        get
        {
            // Check if the instance is null (not yet initialized or the object was destroyed)
            if (instance == null)
            {
                // Attempt to find an existing instance in the scene
                instance = FindObjectOfType<T>();

                // If no instance was found, create a new GameObject with the singleton component
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    instance = singletonObject.AddComponent<T>();
                }
            }

            // Return the singleton instance
            return instance;
        }
    }

    // Ensure the singleton instance is not destroyed on scene reload (optional)
    protected virtual void Awake()
    {
        if (instance == null)
        {
            // Set the instance to this object if it's the first instance being created
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Destroy duplicate instances that were created during scene reload
            Destroy(gameObject);
        }
    }
}
