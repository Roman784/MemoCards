using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationSettings : MonoBehaviour
{
    public static ApplicationSettings instance;

    private void Awake ()
    {
        if (instance != null && instance != this)
        {
            Destroy (gameObject);
            return;
        }
        instance = this;
        
        DontDestroyOnLoad (gameObject);

        Application.targetFrameRate = 500;
    }
}
