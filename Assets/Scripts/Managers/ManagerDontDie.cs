using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerDontDie : MonoBehaviour
{
    public static ManagerDontDie instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
