using UnityEngine;

/// <summary>
/// Singleton manager class that can be accessed from anywhere in the application.
/// Designed to persist across scene loads and provide centralized access to scene-related functionality.
/// </summary>
public class StaticSceneManager : MonoBehaviour
{
    private static StaticSceneManager instance = null;
    private void OnEnable()
    {
        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            instance = this;
        }
    }
    public static StaticSceneManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("StaticSceneManager").AddComponent<StaticSceneManager>();
            }
            return instance;
        }
    }
}
