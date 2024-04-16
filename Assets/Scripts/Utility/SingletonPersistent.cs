using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonPersistent<T> : Singleton<T> where T : MonoBehaviour 
{
    protected override void Awake() {
        if (Instance == null)
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
