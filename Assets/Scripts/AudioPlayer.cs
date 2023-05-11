using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioPlayer : MonoBehaviour
{
    void Awake()
    {
        ManageSingleton();
    }

    void ManageSingleton()
    {
        int instanceCount = FindObjectsOfType(GetType()).Length;
        
        if (instanceCount > 1) 
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

    }
}
