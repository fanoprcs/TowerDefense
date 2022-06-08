using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour //讓其他class當作T
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<T>();//如果我們傳了一個Object進來, 就可以找到該Object的type並且讓instance等於它
            else if(instance != FindObjectOfType<T>())//如果instance不等於現在的東西且instance不為null, 那麼就毀掉傳進來的東西
                Destroy(FindObjectOfType<T>());
            
            DontDestroyOnLoad(FindObjectOfType<T>()); 
            return instance; 
        }
    }
}
