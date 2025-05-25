using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistenceManagerEnemy : MonoBehaviour
{
    // This will store a reference to the child
    private Dictionary<string, GameObject> children = new Dictionary<string, GameObject>();
    public static PersistenceManagerEnemy Instance;

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
        Debug.Log("Enemy Count: "+ transform.childCount);

        children.Clear(); // Clear old references

        // Add all direct children by name
        foreach (Transform child in transform)
        {
            if (child != null)
            {
                child.SetParent(transform);
                children[child.name] = child.gameObject;
                Debug.Log("Child registered: " + child.name);

                foreach (Transform child2 in child)
                {
                    GameObject childObject = child.gameObject;
                    childObject = child2.gameObject;
                    Debug.Log("Child of Child registered: " + child2.name);
                    if (child2.name == "Entity_Status_UI")
                    {
                        GameObject child2Object = child2.gameObject;
                        child2Object = child2.GetChild(0).gameObject;
                        Debug.Log("Child of Child of Child registered: " + childObject.name);
                        foreach (Transform child3 in child2Object.transform.GetChild(0))
                        {
                            GameObject middle = child2Object.transform.GetChild(0).gameObject;
                            middle = child3.gameObject;
                            if (child3.name == "Fill Area")
                            {
                                Debug.Log("Fill Area Found");
                                GameObject nested = child3.gameObject;
                                nested = child3.transform.GetChild(0).gameObject;
                                Debug.Log(nested.name + " Linked and " + nested.transform.GetChild(0).name + " Linked");
                            }
                        }
                    }
                }
            }
        }


    }
}
