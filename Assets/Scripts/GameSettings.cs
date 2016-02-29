using UnityEngine;
using System.Collections;

public class GameSettings : MonoBehaviour
{
    public static GameSettings instance
    {
        get;
        private set;
    }

    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
