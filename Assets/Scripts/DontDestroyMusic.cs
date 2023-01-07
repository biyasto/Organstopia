using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyMusic : MonoBehaviour
{
    private static DontDestroyMusic instance;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
        }
        else {
            instance = this;
        }
    }
    // Update is called once per frame
    public void DetroyMusic()
    {
        Destroy(gameObject);
    }
}
