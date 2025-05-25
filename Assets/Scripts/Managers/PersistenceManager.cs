using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.VisualScripting.Metadata;
using UnityEngine.UIElements;

public class PersistentObject : MonoBehaviour
{
    // This will store a reference to the child
    private Dictionary<string, GameObject> children = new Dictionary<string, GameObject>();
    private PersistentObject Instance;

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
        Debug.Log("Manager Count: " + transform.childCount);

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
            if (child != null && (child.name != "AudioManager") )
            {
                children[child.name] = child.gameObject;
                Debug.Log("Child registered: " + child.name);
            }

            else if (child != null && (child.name == "AudioManager"))
            {
                //Children of audio Manager
                children[child.name] = child.gameObject;
                foreach (Transform child2 in child)
                {
                    if (child2 != null)
                    {
                        children[child.name] = child2.gameObject;
                        //SFX AND BFX
                        foreach (Transform child3 in child2)
                        {
                            if (child3 != null)
                            {
                                GameObject g = child2.transform.gameObject;
                                g = child3.gameObject;
                                Debug.Log("FX REGISTERED: " + child3.name);
                            }
                        }
                    }
                }
            }
        }


    }
}
