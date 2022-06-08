using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour //����Lclass��@T
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<T>();//�p�G�ڭ̶ǤF�@��Object�i��, �N�i�H����Object��type�åB��instance����
            else if(instance != FindObjectOfType<T>())//�p�Ginstance������{�b���F��Binstance����null, ����N�����Ƕi�Ӫ��F��
                Destroy(FindObjectOfType<T>());
            
            DontDestroyOnLoad(FindObjectOfType<T>()); 
            return instance; 
        }
    }
}
