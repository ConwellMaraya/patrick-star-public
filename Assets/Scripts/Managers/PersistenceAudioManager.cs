using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistenceAudioManager : MonoBehaviour
{
    private Dictionary<string, GameObject> children = new Dictionary<string, GameObject>();
    private PersistenceAudioManager Instance;

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
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Always unsubscribe to prevent multiple calls
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Called automatically when a new scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: " + scene.name);
        Debug.Log("Audio Count: " + transform.childCount);

        children.Clear(); // Clear old references

        // Add all direct children by name
        Shiasewa marker = FindObjectOfType<Shiasewa>();
        if (marker != null)
        {
            marker.transform.SetParent(transform);
            Debug.Log("Child with marker re-parented.");
        }
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
