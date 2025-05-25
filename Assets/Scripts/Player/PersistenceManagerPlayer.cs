using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistenceManagerPlayer : MonoBehaviour
{
    public static PersistenceManagerPlayer Instance;
    private Dictionary<string, GameObject> children = new Dictionary<string, GameObject>();

    void Awake()
    {
        // Singleton pattern to prevent duplicates
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reassign references after scene load
        children.Clear(); // Clear old references

        // Add all direct children by name
        foreach (Transform child in transform)
        {
            if (child != null)
            {
                children[child.name] = child.gameObject;
                Debug.Log("Child registered: " + child.name);
            }
        }
    }
}
