using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class MusciaFondo : MonoBehaviour
{
    public static MusciaFondo instance;
 
    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}